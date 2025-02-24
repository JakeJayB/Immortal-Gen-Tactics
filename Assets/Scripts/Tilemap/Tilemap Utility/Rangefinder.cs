using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Rangefinder
{
    public static List<Tile> GetTilesInRange(Tile characterTile, int range)
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
    }
}
