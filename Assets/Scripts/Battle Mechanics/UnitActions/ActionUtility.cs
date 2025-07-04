using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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

    public static void Clear()
    {
        action = null;
    }

    public static void RegisterCleanup()
    {
        MemoryManager.AddListeners(Clear);
    }

    public static Tuple<List<Tile>, OverlayMaterial> DetermineParameters(string actionType, Unit unit) 
    {
        var unitLocation = unit.unitInfo.CellLocation;
        switch (actionType)
        {
            case "Move":
                return new Tuple<List<Tile>, OverlayMaterial>(Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[new Vector2Int(unitLocation.x, unitLocation.z)], unit.unitInfo.FinalMove, Pattern.Splash), OverlayMaterial.MOVE);
            case "Attack":
                return new Tuple<List<Tile>, OverlayMaterial>(Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[new Vector2Int(unitLocation.x, unitLocation.z)], unit.unitInfo.FinalAttack, Pattern.Linear), OverlayMaterial.ATTACK);
            case "SplashSpell (Test)":
                return new Tuple<List<Tile>, OverlayMaterial>(Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[new Vector2Int(unitLocation.x, unitLocation.z)], 3, Pattern.Splash), OverlayMaterial.ATTACK);
            case "Potion":
                return new Tuple<List<Tile>, OverlayMaterial>(
                    Rangefinder.GetTilesInRange(
                        TilemapCreator.TileLocator[new Vector2Int(unitLocation.x, unitLocation.z)], 0, Pattern.Splash),
                    OverlayMaterial.MOVE);
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
        
        HideSelectableTiles();

        ActionUtility.action = action;
        var parameters = DetermineParameters(ActionUtility.action, unit);
        List<Tile> tiles = parameters.Item1;
        OverlayMaterial overlayMaterial = parameters.Item2;

        foreach (var tile in tiles)
        {
            tile.OverlayObj.ActivateOverlayTile(overlayMaterial);
        }
    }
    
    public static void ShowSelectableTilesForAction(List<Tile> area) 
    {
        //if (ChainSystem.UnitIsReacting()) { HideAllSelectableTiles(); }
        if (ChainSystem.UnitIsReacting())
        {
            foreach (var tile in TilemapCreator.TileLocator.Values)
            {
                tile.OverlayObj.DeactivateOverlayTile();
            }
        }

        foreach (var tile in area) {
            tile.OverlayObj.ActivateOverlayTile(OverlayMaterial.ATTACK);
        } 
    }
    
    public static void ShowSelectableTilesForMove(List<Tile> area) 
    {
        //if (ChainSystem.UnitIsReacting()) { HideAllSelectableTiles(); }
        if (ChainSystem.UnitIsReacting())
        {
            foreach (var tile in TilemapCreator.TileLocator.Values)
            {
                tile.OverlayObj.DeactivateOverlayTile();
            }
        }

            foreach (var tile in area) {
            tile.OverlayObj.ActivateOverlayTile(OverlayMaterial.MOVE);
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
    
    public static void HideSelectableTilesForAction(List<Tile> area)
    {
        foreach (var tile in area) {
            tile.OverlayObj.DeactivateOverlayTile();
        }
    }
    
    public static void HideSelectableTiles()
    {
        foreach (var tile in TilemapCreator.TileLocator.Values) {
            tile.OverlayObj.DeactivateOverlayTile();
        }
    }

    public static void HideAllSelectableTiles() {
        foreach (var tile in TilemapCreator.TileLocator.Values) {
            tile.OverlayObj.DeactivateOverlayTile();
        }

        var initialChain = ChainSystem.GetInitialChain();
        if (ChainSystem.UnitIsReacting()) { TilemapUtility.ShowTargetedArea(TilemapUtility.GetTargetedArea(initialChain.unit, initialChain.action, initialChain.target)); }
        
        action = null;
    }
}
