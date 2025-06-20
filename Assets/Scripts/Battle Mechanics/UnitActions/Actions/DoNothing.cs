using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoNothing : UnitAction
{
    public override string Name { get; protected set; } = "Do Nothing";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 0;
    public override int Priority { get; protected set; } = 0;
    public override DamageType DamageType { get; protected set; }
    public override int BasePower { get; protected set; } = 0;
    public override ActionType ActionType { get; protected set; } = ActionType.Wait;
    public override Pattern AttackPattern { get; protected set; } = Pattern.None;
    public override int Range { get; protected set; } = 0;
    public override AIActionScore ActionScore { get; protected set; }
    public override int Splash { get; protected set; } = 0;
    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) {
        return new List<Tile>();
    }

    public override string SlotImageAddress { get; protected set; }
    public override Sprite SlotImage()
    {
        throw new System.NotImplementedException();
    }

    public override float CalculateActionScore(EnemyUnit unit, Vector2Int selectedCell)
    {
        ActionScore = new AIActionScore();
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");
        Debug.Log("Initial Heuristic Score: " + ActionScore.TotalScore());
        
        ActionScore.EvaluateScore(this, unit, TilemapCreator.TileLocator[unit.unitInfo.Vector2CellLocation()].TileInfo.CellLocation,
            unit.unitInfo.CellLocation, new List<Unit>(), unit.FindNearbyUnits());
        
        Debug.Log("Best Heuristic Score: " + ActionScore.TotalScore());
        Debug.Log("Decided Cell Location: " + ActionScore.PotentialCell);
        return ActionScore.TotalScore();
    }

    public override void ActivateAction(Unit unit) {
        throw new System.NotImplementedException();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell) {
        yield return null;
    }
}
