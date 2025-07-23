using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothing : UnitAction {
    public override string Name { get; protected set; } = "Do Nothing";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 0;
    public override int BasePower { get; protected set; } = 0;
    public override int Range { get; protected set; } = 0;
    public override int Splash { get; protected set; } = 0;
    public override int Priority { get; protected set; } = 0;
    public override DamageType DamageType { get; protected set; } = DamageType.None;
    public override ActionType ActionType { get; protected set; } = ActionType.Wait;
    public override TilePattern AttackTilePattern { get; protected set; } = TilePattern.None;
    public override AIActionScore ActionScore { get; protected set; }
    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) { return new List<Tile>(); }
    public override string SlotImageAddress { get; protected set; }

    public override float CalculateActionScore(AIUnit unit, Vector2Int selectedCell) {
        ActionScore = new AIActionScore();
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");
        
        ActionScore.EvaluateScore(this, unit, TileLocator.SelectableTiles[unit.UnitInfo.Vector2CellLocation()].TileInfo.CellLocation,
            unit.UnitInfo.CellLocation, new List<Unit>(), AIUnitScanner.FindNearbyUnits(unit));
        
        Debug.Log("Best Heuristic Score: " + ActionScore.TotalScore());
        Debug.Log("Decided Cell Location: " + ActionScore.PotentialCell);
        return ActionScore.TotalScore();
    }

    public override void ActivateAction(Unit unit) { throw new System.NotImplementedException(); }
    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell) { yield return null; }
}
