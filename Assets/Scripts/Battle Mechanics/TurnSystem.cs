using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        List<Unit> units = new List<Unit>(TilemapCreator.UnitLocator.Values);
        units.Sort((a, b) => a.unitInfo.finalSpeed.CompareTo(b.unitInfo.finalSpeed));

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
            }
        }
    }

    public static void ContinueLoop() => continueLoop = true;

    public static void StartLoop() => startLoop = true;

    public static void StopLoop() => startLoop = false;

}
