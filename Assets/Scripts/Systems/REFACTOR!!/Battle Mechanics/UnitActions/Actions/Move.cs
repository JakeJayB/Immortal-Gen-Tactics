using System.Collections;
using System.Collections.Generic;
using IGT.Core;
using UnityEngine;

public class Move : UnitAction
{
    public sealed override string Name { get; protected set; } = "Move";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 1;
    public override int BasePower { get; protected set; } = 0;
    public override int Range { get; protected set; } = 0;
    public override int Splash { get; protected set; } = 0;
    public override int Priority { get; protected set; } = 0;
    public override DamageType DamageType { get; protected set; } = DamageType.None;
    public override ActionType ActionType { get; protected set; } = ActionType.Move;
    public override UnitClass ClassType { get; protected set; }
    public override TilePattern AttackTilePattern { get; protected set; } = TilePattern.None;
    public override AIActionScore ActionScore { get; protected set; }
    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) {
        return Rangefinder.GetMoveTilesInRange(TileLocator.SelectableTiles[hypoCell.HasValue
                ? new Vector2Int(hypoCell.Value.x, hypoCell.Value.z)
                : unit.UnitInfo.Vector2CellLocation()],
            unit.UnitInfo.FinalMove);
    }
    public sealed override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_walk";

    public override float CalculateActionScore(AIUnit unit, Vector2Int selectedCell) {
        ActionScore = new AIActionScore();
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");

        foreach (var tile in Area(unit, null)) {
            if (TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out Unit foundUnit)) { continue; }

            AIActionScore newScore = new AIActionScore().EvaluateScore(this, unit, tile.TileInfo.CellLocation,
                TilemapCreator.UnitLocator[selectedCell].UnitInfo.CellLocation, new List<Unit>(), AIUnitScanner.FindNearbyUnits(unit));
            
            if (newScore.TotalScore() > ActionScore.TotalScore()) ActionScore = newScore;
        }
        Debug.Log("Best Heuristic Score: " + ActionScore.TotalScore());
        Debug.Log("Decided Cell Location: " + ActionScore.PotentialCell);
        return ActionScore.TotalScore();
    }

    public override void ActivateAction(Unit unit) {
        UnitMenu.HideMenu();
        ActionUtility.ShowSelectableTilesForMove(Area(unit, null));
        ChainSystem.HoldPotentialChain(this, unit);
        MapCursor.ActionState();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell) {
        // Have AI Units show their range of movement before moving
        if (unit is AIUnit) {
            ActionUtility.ShowSelectableTilesForMove(Area(unit, null));
            yield return new WaitForSeconds(2.0f);
            ActionUtility.HideSelectableTilesForAction(Area(unit, null));
        }
        
        // Spend an Action Point to execute the Action
        PayAPCost(unit);
        
        // Remove the Location the Unit is currently at in UnitLocator
        TilemapCreator.UnitLocator.Remove(new Vector2Int(unit.UnitInfo.CellLocation.x, unit.UnitInfo.CellLocation.z));
        
        // Updates the location as the Unit moves
        yield return UnitMovement.Move(unit, selectedCell);
        
        // Adds the location of the tile the Unit ended at in UnitLocator
        TilemapCreator.UnitLocator.Add(new Vector2Int(unit.UnitInfo.CellLocation.x, unit.UnitInfo.CellLocation.z), unit);
        
        CanvasUI.ShowTurnUnitInfoDisplay(unit.UnitInfo);
        CanvasUI.HideTargetUnitInfoDisplay();

        if (unit is not AIUnit) { MapCursor.currentUnit = selectedCell; }
    }
}
