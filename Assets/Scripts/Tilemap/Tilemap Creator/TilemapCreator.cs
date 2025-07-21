using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TilemapCreator : MonoBehaviour
{
    private const string DEFAULT_DIRECTORY = "Assets/StreamingAssets";
    public string fileName;
    public static Dictionary<Vector2Int, Unit> UnitLocator { get; private set; } // Contains all Units in game scene
    
    [SerializeField] private TurnSystem turnSystem;

    public static void Clear() {
        TileLocator.Clear();
        UnitLocator = null;
    }

    public static void RegisterCleanup() =>
    MemoryManager.AddListeners(Clear);

    private void Awake() {
        Debug.Log($"Tilelocator is null? {TileLocator.SelectableTiles == null}");
        TileLocator.Clear();
        UnitLocator = new Dictionary<Vector2Int, Unit>();
    }


    void Start() { LoadFromJson(); }
    
    private void LoadFromJson() {
        string filePath = Path.Combine(Application.streamingAssetsPath, fileName);

        if (File.Exists(filePath)) {
            TilemapData tilemapData = JsonUtility.FromJson<TilemapData>(File.ReadAllText(filePath));
            LoadTileMap(tilemapData.tiles);
            LoadUnit(tilemapData.units);
        }
        else {
            Debug.LogError("File '" +  filePath + "' does not exist");
        }
    }
     
    private void LoadTileMap(List<TileData> tiles) {
        // Group tiles by their (x, z) position to determine the highest tile
        Dictionary<Vector2Int, TileData> topmostTiles = new Dictionary<Vector2Int, TileData>();
        
        foreach (TileData tile in tiles) {
            Vector2Int key = new Vector2Int(tile.cellLocation.x, tile.cellLocation.z);
            bool isTraversable = tile.isTraversable;


            // Check if this is the highest tile at this (x, z) position
            if (isTraversable && !topmostTiles.ContainsKey(key)) { // First time seeing this (x, z) location, store it
                topmostTiles.Add(key, tile);
            }
            else if (isTraversable && tile.cellLocation.y > topmostTiles[key].cellLocation.y) { // Renders the previous tile (bottom tile) before replacing it with a topmost tile
                TileData bottomTile = topmostTiles[key];
                Tile t = new Tile(bottomTile.cellLocation, bottomTile.tileType, bottomTile.terrainType, bottomTile.tileDirection, bottomTile.isStartingArea, bottomTile.isTraversable);
                
                TileLocator.AddToTilemapTiles(bottomTile.cellLocation, t);
                topmostTiles[key] = tile;
            }
            else {
                // This is a bottom tile / non-traversable tile, render it separately
                Tile t = new Tile(tile.cellLocation, tile.tileType, tile.terrainType, tile.tileDirection, tile.isStartingArea, tile.isTraversable);
                TileLocator.AddToTilemapTiles(tile.cellLocation, t);
            }
        }

        // Now, add only the topmost tiles to TileLocator
        foreach (var entry in topmostTiles) {
            TileData tile = entry.Value;
            Tile newTile = new Tile(tile.cellLocation, tile.tileType, tile.terrainType, tile.tileDirection, 
                tile.isStartingArea, tile.isTraversable, OverlayTilePrefabLibrary.FindPrefab(tile.tileType));
            
            TileLocator.AddToTilemapTiles(tile.cellLocation, newTile);
            TileLocator.AddToSelectableTiles(entry.Key, newTile);
        }
    }

    private void LoadUnit(List<UnitData> units) {
        foreach (UnitData unitData in units) {
            Unit unit = UnitFactory.Create(Resources.Load<GameObject>("Prefabs/Unit/Enemy"), unitData.cellLocation, unitData.unitDirection);
            UnitLocator.Add(new Vector2Int(unit.UnitInfo.CellLocation.x, unit.UnitInfo.CellLocation.z), unit);
        }
        StartCoroutine(turnSystem.TurnLoop());
    }
}
