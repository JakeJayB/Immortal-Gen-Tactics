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

        // No valid path found
        return new List<Tile>();
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
    
        return Mathf.Sqrt(dx * dx + dz * dz); // Euclidean Distance
    }
}
