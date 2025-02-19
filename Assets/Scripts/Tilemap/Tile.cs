using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile
{
    public TileRenderer TileRender { get; set; }
    public TileInfo TileInfo { get; set; }
    public GameObject TileObj { get; private set; }
    public OverlayTile OverlayObj { get; private set; }




    public Tile(Vector3Int cellLocation, TileType tileType, TerrainType terrainType, TileDirection direction, bool isStartArea, bool isTraversable, GameObject OverlayObjPrefab = null)
    {
        // Creating the original Tile object
        TileObj = new GameObject("Tile: " + cellLocation);

        TileInfo = TileObj.AddComponent<TileInfo>();
        TileInfo.Initialize(cellLocation, tileType, terrainType, direction, isStartArea, isTraversable);

        TileRender = TileObj.AddComponent<TileRenderer>();
        TileRender.Render(cellLocation, tileType, terrainType, direction);

        // Creating the Overlay Object
        if(OverlayObjPrefab != null)
            OverlayObj = new OverlayTile(TileObj, OverlayObjPrefab);
    }

}




/*public class Tile
{
    public TileRenderer TileRender { get; set; }
    public TileInfo TileInfo { get; set; }
    public GameObject GameObj { get; private set; }



    public Tile(Vector3Int cellLocation, TileType tileType, TerrainType terrainType, TileDirection direction, bool isStartArea, bool isTraversable)
    {
        
        GameObj = new GameObject("Tile: " + cellLocation);
        
        TileInfo = GameObj.AddComponent<TileInfo>();
        TileInfo.Initialize(cellLocation, tileType, terrainType, direction, isStartArea, isTraversable);

        TileRender = GameObj.AddComponent<TileRenderer>();
        TileRender.Render(cellLocation, tileType, terrainType, direction);
    }

}*/



