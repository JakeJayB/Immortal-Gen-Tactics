using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TilemapCreator : MonoBehaviour
{
    private const string DEFAULT_DIRECTORY = "Assets/StreamingAssets";
    public string fileName;
    public static Dictionary<Vector2Int, Tile> TileLocator { get; private set; } // Contains all traversable tiles in game scene
    public static Dictionary<Vector3Int, Tile> AllTiles { get; private set; } // Contains all tiles in game scene
    public static Dictionary<TileType, GameObject> OverlayPrefabs; // Contains Overlay prefabs for every TileType
    public static Dictionary<Vector2Int, Unit> UnitLocator { get; private set; } // Contains all Units in game scene
    
    [SerializeField] private TurnSystem turnSystem;

    [SerializeField] private GameObject OverlayFlatPrefab;
    [SerializeField] private GameObject OverlaySlantedPrefab;
    [SerializeField] private GameObject OverlaySlantedCornerPrefab;
    [SerializeField] private GameObject OverlayStairPrefab;

    public static void Clear()
    {
        TileLocator = null;
        UnitLocator = null;
        AllTiles = null;
        OverlayPrefabs = null;
    }

    public static void RegisterCleanup() =>
    MemoryManager.AddListeners(Clear);

    private void Awake()
    {
        Debug.Log($"Tilelocator is null? {TileLocator == null}");
        TileLocator = new Dictionary<Vector2Int, Tile>();
        UnitLocator = new Dictionary<Vector2Int, Unit>();
        AllTiles = new Dictionary<Vector3Int, Tile>();

        OverlayPrefabs = new Dictionary<TileType, GameObject>()
        {
            {TileType.Flat, OverlayFlatPrefab},
            {TileType.Slanted, OverlaySlantedPrefab},
            {TileType.Slanted_Corner, OverlaySlantedCornerPrefab},
            {TileType.Stairs, OverlayStairPrefab},
        };
    }


    void Start()
    {
        LoadFromJson();
    }

    public static void Initialize()
    {
        
    }
    
    private void LoadFromJson()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);
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
                Tile t = new Tile(bottomTile.cellLocation, bottomTile.tileType, bottomTile.terrainType, bottomTile.tileDirection, bottomTile.isStartingArea, bottomTile.isTraversable);
                
                AllTiles.Add(bottomTile.cellLocation, t);
                topmostTiles[key] = tile;
            }
            else
            {
                // This is a bottom tile / non-traversable tile, render it separately
                Tile t = new Tile(tile.cellLocation, tile.tileType, tile.terrainType, tile.tileDirection, tile.isStartingArea, tile.isTraversable);
                AllTiles.Add(tile.cellLocation, t);
            }
        }

        // Now, add only the topmost tiles to TileLocator
        foreach (var entry in topmostTiles)
        {
            TileData tile = entry.Value;
            Tile newTile = new Tile(tile.cellLocation, tile.tileType, tile.terrainType, tile.tileDirection, tile.isStartingArea, tile.isTraversable, OverlayPrefabs[tile.tileType]);
            AllTiles.Add(tile.cellLocation, newTile);
            TileLocator.Add(entry.Key, newTile);
        }
    }

    private void LoadUnit(List<UnitData> units)
    {
        foreach (UnitData unitData in units)
        {
            Unit unit = UnitFactory.Create(Resources.Load<GameObject>("Prefabs/Unit/Enemy"), unitData.cellLocation, unitData.unitDirection);
            UnitLocator.Add(new Vector2Int(unit.UnitInfo.CellLocation.x, unit.UnitInfo.CellLocation.z), unit);
        }

        StartCoroutine(turnSystem.TurnLoop());
    }
}
