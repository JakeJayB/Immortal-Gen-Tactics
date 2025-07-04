using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : Item
{
    public override string Name { get; protected set; } = "Potion";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 1;
    public override int Priority { get; protected set; } = 1;
    public override DamageType DamageType { get; protected set; } = DamageType.Healing;
    public override int BasePower { get; protected set; } = 20;
    public override ActionType ActionType { get; protected set; } = ActionType.Item;
    public override Pattern AttackPattern { get; protected set; } = Pattern.Direct;
    public override int Range { get; protected set; } = 1;
    public override AIActionScore ActionScore { get; protected set; }
    public override int Splash { get; protected set; } = 0;
    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) {
        return TilemapUtility.GetSplashTilesInRange(TilemapCreator.TileLocator[hypoCell.HasValue
            ? new Vector2Int(hypoCell.Value.x, hypoCell.Value.z)
            : unit.unitInfo.Vector2CellLocation()], Range);
    }

    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_item";
    public override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    public override float CalculateActionScore(EnemyUnit unit, Vector2Int selectedCell)
    {
        ActionScore = null;
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");
        
        foreach (var tile in Area(unit, null))
        {
            if (TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out Unit foundUnit))
            {
                if (foundUnit.unitInfo.IsDead()) { continue; }
                
                AIActionScore newScore = new AIActionScore().EvaluateScore(this, unit, tile.TileInfo.CellLocation,
                    foundUnit.unitInfo.CellLocation, new List<Unit>(), unit.FindNearbyUnits());
            
                // Debug.Log("Heuristic Score at Tile " + tile.TileInfo.CellLocation + ": " + newScore.TotalScore());
                if (ActionScore == null || newScore.TotalScore() > ActionScore.TotalScore()) ActionScore = newScore;
            }
        }
        
        Debug.Log("Best Heuristic Score: " + (ActionScore?.DamageScore <= 0 ? "N/A" : ActionScore?.TotalScore()));
        Debug.Log("Decided Cell Location: " + ActionScore?.PotentialCell);
        return ActionScore!.DamageScore <= 0 ? -9999 : ActionScore.TotalScore();
    }
    
    public override void ActivateAction(Unit unit)
    {
        Store(UnitMenu.SubMenu);
        UnitMenu.HideMenu();
        ActionUtility.ShowSelectableTilesForAction(Area(unit, null));
        ChainSystem.HoldPotentialChain(this, unit);
        MapCursor.ActionState();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        // Spend an Action Point to execute the Action
        PayAPCost(unit);
        
        // Consume Item and Remove it from ActionSet
        yield return Consume(unit, selectedCell);

        if (TilemapCreator.UnitLocator.TryGetValue(selectedCell, out var foundUnit))
        {
            // Heal Unit by Specified Amount
            int heal = DamageCalculator.HealFixedAmount(BasePower, foundUnit.unitInfo);
            SoundFXManager.PlaySoundFXClip("HealPotion", 0.45f);
            yield return DamageDisplay.DisplayUnitDamage(foundUnit.unitInfo, heal);
        }

        Debug.Log(unit.name + " is using a potion. HP: " + unit.unitInfo.currentHP + "/" + unit.unitInfo.FinalHP);
        yield return null;
    }
}
