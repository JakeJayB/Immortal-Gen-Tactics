using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Revive : UnitAction
{
    public override string Name { get; protected set; } = "Revive";
    public override int MPCost { get; protected set; } = 10;
    public override int APCost { get; protected set; } = 2;
    public override int Priority { get; protected set; } = 5;
    public override DamageType DamageType { get; protected set; } = DamageType.Revival;
    public override int BasePower { get; protected set; } = 15;
    public override ActionType ActionType { get; protected set; } = ActionType.Attack;
    public override Pattern AttackPattern { get; protected set; } = Pattern.Direct;
    public override int Range { get; protected set; } = 2;
    public override AIActionScore ActionScore { get; protected set; }
    public override int Splash { get; protected set; } = 0;
    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) {
        return Rangefinder.GetMoveTilesInRange(TilemapCreator.TileLocator[hypoCell.HasValue
                ? new Vector2Int(hypoCell.Value.x, hypoCell.Value.z)
                : unit.unitInfo.Vector2CellLocation()],
            Range);
    }

    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_attack";
    public override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }

    public override float CalculateActionScore(AIUnit unit, Vector2Int selectedCell)
    {
        ActionScore = new AIActionScore();
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");
        Debug.Log("Initial Heuristic Score: " + ActionScore.TotalScore());
        
        foreach (var tile in Area(unit, null))
        {
            if (TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out Unit foundUnit))
            {
                if (!foundUnit.unitInfo.IsDead()) { continue; }
                
                AIActionScore newScore = new AIActionScore().EvaluateScore(this, unit, tile.TileInfo.CellLocation,
                    foundUnit.unitInfo.CellLocation, new List<Unit>(), unit.FindNearbyUnits());
            
                // Debug.Log("Heuristic Score at Tile " + tile.TileInfo.CellLocation + ": " + newScore.TotalScore());
                if (newScore.TotalScore() > ActionScore.TotalScore()) ActionScore = newScore;
            }
        }
        
        Debug.Log("Best Heuristic Score: " + ActionScore.TotalScore());
        Debug.Log("Decided Cell Location: " + ActionScore.PotentialCell);
        return ActionScore.TotalScore();
    }

    public override void ActivateAction(Unit unit)
    {
        UnitMenu.HideMenu();
        ActionUtility.ShowSelectableTilesForAction(Area(unit, null));
        ChainSystem.HoldPotentialChain(this, unit);
        MapCursor.ActionState();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        // Spend the Action Points to execute the Action
        PayAPCost(unit);
        
        // Spend the Magic Points needed to execute the Action
        PayMPCost(unit);

        if (TilemapCreator.UnitLocator.TryGetValue(selectedCell, out var foundUnit))
        {
            if (foundUnit.unitInfo.IsDead())
            {
                // Revive Unit
                foundUnit.unitInfo.Revive();
                
                // Heal Unit by Specified Amount
                int heal = DamageCalculator.HealDamage(this, unit.unitInfo, foundUnit.unitInfo);
                SoundFXManager.PlaySoundFXClip("HealPotion", 0.45f);
                yield return DamageDisplay.DisplayUnitDamage(foundUnit.unitInfo, heal);
            }
        }
        
        yield return null;
    }
}
