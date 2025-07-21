using UnityEngine;
using System.IO;
using System;

public class TilemapToJSON : MonoBehaviour {
    private const string DEFAULT_DIRECTORY = "Assets/Resources/JSON/Levels";
    public string fileName;
    private TilemapData _tilemapData;

    // Start is called before the first frame update
    void Start() {
        if (string.IsNullOrEmpty(fileName)) {
            Debug.LogError("TilemapToJSON: fileName is NULL. Insert a file name and retry");
            return;
        }

        _tilemapData = new TilemapData();
        ExtractTileData();
        ExtractUnitData();
        SaveToJson();
    }

    private void ExtractTileData() {
        GameObject[] tiles = GameObject.FindGameObjectsWithTag("Tile");

        foreach (GameObject tile in tiles) {
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

            if(tileInfo.TerrainType == TerrainType.WATER)
                tileInfo.IsTraversable = false;

            // TileType, TerrainType, and isStartArea are manually inputed
            TileData tileData = new TileData(cellLocation, tileInfo.TileType, tileInfo.TerrainType, tileDirection, tileInfo.IsStartArea, tileInfo.IsTraversable);
            _tilemapData.tiles.Add(tileData);
        }
    }

    private void ExtractUnitData() {
        GameObject[] units = GameObject.FindGameObjectsWithTag("Unit");
        
        foreach (GameObject unit in units) {
            Vector3 pos = unit.transform.position;
            int x = Mathf.RoundToInt(pos.x / .5f);
            int y = Mathf.RoundToInt(pos.y / .25f);
            int z = Mathf.RoundToInt(pos.z / .5f);

            int rotation = Mathf.RoundToInt(unit.transform.eulerAngles.y);
            UnitDirection unitDirection = rotation switch {
                0 => UnitDirection.Forward,
                180 => UnitDirection.Backward,
                90 => UnitDirection.Left,
                270 => UnitDirection.Right,
                _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, "At unit" + new Vector3(pos.x + 0.25f, pos.y, pos.z + 0.25f)),
            };

            UnitData unitData = new UnitData(new Vector3Int(x, y , z), unitDirection);
            _tilemapData.units.Add(unitData);
        }
    }

    private void SaveToJson() {
        if (!Directory.Exists(DEFAULT_DIRECTORY)) { Directory.CreateDirectory(DEFAULT_DIRECTORY); }
        string path = $"{DEFAULT_DIRECTORY}/{fileName}.json";

        string json = JsonUtility.ToJson(_tilemapData, true);
        File.WriteAllText(path, json);
        Debug.Log("Level Saved to " + path);
    }
}
