using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class TileData
{
    public Vector3Int cellLocation;
    public TileType tileType;
    public TerrainType terrainType;
    public TileDirection tileDirection;

    public TileData(Vector3Int location, TileType type, TerrainType terrain, TileDirection direction)
    {
        this.cellLocation = location;
        this.tileType = type;
        this.terrainType = terrain;
        this.tileDirection = direction;
    }
}

[System.Serializable]
public class TileDataList
{
    public List<TileData> tiles = new List<TileData>();
}


public class TilemapToJSON : MonoBehaviour
{
    private const string DEFAULT_DIRECTORY = "Assets/Resources/JSON";
    private const string FILE_NAME = "Tilemap1";
    private TileDataList data;

    // Start is called before the first frame update
    void Start()
    {
        ExtractData();
        SaveToJson();
    }

    private void ExtractData()
    {
        data = new TileDataList();
        GameObject[] NormTiles = GameObject.FindGameObjectsWithTag("Normal Tile");

        foreach (GameObject tile in NormTiles)
        {
            TileInfo tileInfo = tile.GetComponent<TileInfo>();

            Vector3 pos = tile.transform.position;
            int x = Mathf.RoundToInt(pos.x / .5f);
            int y = Mathf.RoundToInt(pos.y / .25f);
            int z = Mathf.RoundToInt(pos.z / .5f);
            Vector3Int cellLocation = new Vector3Int(x, y, z);

            TileData tileData = new TileData(cellLocation, tileInfo.TileType, tileInfo.TerrainType, tileInfo.TileDirection);
            data.tiles.Add(tileData);
        }
    }

    private void SaveToJson()
    {
        if (!System.IO.Directory.Exists(DEFAULT_DIRECTORY)) { System.IO.Directory.CreateDirectory(DEFAULT_DIRECTORY); }
        string path = $"{DEFAULT_DIRECTORY}/{FILE_NAME}.json";

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);

    }


}
