using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class TilemapUtility
{

    public static List<Tile> GetArcTiles(Tile characterTile, int range)
    {
        return null;
    }


    public static List<Tile> GetDirectTile(Tile characterTile, int range)
    {
        Dictionary<Vector2Int, Tile> TileLocator = TilemapCreator.TileLocator;
        Vector2Int startPos = new Vector2Int(characterTile.TileInfo.CellLocation.x, characterTile.TileInfo.CellLocation.z);
        List<Tile> directTiles = new List<Tile>();

        Dictionary<UnitDirection, Vector2Int> directions = new Dictionary<UnitDirection, Vector2Int>
        {
            {UnitDirection.Forward, Vector2Int.up},
            {UnitDirection.Backward, Vector2Int.down},
            {UnitDirection.Left, Vector2Int.left},
            {UnitDirection.Right, Vector2Int.right}
        };

        Vector2Int direction = directions[TilemapCreator.UnitLocator[startPos].unitInfo.UnitDirection];
        for (int i = 1; i <= range; i++)
        {
            Vector2Int checkPos = startPos + direction * i;
            if (TileLocator.TryGetValue(checkPos, out var tile))
            {
                directTiles.Add(tile);
            }
        }

        return directTiles.Distinct().ToList();
    }




    public static List<Tile> GetLinearTilesInRange(Tile characterTile, int range)
    {
        Dictionary<Vector2Int, Tile> TileLocator = TilemapCreator.TileLocator;
        Vector2Int startPos = new Vector2Int(characterTile.TileInfo.CellLocation.x, characterTile.TileInfo.CellLocation.z);
        List<Tile> linearTiles = new List<Tile>();
        

        Vector2Int[] directions =
        {
            Vector2Int.right,
            Vector2Int.left,
            Vector2Int.up, 
            Vector2Int.down 
        };

        foreach (var direction in directions)
        {
            for (int i = 1; i <= range; i++)
            {
                Vector2Int checkPos = startPos + direction * i;
                if (TileLocator.TryGetValue(checkPos, out var tile))
                {
                    linearTiles.Add(tile);
                }
            }
        }

        return linearTiles.Distinct().ToList();
    }


    public static List<Tile> GetSplashTilesInRange(Tile characterTile, int range)
    {

        var inRangeTiles = new List<Tile>();
        int stepCount = 0;

        inRangeTiles.Add(characterTile);

        var tileForPreviousStep = new List<Tile>();
        tileForPreviousStep.Add(characterTile);

        while (stepCount < range)
        {
            var surroundingTiles = new List<Tile>();

            foreach (var tile in tileForPreviousStep)
            {
                surroundingTiles.AddRange(GetNeighborTiles(tile));
            }

            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }

        return inRangeTiles.Distinct().ToList();
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


    public static List<Tile> GetAllTiles()
    {
        return new List<Tile>(TilemapCreator.TileLocator.Values);
    }
    
    // Segment By Direction ------------------------------------------------------------------------------------------
    public static List<List<Tile>> GetDirectionalLinearTilesInRange(Tile characterTile, int range)
    {
        Dictionary<Vector2Int, Tile> TileLocator = TilemapCreator.TileLocator;
        Vector2Int startPos =
            new Vector2Int(characterTile.TileInfo.CellLocation.x, characterTile.TileInfo.CellLocation.z);
        List<List<Tile>> directionalTiles = new List<List<Tile>>();


        Vector2Int[] directions =
        {
            Vector2Int.right,
            Vector2Int.left,
            Vector2Int.up,
            Vector2Int.down
        };

        foreach (var direction in directions)
        {
            List<Tile> tilesInDirection = new List<Tile>();

            for (int i = 1; i <= range; i++)
            {
                Vector2Int checkPos = startPos + direction * i;
                if (TileLocator.TryGetValue(checkPos, out var tile))
                {
                    tilesInDirection.Add(tile);
                }
            }

            directionalTiles.Add(tilesInDirection);
        }

        return directionalTiles;
    }
}
