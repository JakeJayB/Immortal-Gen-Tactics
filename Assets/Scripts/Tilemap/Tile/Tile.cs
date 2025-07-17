using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tile {
    public GameObject TileObj { get; private set; }
    public TileInfo TileInfo { get; set; }
    public OverlayTile OverlayObj { get; set; }
    public string TilePrefabPath { get; set; }
    
    public Tile(Vector3Int cellLocation, TileType tileType, TerrainType terrainType, TileDirection direction, bool isStartArea, bool isTraversable, GameObject OverlayObjPrefab = null) {
        // Creating the original Tile object
        TileObj = new GameObject("Tile: " + cellLocation);
        TileInfo = new TileInfo(cellLocation, tileType, terrainType, direction, isStartArea, isTraversable);
        
        TileRenderer.Render(this, TilePrefabLibrary.FindPrefabPath(this));
        
        // Creating the Overlay Object
        OverlayObj = new OverlayTile(this, OverlayObjPrefab);
    }

    public static void AddTiles(List<Vector3Int> cellLocation) {
        TerrainType DetermineTerrainType(Vector3Int cellLocation) {
            Dictionary<TerrainType, int> terrainCount = new Dictionary<TerrainType, int>();
            Vector3Int[] directions = new Vector3Int[]{ Vector3Int.left, Vector3Int.right, Vector3Int.forward, Vector3Int.back, Vector3Int.down, };

            foreach (var direction in directions) {
                Vector3Int neighborLocation = cellLocation + direction;
                if (TilemapCreator.AllTiles.ContainsKey(neighborLocation)) {
                    TerrainType terrain = TilemapCreator.AllTiles[neighborLocation].TileInfo.TerrainType;
                    if (terrainCount.ContainsKey(terrain))
                        terrainCount[terrain]++;
                    else
                        terrainCount[terrain] = 1;
                }
            }
            // Return the most common neighboring terrain type (default to grass if terrainCount == 0)
            return terrainCount.Count > 0 ? terrainCount.OrderByDescending(t => t.Value).FirstOrDefault().Key : TerrainType.GRASS;
        }

        foreach (Vector3Int cell in cellLocation) { 
            Vector2Int cellLocation2D = new Vector2Int(cell.x, cell.z);
            Vector3Int cellLocation3D = cell;
            TerrainType terrainType;

            if (TilemapCreator.AllTiles.ContainsKey(cellLocation3D)) {
                Debug.Log("Tile already exists at " + cellLocation3D);
                continue;
            }
            
            // Determine terrain type given surrounding tiles
            terrainType = DetermineTerrainType(cellLocation3D);

            // Create Tile (automatically traversable, flat, and forward)
            Tile tile = new Tile(cellLocation3D, TileType.Flat, terrainType, TileDirection.Forward, false, true, TilemapCreator.OverlayPrefabs[TileType.Flat]);

            TilemapCreator.AllTiles.Add(cellLocation3D, tile);
            if (TilemapCreator.TileLocator.ContainsKey(cellLocation2D)) {
                Tile existingTile = TilemapCreator.TileLocator[cellLocation2D];
                existingTile.TileInfo.IsTraversable = false;
                TilemapCreator.TileLocator.Remove(cellLocation2D);

                MonoBehaviour.Destroy(existingTile.OverlayObj.OverlayObj);
                TilemapCreator.TileLocator.Add(cellLocation2D, tile);
            }
            else {
                TilemapCreator.TileLocator.Add(cellLocation2D, tile);
            }
        }
    }

    public static void DestroyTiles(List<Tile> tiles) {
        void UpdateTileLocator(Tile tile, Vector2Int cellLocation2D) {
            tile.TileInfo.IsTraversable = true;

            // add tile to Tilemapcreator.TileLocator
            TilemapCreator.TileLocator.Add(cellLocation2D, tile);

            // Create Overlay tile if not yet created
            if (tile.OverlayObj == null)
                tile.OverlayObj = new OverlayTile(tile, TilemapCreator.OverlayPrefabs[tile.TileInfo.TileType]);
        }

        foreach (Tile tile in tiles) {
            Vector2Int cellLocation2D = new Vector2Int(tile.TileInfo.CellLocation.x, tile.TileInfo.CellLocation.z);
            Vector3Int cellLocation3D = tile.TileInfo.CellLocation;

            //skip tiles that have a unit on it
            if (TilemapCreator.UnitLocator.ContainsKey(cellLocation2D)) continue;

            //Remove tile instance from Tilecreator.TileLocator and Tilecreator.AllTiles
            if (TilemapCreator.TileLocator.ContainsKey(cellLocation2D))
                TilemapCreator.TileLocator.Remove(cellLocation2D);

            if(TilemapCreator.AllTiles.ContainsKey(cellLocation3D))
                TilemapCreator.AllTiles.Remove(cellLocation3D);

            //Add tile under destroyed tile to Tilecreator.Tilelocators
            if(TilemapCreator.AllTiles.ContainsKey(cellLocation3D + Vector3Int.down))
                UpdateTileLocator(TilemapCreator.AllTiles[cellLocation3D + Vector3Int.down], cellLocation2D);

            //Destroy tile gameObject from scene
            MonoBehaviour.Destroy(tile.TileObj);

            //Destroy overlay tile gameobject from scene
            if (tile.OverlayObj != null) MonoBehaviour.Destroy(tile.OverlayObj.OverlayObj);
        }
    }
    
    public bool IsSelectable() { return OverlayObj.IsSelectable; }
}