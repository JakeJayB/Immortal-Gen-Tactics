using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile
{
    public TileRenderer TileRender { get; set; }
    public TileInfo TileInfo { get; set; }
    public GameObject GameObj { get; private set; }



    public Tile(Vector3Int cellLocation, TileType tileType, TerrainType terrainType, TileDirection direction, bool isStartArea)
    {
        
        GameObj = new GameObject("Tile: " + cellLocation);

        TileInfo = GameObj.AddComponent<TileInfo>();
        TileInfo.Initialize(cellLocation, tileType, terrainType, direction, isStartArea);

        TileRender = GameObj.AddComponent<TileRenderer>();
        TileRender.Render(cellLocation, tileType, terrainType, direction);
    }

}
