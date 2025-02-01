using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile
{
    public Vector3Int CellLocation { get; private set; }
    private TileType TileType { get; set; }
    private TerrainType TerrainType { get; set; }
    private TileRenderer TileRender { get; set; }

    public GameObject GameObj { get; private set; }
    
    public Tile(Vector3Int cellLocation, TileType tileType, TerrainType terrainType, TileDirection direction)
    {
        CellLocation = cellLocation;
        TileType = tileType;
        TerrainType = terrainType;

        GameObj = new GameObject("Tile: " + CellLocation);
        TileRender = GameObj.AddComponent<TileRenderer>();
        TileRender.Render(CellLocation, TileType, TerrainType, direction);
    }
    
}
