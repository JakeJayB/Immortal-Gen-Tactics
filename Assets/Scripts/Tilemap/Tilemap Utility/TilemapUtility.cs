using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilemapUtility : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static List<Tile> GetNeighborTiles(Tile currentTile)
    {
        Dictionary<Vector2Int, Tile> TileLocator = TilemapCreator.TileLocator;
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
