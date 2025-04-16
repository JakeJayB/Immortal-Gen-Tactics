using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class UnitPriorityQueue
{
    private List<Unit> heap = new List<Unit>();

    public int Count => heap.Count;

    public void Enqueue(Unit unit)
    {
        heap.Add(unit);
        HeapifyUp(heap.Count - 1);
        PrintSpeeds();
    }

    public Unit Dequeue()
    {
        if (heap.Count == 0) return null;

        Unit root = heap[0];
        heap[0] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1);
        HeapifyDown(0);

        return root;
    }

    private void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (heap[index].unitInfo.currentAP <= heap[parent].unitInfo.currentAP)
                break;

            Swap(index, parent);
            index = parent;
        }
    }

    private void HeapifyDown(int index)
    {
        int lastIndex = heap.Count - 1;
        while (index < heap.Count)
        {
            int left = index * 2 + 1;
            int right = index * 2 + 2;
            int largest = index;

            if (left <= lastIndex && heap[left].unitInfo.currentAP > heap[largest].unitInfo.currentAP)
                largest = left;

            if (right <= lastIndex && heap[right].unitInfo.currentAP > heap[largest].unitInfo.currentAP)
                largest = right;

            if (largest == index) break;

            Swap(index, largest);
            index = largest;
        }
    }

    private void Swap(int a, int b)
    {
        Unit temp = heap[a];
        heap[a] = heap[b];
        heap[b] = temp;
    }

    public List<Unit> ToSortedList()
    {
        List<Unit> sortedList = new List<Unit>(heap);
        sortedList.Sort((a, b) => b.unitInfo.currentAP.CompareTo(a.unitInfo.currentAP));
        return sortedList;
    }


    public void PrintSpeeds()
    {
        var unitList = ToSortedList();
        for (int i = 0; i < unitList.Count; i++)
        {
            Debug.Log(unitList[i].name + " AP: " + unitList[i].unitInfo.currentAP);
        }
    }
}



public class TurnSystem : MonoBehaviour
{
    [SerializeField] private MapCursor mapCursor;
    private static bool startLoop = false;
    private static bool continueLoop = false;
    public static Unit CurrentUnit;

    public IEnumerator TurnLoop()
    {
        MapCursor.SelectStartPositions();
        yield return new WaitUntil(() => startLoop);

        UnitPriorityQueue unitQueue = new UnitPriorityQueue();
        foreach (Unit unit in new List<Unit>(TilemapCreator.UnitLocator.Values))
            unitQueue.Enqueue(unit);

        TurnCycle.InitializeCycle(unitQueue.ToSortedList());

        while (startLoop && unitQueue.Count > 0)
        {
            CurrentUnit = unitQueue.Dequeue(); // Set the new unit whose turn is about to start
            CurrentUnit.unitInfo.RefreshAP();  // Unit AP refreshes to max amount at start of new turn

            if (CurrentUnit.GetComponent<EnemyUnit>())
            {
                mapCursor.gameObject.SetActive(false);
                CurrentUnit.GetComponent<EnemyUnit>().StartTurn();
            }
            else
            {
                mapCursor.gameObject.SetActive(true);
                Vector3Int unitLocation = CurrentUnit.unitInfo.CellLocation;
                CameraMovement.SetFocusPoint(TilemapCreator.TileLocator[new Vector2Int(unitLocation.x, unitLocation.z)].TileObj.transform);
                MapCursor.StartMove(unitLocation);
            }

            // Unity stops execution here until continueLoop turn to true. 
            yield return new WaitUntil(() => continueLoop);
            continueLoop = false;
            unitQueue.Enqueue(CurrentUnit);
            TurnCycle.CycleUnits(unitQueue.ToSortedList());
            yield return new WaitForSeconds(1f);
        }
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

    public static void ContinueLoop() => continueLoop = true;

    public static void StartLoop() => startLoop = true;

    public static void StopLoop() => startLoop = false;

}
