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

    public void StartTurn() { DecideTurnOption(); }

    public void DecideTurnOption()
    {
        if (unitInfo.currentAP > 0)
        {
            --unitInfo.currentAP;
            
            var tempPath =
                Pathfinder.FindPath(
                    TilemapCreator.TileLocator[new Vector2Int(unitInfo.CellLocation.x, unitInfo.CellLocation.z)],
                    TilemapCreator.TileLocator[new Vector2Int(MapCursor.currentUnit.x, MapCursor.currentUnit.y)]);
            var chosenTile = DecideTile(tempPath);
            Debug.Log("Moving to tile: " + chosenTile.TileInfo.CellLocation + "(Towards: " + new Vector2Int(-1, 1));
            new Move().ExecuteAction(this, new Vector2Int(chosenTile.TileInfo.CellLocation.x, chosenTile.TileInfo.CellLocation.z));
        }
        else
        {
            EndTurn();
        }
    }
    
    private void EndTurn()
    {
        new Wait().ExecuteAction(this, new Vector2Int(unitInfo.CellLocation.x, unitInfo.CellLocation.z));
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
}
