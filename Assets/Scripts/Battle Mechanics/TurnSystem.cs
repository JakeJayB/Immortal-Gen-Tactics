using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    [SerializeField]
    private MapCursor mapCursor;
    private List<Unit> units = new List<Unit>();
    private static bool startLoop = false;
    private static bool continueLoop = false;
    

    IEnumerator TurnLoop()
    {
        if(units.Count == 0)
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
                mapCursor.ActivateMove(unit.unitInfo.CellLocation);
                Debug.Log("TurnSystem: Unit's turn at " + unit.unitInfo.CellLocation);

                // Unity stop execution here until continueLoop turn to true. 
                yield return new WaitUntil(() => continueLoop);
                continueLoop = false;
                yield return new WaitForSeconds(2);
            }
        }
    }

    public static void ContinueLoop()
    {
        continueLoop = true;
    }

    public static void StopLoop()
    {
        startLoop = false;
    }

    public void InitializeUnits(List<Unit> units)
    {         
        this.units = units;
        this.units.Sort((a, b) => a.unitInfo.finalSpeed.CompareTo(b.unitInfo.finalSpeed));
        startLoop = true;
        StartCoroutine(TurnLoop());
    }
}
