using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TilemapManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static void AddTiles(List<Vector3Int> cellLocation) {
        TerrainType DetermineTerrainType(Vector3Int cellLocation) {
            Dictionary<TerrainType, int> terrainCount = new Dictionary<TerrainType, int>();
            Vector3Int[] directions = new Vector3Int[]{ Vector3Int.left, Vector3Int.right, Vector3Int.forward, Vector3Int.back, Vector3Int.down, };

            foreach (var direction in directions) {
                Vector3Int neighborLocation = cellLocation + direction;
                if (TileLocator.TilemapTiles.ContainsKey(neighborLocation)) {
                    TerrainType terrain = TileLocator.TilemapTiles[neighborLocation].TileInfo.TerrainType;
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

            if (TileLocator.TilemapTiles.ContainsKey(cellLocation3D)) {
                Debug.Log("Tile already exists at " + cellLocation3D);
                continue;
            }
            
            // Determine terrain type given surrounding tiles
            terrainType = DetermineTerrainType(cellLocation3D);

            // Create Tile (automatically traversable, flat, and forward)
            Tile tile = new Tile(cellLocation3D, TileType.Flat, terrainType, TileDirection.Forward, 
                false, true, OverlayTilePrefabLibrary.FindPrefab(TileType.Flat));

            TileLocator.TilemapTiles.Add(cellLocation3D, tile);
            if (TileLocator.SelectableTiles.ContainsKey(cellLocation2D)) {
                Tile existingTile = TileLocator.SelectableTiles[cellLocation2D];
                existingTile.TileInfo.IsTraversable = false;
                TileLocator.SelectableTiles.Remove(cellLocation2D);

                MonoBehaviour.Destroy(existingTile.OverlayTile.OverlayObj);
                TileLocator.SelectableTiles.Add(cellLocation2D, tile);
            }
            else {
                TileLocator.SelectableTiles.Add(cellLocation2D, tile);
            }
        }
    }

    public static void DestroyTiles(List<Tile> tiles) {
        void UpdateTileLocator(Tile tile, Vector2Int cellLocation2D) {
            tile.TileInfo.IsTraversable = true;

            // add tile to Tilemapcreator.TileLocator
            TileLocator.SelectableTiles.Add(cellLocation2D, tile);

            // Create Overlay tile if not yet created
            if (tile.OverlayTile == null)
                tile.OverlayTile = new OverlayTile(tile, OverlayTilePrefabLibrary.FindPrefab(tile.TileInfo.TileType));
        }

        foreach (Tile tile in tiles) {
            Vector2Int cellLocation2D = new Vector2Int(tile.TileInfo.CellLocation.x, tile.TileInfo.CellLocation.z);
            Vector3Int cellLocation3D = tile.TileInfo.CellLocation;

            //skip tiles that have a unit on it
            if (TilemapCreator.UnitLocator.ContainsKey(cellLocation2D)) continue;

            //Remove tile instance from Tilecreator.TileLocator and Tilecreator.AllTiles
            if (TileLocator.SelectableTiles.ContainsKey(cellLocation2D))
                TileLocator.SelectableTiles.Remove(cellLocation2D);

            if(TileLocator.TilemapTiles.ContainsKey(cellLocation3D))
                TileLocator.TilemapTiles.Remove(cellLocation3D);

            //Add tile under destroyed tile to Tilecreator.Tilelocators
            if(TileLocator.TilemapTiles.ContainsKey(cellLocation3D + Vector3Int.down))
                UpdateTileLocator(TileLocator.TilemapTiles[cellLocation3D + Vector3Int.down], cellLocation2D);

            //Destroy tile gameObject from scene
            Destroy(tile.TileObj);

            //Destroy overlay tile gameobject from scene
            if (tile.OverlayTile != null) Destroy(tile.OverlayTile.OverlayObj);
        }
    }
}
