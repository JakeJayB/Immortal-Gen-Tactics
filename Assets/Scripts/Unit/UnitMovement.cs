using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    // Controls how fast the unit will travel during movement
    private const float TRAVEL_SPEED = 3f;
    
    public static IEnumerator Move(Unit unit, Vector2Int destination)
    {
        var tilemap = TileLocator.SelectableTiles;
        var unitCell = unit.UnitInfo.CellLocation;
        
        List<Tile> path = Pathfinder.FindPath(tilemap[new Vector2Int(unitCell.x, unitCell.z)], tilemap[destination]);

        yield return MoveUnitToTile(unit, path);
    }

    private static IEnumerator MoveUnitToTile(Unit unit, List<Tile> path)
    {
        // Continue to move the unit until no more tiles are left in the path to advance towards
        while (path.Count > 0)
        {
            Tile nextTile = path[0];
            Vector3 targetPosition = new Vector3(
                nextTile.TileObj.transform.position.x, 
                nextTile.TileObj.transform.position.y + (nextTile.TileInfo.TileType == TileType.Flat ? 0.3f : 0.15f),  // Y changes if next tile is flat or sloped
                nextTile.TileObj.transform.position.z);

            // Move unit toward the tile and ensure it stops exactly before continuing
            // Once the unit gets close enough to the destination, end the loop to snap it.
            while (Vector3.Distance(unit.GameObj.transform.position, targetPosition) > 0.05f)
            {
                float step = TRAVEL_SPEED * Time.deltaTime;
                unit.GameObj.transform.position = Vector3.MoveTowards(unit.GameObj.transform.position, targetPosition, step);
                
                yield return null; // Wait for next frame
            }

            // Snap to exact tile position
            unit.GameObj.transform.position = targetPosition;
            unit.UnitInfo.CellLocation = nextTile.TileInfo.CellLocation;

            // Remove the tile just move on from the path list
            path.RemoveAt(0);
        }
    }
}
