using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TilemapCreator : MonoBehaviour
{
    private const string DEFAULT_DIRECTORY = "Assets/Resources/JSON";
    public string fileName;
    public static Dictionary<Vector2Int, Tile> TileLocator { get; private set; }

    void Start()
    {
        TileLocator = new Dictionary<Vector2Int, Tile>();
        LoadFromJson();
    }

    private void LoadFromJson()
    {
        string filePath = DEFAULT_DIRECTORY + "/" + fileName; 
        DataList data;

        if(File.Exists(filePath))
        {
            data = JsonUtility.FromJson<DataList>(File.ReadAllText(filePath));
            LoadTileMap(data.tiles);
            LoadUnit(data.units);
        }
        else
        {
            Debug.LogError("File '" +  filePath + "' does not exist");
        }

    }
     
    private void LoadTileMap(List<TileData> tiles)
    {
        foreach(TileData tile in tiles)
        {
            Tile newTile = new Tile(tile.cellLocation, tile.tileType, tile.terrainType, tile.tileDirection, tile.isStartingArea);

            TileInfo tileInfo = newTile.TileInfo;
            if (!TileLocator.ContainsKey(new Vector2Int(tileInfo.CellLocation.x, tileInfo.CellLocation.z)))
            {
                TileLocator.Add(new Vector2Int(tileInfo.CellLocation.x, tileInfo.CellLocation.z), newTile);
            }
        }
    }

    private void LoadUnit(List<UnitData> units)
    {
        foreach (UnitData unit in units)
        {
            Unit.Initialize(unit.cellLocation);
        }
    }

 
}
