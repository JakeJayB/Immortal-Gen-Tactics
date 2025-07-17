using System;
using UnityEngine;

public class TileTransform : MonoBehaviour {
    public static void PositionTile(GameObject tile, Vector3Int cellLocation) {
        tile.transform.position = new Vector3(
            cellLocation.x * TileProperties.TILE_WIDTH,
            cellLocation.y * TileProperties.TILE_HEIGHT,
            cellLocation.z * TileProperties.TILE_LENGTH);
    }

    public static void RotateTile(GameObject tile, TileDirection direction) {
        var rotation = direction switch {
            TileDirection.Forward => 0f,
            TileDirection.Backward => 180f,
            TileDirection.Left => 90f,
            TileDirection.Right => 270f,
            _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
        };
        tile.transform.rotation = Quaternion.Euler(0, rotation, 0);
    }
    
    public static Vector3[] CenterPivots(Vector3[] vertices) {
        Vector3 pivotOffset = new Vector3(TileProperties.TILE_WIDTH / 2, TileProperties.TILE_HEIGHT / 2, TileProperties.TILE_LENGTH / 2);
        for (int i = 0; i < vertices.Length; i++) {
            vertices[i] -= pivotOffset;
        }
        return vertices;
    }
}
