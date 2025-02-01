using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TilemapData
{
    public List<TileData> tiles;

    public struct Bounds
    {
        public int Min { get; private set; }
        public int Max { get; private set; }

        public Bounds(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
    public Bounds xBounds;
    public Bounds yBounds;
    public Bounds zBounds;
}

[Serializable]
public class TileData
{
    public readonly TileType tileType;              // TODO: Swap TileType and TerrainType in TileData w/ a Prefab Variable
    public readonly TerrainType terrainType;        // TODO: Do this once Prefabs have been made for tiles.
    public readonly TileDirection direction;
    private int x;
    private int y;
    private int z;

    public Vector3Int CellLocation()
    {
        return new Vector3Int(x, y, z);
    }
}