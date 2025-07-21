using System;
using UnityEngine;

[Serializable]
public class TileData {
    public Vector3Int cellLocation;
    public TileType tileType;
    public TerrainType terrainType;
    public TileDirection tileDirection;
    public bool isStartingArea;
    public bool isTraversable;

    public TileData(Vector3Int location, TileType type, TerrainType terrain, TileDirection direction, bool isStartingArea, bool isTraversable) {
        cellLocation = location;
        tileType = type;
        terrainType = terrain;
        tileDirection = direction;
        this.isStartingArea = isStartingArea;
        this.isTraversable = isTraversable;
    }
}
