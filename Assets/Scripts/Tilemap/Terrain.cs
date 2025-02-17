using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Terrain
{
    // Important Note: Resources.Load looks for files in the 'Resources' folder only. So, "Assets/Resources/Materials/.." won't work.
    //private const string MATERIAL_PATH = "Materials/";
    private const string MATERIAL_PATH = "Prefabs/Tilemap/Tiles/";

    // Gets the path of the material based on the terrain type
    private static string GetPath(TileType tileType, TerrainType terrainType)
    {
        string path;
        switch (terrainType)
        {
            case TerrainType.STANDARD:
                path = MATERIAL_PATH + "Standard/";
                break;
            case TerrainType.GRASS:
                path = MATERIAL_PATH + "Grass/";
                break;
            case TerrainType.STONE:
                path = MATERIAL_PATH + "Stone/";
                break;
            case TerrainType.WATER:
                path = MATERIAL_PATH + "Water/";
                break;
            case TerrainType.MOVE:
                path = MATERIAL_PATH + "Move/";
                break;
            case TerrainType.ATTACK:
                path = MATERIAL_PATH + "Attack/";
                break;
            default:
                Debug.LogError("Terrain: TerrainType not found. Default to Standard");
                path = MATERIAL_PATH + "Standard/";
                break;
        }

        switch(tileType)
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
        return path;
    }

    // Returns the material list based on the terrain type
    public static Material[] GetTerrain(TileType tileType, TerrainType terrainType, int subMeshCount)
    {
        string path = GetPath(tileType, terrainType);

        Material[] materials = Resources.Load<GameObject>(path).GetComponent<MeshRenderer>().sharedMaterials;
        return materials;

    }
}
