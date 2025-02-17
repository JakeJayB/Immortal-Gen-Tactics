using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TilePrefabCreator : MonoBehaviour
{
    private const string DEFAULT_DIRECTORY = "Assets/Resources/Prefabs/Tiles";
    private const string PREFAB_EXTENSION = ".mesh";

    // Start is called before the first frame update
    void Start()
    {
        Tile flatTile = new Tile(new Vector3Int(0, 0, 0), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward, false, true);
        SaveAsPrefab(flatTile.GameObj);
    }

    void SaveAsPrefab(GameObject obj)
    {
        Mesh mesh = obj.GetComponent<MeshFilter>().mesh;
        string meshName = obj.GetComponent<TileInfo>().TileType.ToString();

        var path = $"{DEFAULT_DIRECTORY}/{meshName}{PREFAB_EXTENSION}";

        if (!System.IO.Directory.Exists(DEFAULT_DIRECTORY)) { System.IO.Directory.CreateDirectory(DEFAULT_DIRECTORY); }
        AssetDatabase.CreateAsset(mesh, path);

        Destroy(obj);
    }
}