using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;

public static class Terrain
{
    public static readonly Dictionary<TerrainType, Material[]> TerrainMaterials = new Dictionary<TerrainType, Material[]>()
    {
        { TerrainType.Test , new Material[]
        {
            Resources.Load<Material>("Materials/New Material"),
            Resources.Load<Material>("Materials/New Material"),
            Resources.Load<Material>("Materials/New Material"),
            Resources.Load<Material>("Materials/New Material"),
            Resources.Load<Material>("Materials/New Material"),
            Resources.Load<Material>("Materials/New Material")
        } }
    };

    public static Material[] TestTerrain(int subMeshCount)
    {
        Material[] materials = new Material[subMeshCount];
        for (int i = 0; i < subMeshCount; i++) {
            materials[i] = Resources.Load<Material>("Materials/New Material");
        }

        return materials;
    }
}
