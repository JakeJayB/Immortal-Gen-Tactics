using System.Collections;
using System.Collections.Generic;
using IGT.Core;
using UnityEngine;

public class Wait : UnitAction {
    public sealed override string Name { get; protected set; } = "Wait";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 0;
    public override int Priority { get; protected set; } = 0;
    public override DamageType DamageType { get; protected set; } = DamageType.None;
    public override int BasePower { get; protected set; } = 0;
    public override ActionType ActionType { get; protected set; } = ActionType.Wait;
    public override UnitClass ClassType { get; protected set; }
    public override TilePattern AttackTilePattern { get; protected set; } = TilePattern.None;
    public override int Range { get; protected set; } = 0;
    public override AIActionScore ActionScore { get; protected set; }
    public override int Splash { get; protected set; }
    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) { return new List<Tile>(); }
    public sealed override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_wait";
    
    public override float CalculateActionScore(AIUnit unit, Vector2Int selectedCell) {
        ActionScore = new AIActionScore();
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");
        
        ActionScore.EvaluateScore(this, unit, TileLocator.SelectableTiles[unit.UnitInfo.Vector2CellLocation()].TileInfo.CellLocation,
            unit.UnitInfo.CellLocation, new List<Unit>(), AIUnitScanner.FindNearbyUnits(unit));
        
        Debug.Log("Best Heuristic Score: " + ActionScore.TotalScore());
        Debug.Log("Decided Cell Location: " + ActionScore.PotentialCell);
        return ActionScore.TotalScore();
    }

    public override void ActivateAction(Unit unit)
    {
        UnitMenu.HideMenu();
        MapCursor.EndMove();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        // TODO: Have ActivateAction() call a direction selector for the player to choose which
        // TODO: direction to face the unit. Once selected, it should call this function to
        // TODO: end the move after instead of ActivateAction().
        
        MapCursor.EndMove();
        yield return null;
    }
}
