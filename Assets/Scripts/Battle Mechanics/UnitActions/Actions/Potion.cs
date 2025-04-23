using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : UnitAction
{
    public override string Name { get; protected set; } = "Potion";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 1;
    public override int Priority { get; protected set; } = 1;
    public override DamageType DamageType { get; protected set; } = DamageType.Healing;
    public override int BasePower { get; protected set; } = 20;
    public override ActionType ActionType { get; protected set; } = ActionType.Item;
    public override Pattern AttackPattern { get; protected set; } = Pattern.Direct;
    public override int Range { get; protected set; } = 0;
    public override AIActionScore ActionScore { get; protected set; }
    public override int Splash { get; protected set; }
    public override List<Tile> Area(Unit unit) {
        return new List<Tile> { TilemapCreator.TileLocator[unit.unitInfo.Vector2CellLocation()] };
    }

    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_item";
    public override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    public override float CalculateActionScore(EnemyUnit unit, Vector2Int selectedCell)
    {
        ActionScore = new AIActionScore();
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");
        Debug.Log("Initial Heuristic Score: " + ActionScore.TotalScore());
        
        ActionScore.EvaluateScore(this, unit, TilemapCreator.TileLocator[unit.unitInfo.Vector2CellLocation()].TileInfo.CellLocation,
            unit.FindNearbyUnits()[0].unitInfo.CellLocation, new List<Unit>(), unit.FindNearbyUnits());
        
        Debug.Log("Best Heuristic Score: " + ActionScore.TotalScore());
        Debug.Log("Decided Cell Location: " + ActionScore.PotentialCell);
        return ActionScore.TotalScore();
    }

    public override void ActivateAction(Unit unit)
    {
        UnitMenu.HideMenu();
        ActionUtility.ShowSelectableTilesForAction(unit, Name);
        ChainSystem.HoldPotentialChain(this, unit);
        MapCursor.ActionState();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        // Spend an Action Point to execute the Action
        PayAPCost(unit);
        
        // Consume Item and Remove it from ActionSet
        if (UnitMenu.InSubMenu) {
            yield return UnitMenu.SubMenu.ExecuteAction(unit, selectedCell);
        } else {
            unit.unitInfo.ActionSet.RemoveAction(this);
        }
        
        // Heal Unit by Specified Amount
        yield return DamageDisplay.DisplayUnitDamage(unit.unitInfo, DamageCalculator.HealFixedAmount(BasePower, unit.unitInfo));

        if (unit.unitInfo.UnitAffiliation == UnitAffiliation.Player)
            CanvasUI.ShowTurnUnitInfoDisplay(unit.unitInfo);
        else
            CanvasUI.ShowTargetUnitInfoDisplay(unit.unitInfo);

        Debug.Log(unit.name + " is using a potion. HP: " + unit.unitInfo.currentHP + "/" + unit.unitInfo.finalHP);
        yield return null;
    }
}
