using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitPriorityQueue {
    private static List<Unit> Heap;
    public int Count => Heap.Count;
    
    public UnitPriorityQueue() {
        Heap = new List<Unit>();
    }
    
    public static void Clear() { Heap = null; }

    public void Enqueue(Unit unit) {
        if (unit == null) {
            Debug.LogError("TurnSystem: Attempted to add a null unit to the queue. is Unit dead?");
            Heapify();
            return;
        }

        unit.UnitInfo.currentCT = 0;
        Heap.Add(unit);
        HeapifyUp(Heap.Count - 1);
    }

    public Unit Dequeue() {
        if (Heap.Count == 0) return null;

        ExecuteTick();
        TurnCycle.CycleUnits(ToSortedList());

        Unit root = Heap[0];
        Heap[0] = Heap[Heap.Count - 1];
        Heap.RemoveAt(Heap.Count - 1);
        HeapifyDown(0);
        return root;
    }

    public int PeekCT() {
        if (Heap.Count == 0) return -1;
        return Heap[0].UnitInfo.currentCT;
    }

    private static bool IsHigherPriority(Unit a, Unit b) {
        if (a.UnitInfo.currentCT > b.UnitInfo.currentCT) return true;
        if (a.UnitInfo.currentCT < b.UnitInfo.currentCT) return false;
        return a.UnitInfo.FinalSpeed > b.UnitInfo.FinalSpeed;
    }


private void HeapifyUp(int index) {
    while (index > 0) {
        int parent = (index - 1) / 2;
        if (!IsHigherPriority(Heap[index], Heap[parent]))
            break;

        Swap(index, parent);
        index = parent;
    }
}

    private static void HeapifyDown(int index) {
        int lastIndex = Heap.Count - 1;
        
        while (index < Heap.Count) {
            int left = index * 2 + 1;
            int right = index * 2 + 2;
            int largest = index;

            if (left <= lastIndex && IsHigherPriority(Heap[left], Heap[largest]))
                largest = left;

            if (right <= lastIndex && IsHigherPriority(Heap[right], Heap[largest]))
                largest = right;

            if (largest == index) break;

            Swap(index, largest);
            index = largest;
        }
    }

    private static void Swap(int a, int b) { (Heap[a], Heap[b]) = (Heap[b], Heap[a]); }

    public static void Heapify() {
        for (int i = (Heap.Count / 2) - 1; i >= 0; i--)
            HeapifyDown(i);
    }

    public static void Add(Unit unit) {
        Debug.Log($"TurnSystem: Unit {unit.GameObj.name} added to queue");
        Heap.Add(unit);
        Heapify();
        TurnCycle.CycleUnits(ToSortedList(true));
    }
    
    public void Remove(Unit unit) {
        Debug.Log($"TurnSystem: Unit {unit.GameObj.name} removed from queue");
        Heap.Remove(unit);
        Heapify();
        TurnCycle.CycleUnits(ToSortedList(true));
    }

    public void ExecuteTick() {
        do {
            foreach(Unit unit in Heap) {
                unit.UnitInfo.currentCT = Mathf.Clamp(unit.UnitInfo.currentCT + unit.UnitInfo.FinalSpeed, 0, 100);
            }
            Heapify();
        } while (PeekCT() != 100);
    }

    public static List<Unit> ToSortedList(bool includeCurrentUnit = false) {
        List<Unit> sortedList = new List<Unit>(Heap);
        if(includeCurrentUnit && !TurnSystem.CurrentUnit.UnitInfo.IsDead())
            sortedList.Add(TurnSystem.CurrentUnit);
       
        sortedList.Sort((a, b) => {
            int ctComparison = b.UnitInfo.currentCT.CompareTo(a.UnitInfo.currentCT);
            if (ctComparison == 0)
                return b.UnitInfo.FinalSpeed.CompareTo(a.UnitInfo.FinalSpeed); // Tie-breaker
            return ctComparison;
        });
        return sortedList;
    }
    
    public void PrintUnits() {
        Debug.Log("-------------------------------------------");
        foreach(Unit unit in Heap) {
            Debug.Log($"{unit.GameObj.name}'s CT: {unit.UnitInfo.currentCT}");
        }
        Debug.Log("-------------------------------------------");
    }
}



public class TurnSystem : MonoBehaviour {
    [SerializeField] private MapCursor mapCursor;
    private static bool startLoop = false;
    private static bool continueLoop = false;
    public static Unit CurrentUnit;
    public static UnitPriorityQueue unitQueue { get; private set; }

    public static void Clear() {
        unitQueue = null;
        CurrentUnit = null;
        startLoop = false;
        continueLoop = false;
    }

    public static void RegisterCleanup() {
        MemoryManager.AddListeners(Clear);
        MemoryManager.AddListeners(UnitPriorityQueue.Clear);
    }


    public IEnumerator TurnLoop() {
        MapCursor.SelectStartPositions();
        yield return new WaitUntil(() => startLoop);

        unitQueue = new UnitPriorityQueue();
        foreach (Unit unit in new List<Unit>(TilemapCreator.UnitLocator.Values))
            unitQueue.Enqueue(unit);

        unitQueue.ExecuteTick();
        TurnCycle.InitializeCycle(UnitPriorityQueue.ToSortedList());

        while (startLoop && unitQueue.Count > 0) {
            CurrentUnit = unitQueue.Dequeue(); // Set the new unit whose turn is about to start
            CurrentUnit.UnitInfo.RefreshAP();  // Unit AP refreshes to max amount at start of new turn

            if (CurrentUnit is AIUnit enemyUnit) {
                MapCursor.SetGameObjInactive();
                Vector2Int unitLocation = CurrentUnit.UnitInfo.Vector2CellLocation();
                CameraMovement.CheckAndMove(TileLocator.SelectableTiles[unitLocation].TileObj.transform);
                enemyUnit.AIUnitBehavior.StartTurn(enemyUnit);
            }
            else {
                MapCursor.SetGameObjActive();
                Vector2Int unitLocation = CurrentUnit.UnitInfo.Vector2CellLocation();
                CameraMovement.CheckAndMove(TileLocator.SelectableTiles[unitLocation].TileObj.transform);
                MapCursor.StartMove(unitLocation);
            }

            // Unity stops execution here until continueLoop turn to true. 
            yield return new WaitUntil(() => continueLoop);

            continueLoop = false;
            if (!CurrentUnit.UnitInfo.IsDead()) unitQueue.Enqueue(CurrentUnit);
            CurrentUnit = null;
            yield return new WaitForSeconds(1f);
        }
    }

    public static void IsGameOver() {
        bool enemyAlive = false;
        bool allyAlive = false;

        foreach (Unit unit in TilemapCreator.UnitLocator.Values) {
            if (unit.UnitInfo.IsDead()) continue;

            if (unit.UnitInfo.UnitAffiliation == UnitAffiliation.Enemy)
                enemyAlive = true;
            else if (unit.UnitInfo.UnitAffiliation == UnitAffiliation.Player)
                allyAlive = true;

            // If both sides have at least one living unit, we can stop early
            if (enemyAlive && allyAlive) break;
        }

        if (!enemyAlive) {
            Debug.Log("TurnSystem: Player wins!");
            GameOver.ShowMenu(GameResult.Win);
        }
        if (!allyAlive) {
            Debug.Log("TurnSystem: Enemy wins!");
            GameOver.ShowMenu(GameResult.Lose);
        }
    }

    public static void AddUnit(Unit unit) {
        unit.UnitInfo.currentCT = 0;
        CycleUnitIcons.AddUnit(unit);
        UnitPriorityQueue.Add(unit);
    }
    
    public static void RemoveUnit(Unit unit) {
        TurnCycle.RemoveUnit(unit);
        unitQueue.Remove(unit);
    }

    public static void ContinueLoop() => continueLoop = true;
    public static void StartLoop() => startLoop = true;
    public static void StopLoop() => startLoop = false;

}

