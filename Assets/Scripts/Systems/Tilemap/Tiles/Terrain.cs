using UnityEngine;

public static class Terrain { 
    public static Material[] GetTerrain(string path) {
        Material[] materials = Resources.Load<GameObject>(path).GetComponent<MeshRenderer>().sharedMaterials;
        return materials;
    }
}
