using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TilemapCreator : MonoBehaviour
{
    private const string DEFAULT_DIRECTORY = "Assets/Resources/JSON";
    public string fileName;
    public static Dictionary<Vector2Int, Tile> TileLocator { get; private set; }

    [SerializeField]
    private TurnSystem turnSystem; 

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
        // Group tiles by their (x, z) position to determine the highest tile
        Dictionary<Vector2Int, TileData> topmostTiles = new Dictionary<Vector2Int, TileData>();
        
        foreach (TileData tile in tiles)
        {
            Vector2Int key = new Vector2Int(tile.cellLocation.x, tile.cellLocation.z);

            // Check if this is the highest tile at this (x, z) position
            if (!topmostTiles.ContainsKey(key))
            {
                // First time seeing this (x, z) location, store it
                topmostTiles.Add(key, tile);
            }
            else if(tile.cellLocation.y > topmostTiles[key].cellLocation.y)
            {
                // Renders the previous tile (bottom tile) before replacing it with a topmost tile
                TileData bottomTile = topmostTiles[key];
                new Tile(bottomTile.cellLocation, bottomTile.tileType, bottomTile.terrainType, bottomTile.tileDirection, bottomTile.isStartingArea);
                topmostTiles[key] = tile;
            }
            else
            {
                // This is a bottom tile, store it separately
                new Tile(tile.cellLocation, tile.tileType, tile.terrainType, tile.tileDirection, tile.isStartingArea);
            }
        }

        // Now, add only the topmost tiles to TileLocator
        foreach (var entry in topmostTiles)
        {
            TileData tile = entry.Value;
            Tile newTile = new Tile(tile.cellLocation, tile.tileType, tile.terrainType, tile.tileDirection, tile.isStartingArea);
            TileLocator.Add(entry.Key, newTile);
        }
    }

    private void LoadUnit(List<UnitData> units)
    {
        List<Unit> unitsList = new List<Unit>();
        foreach (UnitData unitData in units)
        {
            Unit unit = Unit.Initialize(unitData.cellLocation);
            unitsList.Add(unit);
        }

        //turnSystem.InitializeUnits(unitsList);
    }

 
}
