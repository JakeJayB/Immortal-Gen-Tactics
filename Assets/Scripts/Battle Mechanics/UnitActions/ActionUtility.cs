using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class ActionUtility
{
    private static string action = null;

/*    public static void ShowSelectableTilesForAction(Unit unit)
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
    }*/

    private static Tuple<List<Tile>, OverlayMaterial> DetermineParameters(string actionType, Unit unit) 
    {
        var unitLocation = unit.unitInfo.CellLocation;
        switch (actionType)
        {
            case "Move":
                return new Tuple<List<Tile>, OverlayMaterial>(Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[new Vector2Int(unitLocation.x, unitLocation.z)], unit.unitInfo.finalMove), OverlayMaterial.MOVE);
            case "Attack":
                return new Tuple<List<Tile>, OverlayMaterial>(Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[new Vector2Int(unitLocation.x, unitLocation.z)], unit.unitInfo.finalAttack), OverlayMaterial.ATTACK);
            default:
                throw new ArgumentException("ActionUtility: Invalid action type");
        }
    }


    public static void ShowSelectableTilesForAction(Unit unit, string action)
    {
        if(ActionUtility.action != null)
        {
            Debug.LogError("ActionUtility: ActionUtility.action must be null. Returning without showing tiles");
            return;
        }

        ActionUtility.action = action;
        var parameters = DetermineParameters(ActionUtility.action, unit);
        List<Tile> tiles = parameters.Item1;
        OverlayMaterial overlayMaterial = parameters.Item2;

        foreach (var tile in tiles)
        {
            tile.OverlayObj.ActivateOverlayTile(overlayMaterial);
        }
    }

    public static void HideSelectableTilesForAction(Unit unit)
    {

        if (ActionUtility.action == null) 
        {
            Debug.LogError("ActionUtility: ActionUtility.action is null. Returning without hiding tiles"); 
            return; 
        }

        var parameters = DetermineParameters(ActionUtility.action, unit);
        List<Tile> tiles = parameters.Item1;

        foreach (var tile in tiles)
        {
            tile.OverlayObj.DeactivateOverlayTile();
        }
        
        ActionUtility.action = null;
    }
}
