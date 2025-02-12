using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class Terrain
{
    // Important Note: Resources.Load looks for files in the 'Resources' folder only. So, "Assets/Resources/Materials/.." won't work.
    private const string MATERIAL_PATH = "Materials/";
/*
    public static readonly Dictionary<TerrainType, Material[]> TerrainMaterials = new Dictionary<TerrainType, Material[]>()
        {
            { TerrainType.STANDARD , new Material[]
            {
                Resources.Load<Material>("Materials/New Material"),
                Resources.Load<Material>("Materials/New Material"),
                Resources.Load<Material>("Materials/New Material"),
                Resources.Load<Material>("Materials/New Material"),
                Resources.Load<Material>("Materials/New Material"),
                Resources.Load<Material>("Materials/New Material")
            } }
        };*/

    // Gets the path of the material based on the terrain type
    private static string GetPath(TerrainType terrainType)
    {
        // Important Note: Resources.Load doesn't expect file extensions. So, "Materials/...mat" won't work.
        switch (terrainType)
        {
            case TerrainType.STANDARD:
                return MATERIAL_PATH + "Standard";
            case TerrainType.TRANSPARENT:
                return MATERIAL_PATH + "Transparent";
            case TerrainType.GRASS:
                return MATERIAL_PATH + "Grass";
            case TerrainType.STONE:
                return MATERIAL_PATH + "Stone";
            case TerrainType.WATER:
                return MATERIAL_PATH + "Water";
            default:
                Debug.LogError("Terrain: TerrainType not found. Default to Standard");
                return MATERIAL_PATH + "Standard";
        }
    }

    // Returns the material list based on the terrain type
    public static Material[] GetTerrain(TerrainType terrainType, int subMeshCount)
    {
        Material[] materials = new Material[subMeshCount];
        string path = GetPath(terrainType);        

        for (int i = 0; i < subMeshCount; i++) 
            materials[i] = Resources.Load<Material>(path);
        

        return materials;

    }

}
