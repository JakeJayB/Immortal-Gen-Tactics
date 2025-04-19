using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum Pattern
{
    Direct,
    Linear,
    Arc,
    Splash,
    All,
    None
}

public class Rangefinder
{

    public static List<Tile> GetTilesInRange(Tile characterTile, int range, Pattern rangePattern)
    {
        switch (rangePattern)
        {
            case Pattern.Direct:
                return TilemapUtility.GetDirectTile(characterTile, range);
            case Pattern.Linear:
                return TilemapUtility.GetLinearTilesInRange(characterTile, range);
            case Pattern.Arc:
                Debug.LogError("Rangefinder: Arc pattern not implemented yet. Returning null");
                return TilemapUtility.GetArcTiles(characterTile, range);
            case Pattern.Splash:
                return TilemapUtility.GetSplashTilesInRange(characterTile, range);
            case Pattern.All:
                return TilemapUtility.GetAllTiles();
            default:
                return null;

        }
    }
}


    /*    public static List<Tile> GetTilesInRange(Tile characterTile, int range)
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
                    surroundingTiles.AddRange(TilemapUtility.GetNeighborTiles(tile));
                }

                inRangeTiles.AddRange(surroundingTiles);
                tileForPreviousStep = surroundingTiles.Distinct().ToList();
                stepCount++;
            }

            return inRangeTiles.Distinct().ToList();
        }*/
