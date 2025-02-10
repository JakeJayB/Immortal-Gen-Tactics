using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnSystem : MonoBehaviour
{
    [SerializeField]
    private MapCursor mapCursor;
    private List<Unit> units = new List<Unit>();
    private bool startLoop = true;
    private bool continueLoop = false;
    

    IEnumerator TurnLoop()
    {
        while (startLoop)
        {
            // TODO: Sort units if any unit's speed changes 
            // TODO: Turn List of units into a Queue
            foreach (Unit unit in units)
            {
                // TODO: If unit is not dead or enemy and Ally is not AI-controlled, send signal to MapCursor
                mapCursor.ActivateMove(unit.CellLocation);
                Debug.Log("TurnSystem: Unit's turn at " + unit.CellLocation);

                // Unity stop execution here until continueLoop turn to true. 
                yield return new WaitUntil(() => continueLoop);
                continueLoop = false;
                yield return new WaitForSeconds(2);
            }
        }
    }

    public void ContinueLoop()
    {
        this.continueLoop = true;
    }

    public void InitializeUnits(List<Unit> units)
    {
        /* This function is called from TilemapCreator */ 
        
        // TODO: Sort units based on speed
        this.units = units;
        StartCoroutine(TurnLoop());
    }
}
