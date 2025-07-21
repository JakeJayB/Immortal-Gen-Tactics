using System.Collections.Generic;
using UnityEngine;

public class TileLocator {
    public static Dictionary<Vector2Int, Tile> SelectableTiles { get; private set; } = new();     // All Selectable Tile On Tilemap
    public static Dictionary<Vector3Int, Tile> TilemapTiles { get; private set; } = new();            // All Tilemap Tiles
    
    public static void Clear() {
        SelectableTiles = new();
        TilemapTiles = new();
    }
    public static void AddToSelectableTiles(Vector2Int cellLocation, Tile tile) {
        SelectableTiles.TryAdd(cellLocation, tile); 
    }
    public static void AddToTilemapTiles(Vector3Int cellLocation, Tile tile) {
        TilemapTiles.TryAdd(cellLocation, tile); 
    }
}
