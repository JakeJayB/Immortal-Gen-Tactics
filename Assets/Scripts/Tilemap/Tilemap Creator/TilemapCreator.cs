using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TilemapCreator : MonoBehaviour
{
    private const string DEFAULT_DIRECTORY = "Assets/Resources/JSON/Levels";
    public string fileName;
    public static Dictionary<Vector2Int, Tile> TileLocator { get; private set; }
    public static Dictionary<Vector2Int, Unit> UnitLocator { get; private set; }

    [SerializeField] private TurnSystem turnSystem;

    [SerializeField] private GameObject OverlayFlatPrefab;
    [SerializeField] private GameObject OverlaySlantedPrefab;
    [SerializeField] private GameObject OverlaySlantedCornerPrefab;
    [SerializeField] private GameObject OverlayStairPrefab;

    private Dictionary<TileType, GameObject> OverlayPrefabs;

    void Start()
    {
        TileLocator = new Dictionary<Vector2Int, Tile>();
        UnitLocator = new Dictionary<Vector2Int, Unit>();

        OverlayPrefabs = new Dictionary<TileType, GameObject>()
        {
            {TileType.Flat, OverlayFlatPrefab},
            {TileType.Slanted, OverlaySlantedPrefab},
            {TileType.Slanted_Corner, OverlaySlantedCornerPrefab},
            {TileType.Stairs, OverlayStairPrefab},
        };

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
            bool isTraversable = tile.isTraversable;


            // Check if this is the highest tile at this (x, z) position
            if (isTraversable && !topmostTiles.ContainsKey(key)) // First time seeing this (x, z) location, store it
            {
                topmostTiles.Add(key, tile);
            }
            else if(isTraversable && tile.cellLocation.y > topmostTiles[key].cellLocation.y) // Renders the previous tile (bottom tile) before replacing it with a topmost tile
            {
                TileData bottomTile = topmostTiles[key];
                new Tile(bottomTile.cellLocation, bottomTile.tileType, bottomTile.terrainType, bottomTile.tileDirection, bottomTile.isStartingArea, bottomTile.isTraversable);

                topmostTiles[key] = tile;
            }
            else
            {
                // This is a bottom tile / non-traversable tile, render it separately
                new Tile(tile.cellLocation, tile.tileType, tile.terrainType, tile.tileDirection, tile.isStartingArea, tile.isTraversable);

            }
        }

        // Now, add only the topmost tiles to TileLocator
        foreach (var entry in topmostTiles)
        {
            TileData tile = entry.Value;
            Tile newTile = new Tile(tile.cellLocation, tile.tileType, tile.terrainType, tile.tileDirection, tile.isStartingArea, tile.isTraversable, OverlayPrefabs[tile.tileType]);
            TileLocator.Add(entry.Key, newTile);
        }
    }

    private void LoadUnit(List<UnitData> units)
    {
        List<Unit> unitsList = new List<Unit>();
        foreach (UnitData unitData in units)
        {
            Unit unit = Unit.Initialize(unitData.cellLocation);
            UnitLocator.Add(new Vector2Int(unit.unitInfo.CellLocation.x, unit.unitInfo.CellLocation.z), unit);
            unitsList.Add(unit);
        }

        turnSystem.InitializeUnits(unitsList);
    }
    
    public static List<Tile> GetNeighborTiles(Tile currentTile)
    {
                
        List<Tile> neighbors = new List<Tile>();

        // Up
        Vector2Int locationToCheck = new Vector2Int(
            currentTile.TileInfo.CellLocation.x + 1,
            currentTile.TileInfo.CellLocation.z);

        if (TileLocator.TryGetValue(locationToCheck, out var upTile))
        {
            neighbors.Add(upTile);
        }

        // Down
        locationToCheck = new Vector2Int(
            currentTile.TileInfo.CellLocation.x - 1,
            currentTile.TileInfo.CellLocation.z);

        if (TileLocator.TryGetValue(locationToCheck, out var downTile))
        {
            neighbors.Add(downTile);
        }

        // Left
        locationToCheck = new Vector2Int(
            currentTile.TileInfo.CellLocation.x,
            currentTile.TileInfo.CellLocation.z + 1);

        if (TileLocator.TryGetValue(locationToCheck, out var leftTile))
        {
            neighbors.Add(leftTile);
        }

        // Right
        locationToCheck = new Vector2Int(
            currentTile.TileInfo.CellLocation.x,
            currentTile.TileInfo.CellLocation.z - 1);

        if (TileLocator.TryGetValue(locationToCheck, value: out var rightTile))
        {
            neighbors.Add(rightTile);
        }

        // Return the found neighbors.
        return neighbors;
    }

 
}
