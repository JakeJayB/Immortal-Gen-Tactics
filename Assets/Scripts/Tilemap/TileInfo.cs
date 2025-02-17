using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileInfo : MonoBehaviour
{
    public Vector3Int CellLocation;
    public TileType TileType;
    public TerrainType TerrainType;
    public TileDirection TileDirection;
    public Boolean IsStartArea;
    public Boolean isTraversable;

    public void Initialize(Vector3Int cellLocation, TileType tileType, TerrainType terrainType, TileDirection direction, bool isStartArea, bool isTraversable)
    {
        CellLocation = cellLocation;
        TileType = tileType;
        TerrainType = terrainType;
        TileDirection = direction;
        this.IsStartArea = isStartArea;
        this.isTraversable = isTraversable;
    }

    
}