using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public Vector3Int CellLocation;
    public TileType TileType;
    public TerrainType TerrainType;
    public TileDirection TileDirection;

    public void Initialize(Vector3Int cellLocation, TileType tileType, TerrainType terrainType, TileDirection direction)
    {
        CellLocation = cellLocation;
        TileType = tileType;
        TerrainType = terrainType;
        TileDirection = direction;
    }
}