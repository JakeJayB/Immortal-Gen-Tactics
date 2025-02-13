using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

[System.Serializable]
public class TileData
{
    public Vector3Int cellLocation;
    public TileType tileType;
    public TerrainType terrainType;
    public TileDirection tileDirection;
    public bool isStartingArea;

    public TileData(Vector3Int location, TileType type, TerrainType terrain, TileDirection direction, bool isStartingArea)
    {
        this.cellLocation = location;
        this.tileType = type;
        this.terrainType = terrain;
        this.tileDirection = direction;
        this.isStartingArea = isStartingArea;
    }
}

[System.Serializable]
public class UnitData
{
    public Vector3Int cellLocation;

    public UnitData(Vector3Int cellLocation)
    {
        this.cellLocation = cellLocation;
    }
}

[System.Serializable]
public class DataList
{
    public List<TileData> tiles = new List<TileData>();
    public List<UnitData> units = new List<UnitData>();
    
}


public class TilemapToJSON : MonoBehaviour
{
    private const string DEFAULT_DIRECTORY = "Assets/Resources/JSON/Levels";
    public string fileName;
    private DataList data;

    // Start is called before the first frame update
    void Start()
    {

        if (String.IsNullOrEmpty(fileName))
        {
            Debug.LogError("TilemapToJSON: fileName is NULL. Insert a file name and retry");
            return;
        }

        data = new DataList();
        ExtractTileData();
        ExtractUnitData();
        SaveToJson();
    }

    private void ExtractTileData()
    {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles)
        {
            TileInfo tileInfo = tile.GetComponent<TileInfo>();

            // gets the tile location and determines the Vector3Int cell location
            Vector3 pos = tile.transform.position;
            int x = Mathf.RoundToInt(pos.x / .5f);
            int y = Mathf.RoundToInt(pos.y / .25f);
            int z = Mathf.RoundToInt(pos.z / .5f);
            Vector3Int cellLocation = new Vector3Int(x, y, z);


            // Get the rotation of the tile and determines the tileDirection enum
            int rotation = Mathf.RoundToInt(tile.transform.eulerAngles.y);
            TileDirection tileDirection = rotation switch
            {
                0 => TileDirection.Forward,
                180 => TileDirection.Backward,
                90 => TileDirection.Left,
                270 => TileDirection.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, "At tile" + new Vector3(pos.x+0.25f , pos.y, pos.z+0.25f)),
            };

            // TileType, TerrainType, and isStartArea are manually inputed
            TileData tileData = new TileData(cellLocation, tileInfo.TileType, tileInfo.TerrainType, tileDirection, tileInfo.IsStartArea);
            data.tiles.Add(tileData);
        }
    }

    private void ExtractUnitData()
    {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        foreach (GameObject unit in units)
        {
            Vector3 pos = unit.transform.position;
            int x = Mathf.RoundToInt(pos.x / .5f);
            int y = Mathf.RoundToInt(pos.y / .25f);
            int z = Mathf.RoundToInt(pos.z / .5f);
            UnitData unitData = new UnitData(new Vector3Int(x, y , z));
            data.units.Add(unitData);
        }
    }

    private void SaveToJson()
    {
        if (!System.IO.Directory.Exists(DEFAULT_DIRECTORY)) { System.IO.Directory.CreateDirectory(DEFAULT_DIRECTORY); }
        string path = $"{DEFAULT_DIRECTORY}/{fileName}.json";

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
        Debug.Log("Level Saved to " + path);

    }


}
