using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionUtility
{
    public static void ShowSelectableTilesForAction(Unit unit)
    {
        var unitLocation = unit.unitInfo.CellLocation;
        foreach (var tile in Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[new Vector2Int(unitLocation.x, unitLocation.z)], unit.unitInfo.finalMove))
        {
            tile.OverlayObj.ActivateOverlayTile(OverlayMaterial.MOVE);
        }
    }

    public static void HideSelectableTilesForAction(Unit unit)
    {
        var unitLocation = unit.unitInfo.CellLocation;
        foreach (var tile in Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[new Vector2Int(unitLocation.x, unitLocation.z)], unit.unitInfo.finalMove))
        {
            tile.OverlayObj.DeactivateOverlayTile();
        }
    }
}
