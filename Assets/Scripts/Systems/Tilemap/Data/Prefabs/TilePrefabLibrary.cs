using UnityEngine;

public class TilePrefabLibrary {
    private const string PREFAB_PATH = "Prefabs/Tilemap/Tiles/";
    
    public static string FindPrefabPath(Tile tile) {
        string path;
        switch (tile.TileInfo.TerrainType) {
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

        switch (tile.TileInfo.TileType) { 
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
        
        return path;
    }
}
