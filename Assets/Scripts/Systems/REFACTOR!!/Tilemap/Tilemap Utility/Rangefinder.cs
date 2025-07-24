using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Rangefinder
{
    public static List<Tile> GetTilesInRange(Tile characterTile, int range, TilePattern rangeTilePattern)
    {
        switch (rangeTilePattern)
        {
            case TilePattern.Direct:
                return TilemapUtility.GetDirectTile(characterTile, range);
            case TilePattern.Linear:
            case TilePattern.Rush:
                return TilemapUtility.GetLinearTilesInRange(characterTile, range);
            case TilePattern.Arc:
                Debug.LogError("Rangefinder: Arc pattern not implemented yet. Returning null");
                return TilemapUtility.GetArcTiles(characterTile, range);
            case TilePattern.Splash:
                return TilemapUtility.GetSplashTilesInRange(characterTile, range);
            case TilePattern.All:
                return TilemapUtility.GetSelectableTiles();
            default:
                return null;

        }
    }
    
    public static List<Tile> GetMoveTilesInRange(Tile characterTile, int range)
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
                if (stepCount != 0 && (TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out var foundUnit)))
                {
                    continue;
                }
                
                surroundingTiles.AddRange(TilemapUtility.GetNeighborTiles(tile));

                foreach (var possibleTile in surroundingTiles.ToList()) {
                    if (TilemapCreator.UnitLocator.TryGetValue(possibleTile.TileInfo.Vector2CellLocation(), out foundUnit)) {
                        surroundingTiles.Remove(possibleTile);
                    }
                }
            }

            inRangeTiles.AddRange(surroundingTiles);
            tileForPreviousStep = surroundingTiles.Distinct().ToList();
            stepCount++;
        }

        inRangeTiles.Remove(characterTile);
        return inRangeTiles.Distinct().ToList();
    }
}
