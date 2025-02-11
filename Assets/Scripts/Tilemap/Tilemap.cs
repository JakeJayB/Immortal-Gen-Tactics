using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilemap : MonoBehaviour
{
    public static Dictionary<Vector2Int, Tile> TileLocator { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        TileLocator = new Dictionary<Vector2Int, Tile>();

        /*        List<Tile> testTiles = new List<Tile>()
                {
                    new Tile(new Vector3Int(-3, 1, -4), TileType.Slanted_Corner, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-3, 1, -5), TileType.Slanted, TerrainType.Test, TileDirection.Left),
                    new Tile(new Vector3Int(-4, 1, -4), TileType.Slanted, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-4, 1, -5), TileType.Flat, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-5, 1, -4), TileType.Stairs, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-5, 1, -5), TileType.Flat, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-3, 0, -3), TileType.Flat, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-4, 0, -3), TileType.Flat, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-5, 0, -3), TileType.Flat, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-6, 0, -3), TileType.Flat, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-3, 0, -4), TileType.Flat, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-4, 0, -4), TileType.Flat, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-4, 0, -5), TileType.Flat, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-5, 0, -4), TileType.Flat, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-5, 0, -5), TileType.Flat, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-6, 0, -4), TileType.Flat, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-3, 0, -5), TileType.Flat, TerrainType.Test, TileDirection.Forward),
                    new Tile(new Vector3Int(-6, 0, -5), TileType.Flat, TerrainType.Test, TileDirection.Forward)
                };*/

        List<Tile> testTiles = new List<Tile>()
        {
            new Tile(new Vector3Int(0,0,0), TileType.Flat, TerrainType.Test, TileDirection.Forward, true),
            new Tile(new Vector3Int(0,0,1), TileType.Flat, TerrainType.Test, TileDirection.Forward, false),
            new Tile(new Vector3Int(0,0,-1), TileType.Flat, TerrainType.Test, TileDirection.Forward, false),
            new Tile(new Vector3Int(1,0,0), TileType.Flat, TerrainType.Test, TileDirection.Forward, false),
            new Tile(new Vector3Int(1,0,1), TileType.Flat, TerrainType.Test, TileDirection.Forward, false),
            new Tile(new Vector3Int(1,0,-1), TileType.Flat, TerrainType.Test, TileDirection.Forward, false),
            new Tile(new Vector3Int(-1,0,0), TileType.Flat, TerrainType.Test, TileDirection.Forward, false),
            new Tile(new Vector3Int(-1,0,-1), TileType.Flat, TerrainType.Test, TileDirection.Forward, false),
            new Tile(new Vector3Int(-1,0,1), TileType.Flat, TerrainType.Test, TileDirection.Forward , false),
            new Tile(new Vector3Int(1,1,-1), TileType.Flat, TerrainType.Test, TileDirection.Forward, false)
        };
        
        CreateTilemap(testTiles);

        //Unit unit = Unit.Initialize(new Vector3Int(-4, 1, - 5));
        //Unit unit = Unit.Initialize(new Vector3Int(0, 0, 0));

    }


    // TODO: Only place traversable tiles inside of TileLocator and not every single physical tile.
    // TODO: Traversable tiles are all unique tile positions (x, y) of the highest z-value for each.
    /*    public void CreateTilemap(List<TileData> tileData)
        {
            foreach (var data in tileData)
            {
                data = data.
                Tile tile = new Tile(data.CellLocation(), data.tileType, data.terrainType, data.direction);
                if (!TileLocator.ContainsKey(new Vector2Int(tile.CellLocation.x, tile.CellLocation.z))) {
                    TileLocator.Add(new Vector2Int(tile.CellLocation.x, tile.CellLocation.z), tile);
                }
            }
        }*/

    // This is a simple function used to test cell creation.
    public void CreateTilemap(List<Tile> tileData)
    {
        foreach (Tile tile in tileData)
        {
            TileInfo tileInfo = tile.TileInfo;
            if (!TileLocator.ContainsKey(new Vector2Int(tileInfo.CellLocation.x, tileInfo.CellLocation.z))) {
                TileLocator.Add(new Vector2Int(tileInfo.CellLocation.x, tileInfo.CellLocation.z), tile);
            }
        }
    }
}