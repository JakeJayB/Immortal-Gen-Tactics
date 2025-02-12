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
                    new Tile(new Vector3Int(-3, 1, -4), TileType.Slanted_Corner, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-3, 1, -5), TileType.Slanted, TerrainType.STANDARD, TileDirection.Left),
                    new Tile(new Vector3Int(-4, 1, -4), TileType.Slanted, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-4, 1, -5), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-5, 1, -4), TileType.Stairs, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-5, 1, -5), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-3, 0, -3), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-4, 0, -3), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-5, 0, -3), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-6, 0, -3), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-3, 0, -4), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-4, 0, -4), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-4, 0, -5), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-5, 0, -4), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-5, 0, -5), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-6, 0, -4), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-3, 0, -5), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward),
                    new Tile(new Vector3Int(-6, 0, -5), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward)
                };*/

        List<Tile> testTiles = new List<Tile>()
        {
            new Tile(new Vector3Int(0,0,0), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward, true),
            new Tile(new Vector3Int(0,0,1), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward, false),
            new Tile(new Vector3Int(0,0,-1), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward, false),
            new Tile(new Vector3Int(1,0,0), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward, false),
            new Tile(new Vector3Int(1,0,1), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward, false),
            new Tile(new Vector3Int(1,0,-1), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward, false),
            new Tile(new Vector3Int(-1,0,0), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward, false),
            new Tile(new Vector3Int(-1,0,-1), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward, false),
            new Tile(new Vector3Int(-1,0,1), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward , false),
            new Tile(new Vector3Int(1,1,-1), TileType.Flat, TerrainType.STANDARD, TileDirection.Forward, false)
        };
        
        CreateTilemap(testTiles);

        Unit unit = Unit.Initialize(new Vector3Int(0, 0, 0));

    }

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