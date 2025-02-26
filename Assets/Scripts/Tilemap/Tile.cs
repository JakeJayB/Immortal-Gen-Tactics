using Environment.Instancing;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile
{
    private static string PREFAB_PATH = "Prefabs/Tilemap/Tiles/";
    public TileRenderer TileRender { get; set; }
    public TileInfo TileInfo { get; set; }
    public GameObject TileObj { get; private set; }
    public OverlayTile OverlayObj { get; set; }
    public string TilePrefabPath { get; set; }



    public Tile(Vector3Int cellLocation, TileType tileType, TerrainType terrainType, TileDirection direction, bool isStartArea, bool isTraversable, GameObject OverlayObjPrefab = null)
    {
        // Creating the original Tile object
        TileObj = new GameObject("Tile: " + cellLocation);

        TileInfo = TileObj.AddComponent<TileInfo>();
        TileInfo.Initialize(cellLocation, tileType, terrainType, direction, isStartArea, isTraversable);

        SetPrefabPath();

        TileRender = TileObj.AddComponent<TileRenderer>();
        TileRender.Render(cellLocation, tileType, terrainType, direction, TilePrefabPath);


        // Creating the Overlay Object
        if(OverlayObjPrefab != null)
            OverlayObj = new OverlayTile(TileObj, OverlayObjPrefab);

    }

    private void SetPrefabPath()
    {
        string path;
        switch (TileInfo.TerrainType)
        {
            case TerrainType.STANDARD:
                path = PREFAB_PATH + "Standard/";
                break;
            case TerrainType.GRASS:
                path = PREFAB_PATH + "Grass/";
                break;
            case TerrainType.STONE:
                path = PREFAB_PATH + "Stone/";
                break;
            case TerrainType.WATER:
                path = PREFAB_PATH + "Water/";
                break;
            case TerrainType.OVERLAY:
                path = PREFAB_PATH + "Overlay/";
                break;
            default:
                Debug.LogError("Terrain: TerrainType not found. Default to Standard");
                path = PREFAB_PATH + "Standard/";
                break;
        }

        switch (TileInfo.TileType)
        { 
            case TileType.Flat:
                path += "Flat";
                break;
            case TileType.Slanted:
                path += "Slanted";
                break;
            case TileType.Slanted_Corner:
                path += "Slanted_Corner";
                break;
            case TileType.Stairs:
                path += "Stairs";
                break;
            default:
                Debug.LogError("Terrain: TileType not found. Default to Flat");
                path += "Flat";
                break;
        }

        TilePrefabPath = path;
    }


    public static void DestroyTiles(List<Tile> tiles)
    {

        void UpdateTileLocator(Tile tile, Vector2Int cellLocation2D)
        {
            // set traversable to true
            tile.TileInfo.isTraversable = true;

            // add tile to Tilemapcreator.TileLocator
            TilemapCreator.TileLocator.Add(cellLocation2D, tile);

            // Create Overlay tile if not yet created
            if(tile.OverlayObj == null)
                tile.OverlayObj = new OverlayTile(tile.TileObj, TilemapCreator.OverlayPrefabs[tile.TileInfo.TileType]);
            

        }


        foreach(Tile tile in tiles)
        {
            Vector2Int cellLocation2D = new Vector2Int(tile.TileInfo.CellLocation.x, tile.TileInfo.CellLocation.z);
            Vector3Int cellLocation3D = tile.TileInfo.CellLocation;

            //skip tiles that have a unit on it
            if (TilemapCreator.UnitLocator.ContainsKey(cellLocation2D))
                continue;

            //Remove tile instance from Tilecreator.TileLocator and Tilecreator.AllTiles
            if (TilemapCreator.TileLocator.ContainsKey(cellLocation2D))
                TilemapCreator.TileLocator.Remove(cellLocation2D);

            if(TilemapCreator.AllTiles.ContainsKey(cellLocation3D))
                TilemapCreator.AllTiles.Remove(cellLocation3D);

            //Add tile under destroyed tile to Tilecreator.Tilelocators
            if(TilemapCreator.AllTiles.ContainsKey(cellLocation3D + Vector3Int.down))
                UpdateTileLocator(TilemapCreator.AllTiles[cellLocation3D + Vector3Int.down], cellLocation2D);

            //Destroy tile gameObject from scene
            MonoBehaviour.Destroy(tile.TileObj);

            //Destroy overlay tile gameobject from scene
            if (tile.OverlayObj != null)
                MonoBehaviour.Destroy(tile.OverlayObj.OverlayObj);
        }

        
    }
    
    public bool IsSelectable() { return OverlayObj.IsSelectable; }
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



