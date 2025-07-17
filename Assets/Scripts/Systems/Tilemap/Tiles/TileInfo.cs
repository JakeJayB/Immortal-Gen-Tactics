using System;
using UnityEngine;

public class TileInfo {
    public Vector3Int CellLocation;
    public TileType TileType;
    public TerrainType TerrainType;
    public TileDirection TileDirection;
    public Boolean IsStartArea;
    public Boolean IsTraversable;

    public TileInfo(Vector3Int cellLocation, TileType tileType, TerrainType terrainType, TileDirection direction, bool isStartArea, bool isTraversable) {
        CellLocation = cellLocation;
        TileType = tileType;
        TerrainType = terrainType;
        TileDirection = direction;
        IsStartArea = isStartArea;
        IsTraversable = isTraversable;
    }

    public Vector2Int Vector2CellLocation() { return new Vector2Int(CellLocation.x, CellLocation.z); }
}