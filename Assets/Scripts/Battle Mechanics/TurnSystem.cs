using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UnitPriorityQueue
{
    private static List<Unit> Heap = new List<Unit>();

    public int Count => Heap.Count;

    public void Enqueue(Unit unit)
    {
        if(unit == null)
        {
            Debug.LogError("TurnSystem: Attempted to add a null unit to the queue. is Unit dead?");
            Heapify();
            return;
        }

        unit.unitInfo.currentCT = 0;
        Heap.Add(unit);
        HeapifyUp(Heap.Count - 1);
    }

    public Unit Dequeue()
    {
        if (Heap.Count == 0) return null;

        ExecuteTick();
        TurnCycle.CycleUnits(ToSortedList());

        Unit root = Heap[0];
        Heap[0] = Heap[Heap.Count - 1];
        Heap.RemoveAt(Heap.Count - 1);
        HeapifyDown(0);
        return root;
    }

    public int PeekCT()
    {
        if (Heap.Count == 0) return -1;
        return Heap[0].unitInfo.currentCT;
    }

    private bool IsHigherPriority(Unit a, Unit b)
    {
        if (a.unitInfo.currentCT > b.unitInfo.currentCT) return true;
        if (a.unitInfo.currentCT < b.unitInfo.currentCT) return false;
        return a.unitInfo.finalSpeed > b.unitInfo.finalSpeed;
    }


private void HeapifyUp(int index)
{
    while (index > 0)
    {
        int parent = (index - 1) / 2;
        if (!IsHigherPriority(Heap[index], Heap[parent]))
            break;

        Swap(index, parent);
        index = parent;
    }
}

    private void HeapifyDown(int index)
    {
        int lastIndex = Heap.Count - 1;
        while (index < Heap.Count)
        {
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

    private void Swap(int a, int b)
    {
        Unit temp = Heap[a];
        Heap[a] = Heap[b];
        Heap[b] = temp;
    }

    public void Heapify()
    {
        for (int i = (Heap.Count / 2) - 1; i >= 0; i--)
            HeapifyDown(i);
    }

    public void Remove(Unit unit)
    {
        Debug.Log($"TurnSystem: Unit {unit.name} removed from queue");
        Heap.Remove(unit);
        Heapify();
        TurnCycle.CycleUnits(ToSortedList(true));
    }

    public void ExecuteTick()
    {
        do
        {
            foreach(Unit unit in Heap)
            {
                unit.unitInfo.currentCT = Mathf.Clamp(unit.unitInfo.currentCT + unit.unitInfo.finalSpeed, 0, 100);
            }
            Heapify();
        } while (PeekCT() != 100);
    }


    public List<Unit> ToList()
    {
        return new List<Unit>(Heap);
    }

    public List<Unit> ToSortedList(bool includeCurrentUnit = false)
    {
        List<Unit> sortedList = new List<Unit>(Heap);
        if(includeCurrentUnit && TurnSystem.CurrentUnit != null)
            sortedList.Add(TurnSystem.CurrentUnit);
       
        sortedList.Sort((a, b) => {
            int ctComparison = b.unitInfo.currentCT.CompareTo(a.unitInfo.currentCT);
            if (ctComparison == 0)
                return b.unitInfo.finalSpeed.CompareTo(a.unitInfo.finalSpeed); // Tie-breaker
            return ctComparison;
        });
        return sortedList;
    }
    
    public void PrintUnits()
    {
        Debug.Log("-------------------------------------------");
        foreach(Unit unit in Heap)
        {
            Debug.Log($"{unit.name}'s CT: {unit.unitInfo.currentCT}");
        }
        Debug.Log("-------------------------------------------");
    }
}



public class TurnSystem : MonoBehaviour
{
    [SerializeField] private MapCursor mapCursor;
    private static bool startLoop = false;
    private static bool continueLoop = false;
    public static Unit CurrentUnit;
    public static UnitPriorityQueue unitQueue { get; private set; }

    public IEnumerator TurnLoop()
    {
        MapCursor.SelectStartPositions();
        yield return new WaitUntil(() => startLoop);

        unitQueue = new UnitPriorityQueue();
        foreach (Unit unit in new List<Unit>(TilemapCreator.UnitLocator.Values))
            unitQueue.Enqueue(unit);

        unitQueue.ExecuteTick();
        TurnCycle.InitializeCycle(unitQueue.ToSortedList());

        while (startLoop && unitQueue.Count > 0)
        {
            CurrentUnit = unitQueue.Dequeue(); // Set the new unit whose turn is about to start
            CurrentUnit.unitInfo.RefreshAP();  // Unit AP refreshes to max amount at start of new turn

            if (CurrentUnit.GetComponent<EnemyUnit>())
            {
                MapCursor.SetGameObjInactive();
                Vector2Int unitLocation = CurrentUnit.unitInfo.Vector2CellLocation();
                CameraMovement.CheckAndMove(TilemapCreator.TileLocator[unitLocation].TileObj.transform);
                CurrentUnit.GetComponent<EnemyUnit>().StartTurn();
            }
            else
            {
                MapCursor.SetGameObjActive();
                Vector2Int unitLocation = CurrentUnit.unitInfo.Vector2CellLocation();
                CameraMovement.CheckAndMove(TilemapCreator.TileLocator[unitLocation].TileObj.transform);
                MapCursor.StartMove(unitLocation);
            }

            // Unity stops execution here until continueLoop turn to true. 
            yield return new WaitUntil(() => continueLoop);

            continueLoop = false;
            unitQueue.Enqueue(CurrentUnit);
            CurrentUnit = null;
            yield return new WaitForSeconds(1f);
        }
    }

    private static void IsGameOver()
    {
        bool enemyAlive = false;
        bool allyAlive = false;

        foreach (Unit unit in TilemapCreator.UnitLocator.Values)
        {
            if (unit.unitInfo.IsDead())
                continue;

            if (unit.unitInfo.UnitAffiliation == UnitAffiliation.Enemy)
                enemyAlive = true;
            else if (unit.unitInfo.UnitAffiliation == UnitAffiliation.Player)
                allyAlive = true;

            // If both sides have at least one living unit, we can stop early
            if (enemyAlive && allyAlive)
                break;
        }

        if (!enemyAlive)
        {
            Debug.Log("TurnSystem: Player wins!");
            SceneManager.LoadScene(0);
        }
        if (!allyAlive)
        {
            Debug.Log("TurnSystem: Enemy wins!");
            SceneManager.LoadScene(0);
        }
    }

    public static void RemoveUnit(Unit unit)
    {
        if (CurrentUnit == unit)
            CurrentUnit = null;

        TurnCycle.RemoveUnit(unit);
        unitQueue.Remove(unit);

        IsGameOver();
    }

    public static void ContinueLoop() => continueLoop = true;

    public static void StartLoop() => startLoop = true;

    public static void StopLoop() => startLoop = false;

}

    /*    public IEnumerator TurnLoop()
        {
            MapCursor.SelectStartPositions();
            yield return new WaitUntil(() => startLoop);

            List<Unit> units = new List<Unit>(TilemapCreator.UnitLocator.Values);
            units.Sort((a, b) => a.unitInfo.finalSpeed.CompareTo(b.unitInfo.finalSpeed));

            TurnCycle.InitializeCycle(units);


            if (units.Count == 0)
            {
                Debug.LogError("TurnSystem: No units to loop through");
                yield break;
            }

            while (startLoop)
            {

                // TODO: Sort units if any unit's speed changes 
                // TODO: Turn List of units into a Queue
                foreach (Unit unit in units)
                {
                    // TODO: If unit is not dead or enemy and Ally is not AI-controlled, send signal to MapCursor

                    CurrentUnit = unit; // Set the new unit whose turn is about to start
                    unit.unitInfo.RefreshAP(); // Unit AP refreshes to max amount at start of new turn

                    if (unit.GetComponent<EnemyUnit>())
                    {
                        mapCursor.gameObject.SetActive(false);
                        unit.GetComponent<EnemyUnit>().StartTurn();
                    }
                    else
                    {
                        mapCursor.gameObject.SetActive(true);
                        Vector3Int unitLocation = unit.unitInfo.CellLocation;
                        CameraMovement.SetFocusPoint(TilemapCreator.TileLocator[new Vector2Int(unitLocation.x, unitLocation.z)].TileObj.transform);
                        MapCursor.StartMove(unitLocation);
                    }

                    Debug.Log("TurnSystem: Unit's turn at " + unit.unitInfo.CellLocation);

                    // Unity stop execution here until continueLoop turn to true. 
                    yield return new WaitUntil(() => continueLoop);
                    continueLoop = false;
                    yield return new WaitForSeconds(1f);
                    TurnCycle.CycleUnits(units);
                }
            }
        }*/