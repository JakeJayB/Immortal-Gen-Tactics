using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePrefabCreator : MonoBehaviour
{
    private const string DEFAULT_DIRECTORY = "Assets/Resources/Prefabs/Tiles";
    private const string DEFAULT_NAME = "Default";
    private const string PREFAB_EXTENSION = ".prefab";
    
    // Start is called before the first frame update
    void Start()
    {
        Tile flatTestTile = new Tile(new Vector3Int(0, 0, 0), TileType.Flat, TerrainType.Test, TileDirection.Forward);
        SaveAsPrefab(flatTestTile.GameObj, "Flat(Test)");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    void SaveAsPrefab(GameObject obj, string objName)
    {
        objName = string.IsNullOrEmpty(objName) ? DEFAULT_NAME : objName;
        var path = $"{DEFAULT_DIRECTORY}/{objName}{PREFAB_EXTENSION}";
        
        if (!System.IO.Directory.Exists(DEFAULT_DIRECTORY)) { System.IO.Directory.CreateDirectory(DEFAULT_DIRECTORY); }
        UnityEditor.PrefabUtility.SaveAsPrefabAsset(obj, path, out bool success);
        if (success) {
            Debug.Log($"Prefab saved successfully at {path}");
        } else {
            Debug.LogError("Failed to save prefab.");
        }
        
        Destroy(obj);
    }
}
