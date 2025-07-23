using System.Collections.Generic;
using UnityEngine;

public class AIUnitScanner {
    // TODO: AI doesn't perform any organic decision-making if it doesn't recognize an enemy.
    // TODO: Switch the value of 20 back to the unit's finalMove.
    public static List<Unit> FindNearbyUnits(AIUnit unitAI) {
        List<Unit> nearbyUnits = new List<Unit>();
        
        // Check Units based on Unit's Movement Range for now until finalized
        // It will save an AP for an action once they select and move towards the opponent
        var surroundings = Rangefinder.GetTilesInRange(TileLocator.SelectableTiles[unitAI.UnitInfo.Vector2CellLocation()],
            30 * (unitAI.UnitInfo.currentAP), TilePattern.Splash);

        // Don't Count the same tile as the Unit conducting the search
        surroundings.Remove(TileLocator.SelectableTiles[unitAI.UnitInfo.Vector2CellLocation()]);

        foreach (Tile tile in surroundings) {
            var cell = new Vector2Int(tile.TileInfo.CellLocation.x, tile.TileInfo.CellLocation.z);
            if (TilemapCreator.UnitLocator.TryGetValue(cell, out Unit unit)) {
                if (!unit.UnitInfo.IsDead() && unit.UnitInfo.UnitAffiliation != unitAI.UnitInfo.UnitAffiliation) { nearbyUnits.Add(unit); }
            }
        }

        return nearbyUnits;
    }
}
