using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMovement : MonoBehaviour {
    private const float TRAVEL_SPEED = 3f;
    private const float FLAT_Y_OFFSET = 0.3f;
    private const float SLOPE_Y_OFFSET = 0.15f;
    private const float POS_SNAP_RANGE = 0.05f;
    
    public static IEnumerator Move(Unit unit, Vector2Int destination) {
        var tilemap = TileLocator.SelectableTiles;
        var unitCell = unit.UnitInfo.CellLocation;
        List<Tile> path = Pathfinder.FindPath(tilemap[new Vector2Int(unitCell.x, unitCell.z)], tilemap[destination]);
        yield return MoveUnitToTile(unit, path);
    }

    private static IEnumerator MoveUnitToTile(Unit unit, List<Tile> path) {
        while (path.Count > 0) {
            Tile nextTile = path[0];
            Vector3 targetPosition = new Vector3(
                nextTile.TileObj.transform.position.x, 
                nextTile.TileObj.transform.position.y + (nextTile.TileInfo.TileType == TileType.Flat ? FLAT_Y_OFFSET : SLOPE_Y_OFFSET),
                nextTile.TileObj.transform.position.z);
            
            while (Vector3.Distance(unit.GameObj.transform.position, targetPosition) > POS_SNAP_RANGE) {
                float step = TRAVEL_SPEED * Time.deltaTime;
                unit.GameObj.transform.position = Vector3.MoveTowards(unit.GameObj.transform.position, targetPosition, step);
                yield return null;
            }
            
            unit.GameObj.transform.position = targetPosition;
            unit.UnitInfo.CellLocation = nextTile.TileInfo.CellLocation;
            path.RemoveAt(0);
        }
    }
}
