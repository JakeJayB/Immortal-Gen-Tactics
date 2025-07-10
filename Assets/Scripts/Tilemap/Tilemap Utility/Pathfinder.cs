using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinder
{
    public static List<Tile> FindPath(Tile start, Tile end)
    {
        Dictionary<Tile, float> openList = new Dictionary<Tile, float>();
        List<Tile> closedList = new List<Tile>();
        Dictionary<Tile, Tile> cameFrom = new Dictionary<Tile, Tile>(); // Parent tracking

        openList.Add(start, 0);

        while (openList.Count > 0)
        {
            // Get the tile with the lowest F-cost
            Tile currentTile = openList.OrderBy(x => x.Value).First().Key;

            openList.Remove(currentTile);
            closedList.Add(currentTile);

            if (currentTile == end)
            {
                if (TilemapCreator.UnitLocator.TryGetValue(currentTile.TileInfo.Vector2CellLocation(),
                        out var foundUnit))
                {
                    currentTile = cameFrom.Last().Value;
                    end = currentTile;
                }
                
                while (TilemapCreator.UnitLocator.TryGetValue(currentTile.TileInfo.Vector2CellLocation(),
                        out foundUnit) && currentTile != start)
                {
                    end = currentTile;
                    currentTile = cameFrom[currentTile];
                }
                
                return GetPath(cameFrom, start, end);
            }

            List<Tile> neighborTiles = TilemapUtility.GetNeighborTiles(currentTile);

            foreach (Tile neighbor in neighborTiles)
            {
                if (closedList.Contains(neighbor)) continue;

                float cost = GetDistance(start, neighbor);
                float heuristic = GetDistance(end, neighbor);
                float totalCost = cost + heuristic;

                if (!openList.ContainsKey(neighbor) || totalCost < openList[neighbor])
                {
                    openList[neighbor] = totalCost;
                    cameFrom[neighbor] = currentTile; // Store parent tile
                }
            }
        }

        // Return path if unable to reach the end tile
        return cameFrom.Count > 0 ? GetPath(cameFrom, start, cameFrom.Last().Key) : new List<Tile>();
    }

    private static List<Tile> GetPath(Dictionary<Tile, Tile> cameFrom, Tile start, Tile end)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = end;

        while (cameFrom.ContainsKey(currentTile))
        {
            path.Add(currentTile);
            currentTile = cameFrom[currentTile];
        }

        path.Add(start);
        path.Reverse(); 
        return path;
    }
    
    private static float GetDistance(Tile start, Tile neighbor)
    {
        float dx = start.TileInfo.CellLocation.x - neighbor.TileInfo.CellLocation.x;
        float dz = start.TileInfo.CellLocation.z - neighbor.TileInfo.CellLocation.z;
    
        return Mathf.Abs(dx) + Mathf.Abs(dz);
    }

    public static int DistanceBetweenUnits(Unit a, Unit b)
    {
        int dx = a.unitInfo.CellLocation.x - b.unitInfo.CellLocation.x;
        int dz = a.unitInfo.CellLocation.z - b.unitInfo.CellLocation.z;
    
        return Mathf.Abs(dx) + Mathf.Abs(dz);
    }
    
    public static int DistanceBetweenCells(Vector3Int cellA, Vector3Int cellB)
    {
        int dx = cellA.x - cellB.x;
        int dz = cellA.z - cellB.z;
    
        return Mathf.Abs(dx) + Mathf.Abs(dz);
    }

    public static Vector2Int ProjectedRushLocation(Unit unit, Vector2Int direction)
    {
        Vector2Int projectedLocation = Vector2Int.zero;
        Vector2Int startCell = unit.unitInfo.Vector2CellLocation();
        Vector2Int previousCell = startCell;
        
        for (int i = 1; i <= unit.unitInfo.FinalMove; i++)
        {
            Vector2Int nextCell = startCell + direction * i;

            // Stop the unit from trying to traverse null tile locations
            if (!TilemapCreator.TileLocator.TryGetValue(nextCell, out var tile))
            {
                projectedLocation = previousCell;
                break;
            }
            
            if (TilemapCreator.UnitLocator.TryGetValue(nextCell, out var targetUnit))
            {
                projectedLocation = TilemapCreator.UnitLocator.TryGetValue(nextCell, out var stillThere)
                    ? previousCell : nextCell;
                break;
            }

            previousCell = nextCell;

            if (i != unit.unitInfo.FinalMove) continue;
            projectedLocation = previousCell;
        }
        
        return projectedLocation;
    }
}
