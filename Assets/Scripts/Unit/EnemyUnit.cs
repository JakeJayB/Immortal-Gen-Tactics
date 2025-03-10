using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// TODO: Create a dynamic behavior that scans tiles for units to target move for.
public class EnemyUnit : Unit
{
    // Start is called before the first frame update
    void Start()
    {
        unitInfo = GetComponent<UnitInfo>();
        unitMovement = GetComponent<UnitMovement>();
        
        // Remove following lines after EnemyUnits are properly
        // implemented through the json files.
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Units/Test_Enemy/Test_Sprite_Enemy(Down-Left)");
        unitInfo.currentAP = 3;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartTurn();
        }
    }

    public void StartTurn() { StartCoroutine(DecideTurnOption()); }

    // TODO: Make the executions of UnitActions as coroutines so that no future instructions will
    // TODO: execute until they finish. This is especially important for EnemyAI Action Calls.
    public IEnumerator DecideTurnOption()
    {
        while (unitInfo.currentAP > 0)
        {
            --unitInfo.currentAP;
            
            if (InRange(TilemapCreator.UnitLocator[MapCursor.currentUnit], 1, Pattern.Linear))
            {
                Debug.Log("Enemy Unit Attacking...");
                StartCoroutine(new Attack().ExecuteAction(this, MapCursor.currentUnit));
            }
            else
            {
                var tempPath =
                    Pathfinder.FindPath(
                        TilemapCreator.TileLocator[new Vector2Int(unitInfo.CellLocation.x, unitInfo.CellLocation.z)],
                        TilemapCreator.TileLocator[new Vector2Int(MapCursor.currentUnit.x, MapCursor.currentUnit.y)]);
                var chosenTile = DecideTile(tempPath);
                Debug.Log("Moving to tile: " + chosenTile.TileInfo.CellLocation + "(Towards: " + new Vector2Int(-1, 1));
                yield return new Move().ExecuteAction(this, new Vector2Int(chosenTile.TileInfo.CellLocation.x, chosenTile.TileInfo.CellLocation.z));
            }
        }
       
        EndTurn();
    }
    
    private void EndTurn()
    {
        StartCoroutine(new Wait().ExecuteAction(this, new Vector2Int(unitInfo.CellLocation.x, unitInfo.CellLocation.z)));
    }

    private Tile DecideTile(List<Tile> tempPath)
    {
        Tile chosenTile = null;
        List<Tile> shortestPath = tempPath;

        foreach (Tile tile in Rangefinder.GetTilesInRange(
                     TilemapCreator.TileLocator[new Vector2Int(unitInfo.CellLocation.x, unitInfo.CellLocation.z)],
                     unitInfo.finalMove, Pattern.Splash))
        {
            Vector2Int tileCell = new Vector2Int(tile.TileInfo.CellLocation.x, tile.TileInfo.CellLocation.z);
            
            if (!TilemapCreator.UnitLocator.ContainsKey(tileCell))
            {
                List<Tile> foundPath = Pathfinder.FindPath(TilemapCreator.TileLocator[tileCell],
                    TilemapCreator.TileLocator[new Vector2Int(MapCursor.currentUnit.x, MapCursor.currentUnit.y)]);

                if (foundPath.Count < shortestPath.Count)
                {
                    shortestPath = foundPath;
                    chosenTile = tile;
                }
            }
        }

        return chosenTile ?? TilemapCreator.TileLocator[new Vector2Int(shortestPath[^1].TileInfo.CellLocation.x, shortestPath[^1].TileInfo.CellLocation.z)];
    }

    private bool InRange(Unit unit, int range, Pattern pattern)
    {
        var neighborTiles =
            Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[unitInfo.Vector2CellLocation()], range, pattern);

        return neighborTiles.Contains(TilemapCreator.TileLocator[unit.unitInfo.Vector2CellLocation()]);
    }
}
