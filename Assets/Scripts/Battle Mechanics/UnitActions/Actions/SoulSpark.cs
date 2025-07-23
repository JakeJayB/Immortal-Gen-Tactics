using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulSpark : Item {
    public override string Name { get; protected set; } = "SoulSpark";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 1;
    public override int BasePower { get; protected set; } = 10;
    public override int Range { get; protected set; } = 1;
    public override int Splash { get; protected set; } = 0;
    public override int Priority { get; protected set; } = 1;
    public override DamageType DamageType { get; protected set; } = DamageType.Revival;
    public override ActionType ActionType { get; protected set; } = ActionType.Item;
    public override TilePattern AttackTilePattern { get; protected set; } = TilePattern.Direct;
    public override AIActionScore ActionScore { get; protected set; }
    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) {
        return TilemapUtility.GetLinearTilesInRange(TileLocator.SelectableTiles[hypoCell.HasValue
            ? new Vector2Int(hypoCell.Value.x, hypoCell.Value.z)
            : unit.UnitInfo.Vector2CellLocation()], Range);
    }
    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_attack";

    public override float CalculateActionScore(AIUnit unit, Vector2Int selectedCell) {
        ActionScore = new AIActionScore();
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");
        
        foreach (var tile in Area(unit, null)) {
            if (TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out Unit foundUnit)) {
                if (!foundUnit.UnitInfo.IsDead()) { continue; }
                
                AIActionScore newScore = new AIActionScore().EvaluateScore(this, unit, tile.TileInfo.CellLocation,
                    foundUnit.UnitInfo.CellLocation, new List<Unit>(), AIUnitScanner.FindNearbyUnits(unit));
                
                if (newScore.TotalScore() > ActionScore.TotalScore()) ActionScore = newScore;
            }
        }

        Debug.Log("Best Heuristic Score: " + (ActionScore.DamageScore <= 0 ? "N/A" : ActionScore.TotalScore()));
        Debug.Log("Decided Cell Location: " + ActionScore.PotentialCell);
        return ActionScore.DamageScore <= 0 ? -9999 : ActionScore.TotalScore();
    }

    public override void ActivateAction(Unit unit) {
        AccessStorage(UnitMenu.SubMenu);
        UnitMenu.HideMenu();
        ActionUtility.ShowSelectableTilesForAction(Area(unit, null));
        ChainSystem.HoldPotentialChain(this, unit);
        MapCursor.ActionState();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell) {
        PayAPCost(unit);                            // Spend an Action Point to execute the Action
        yield return Consume(unit, selectedCell);   // Consume Item and Remove it from ActionSet

        if (!TilemapCreator.UnitLocator.TryGetValue(selectedCell, out var foundUnit)) yield break;
        if (!foundUnit.UnitInfo.IsDead()) yield break;
        
        foundUnit.UnitInfo.Revive();    // Revive Unit
                
        // Heal Unit by Specified Amount
        int heal = DamageCalculator.HealDamage(this, unit.UnitInfo, foundUnit.UnitInfo);
        SoundFXManager.PlaySoundFXClip("HealPotion", 0.45f);
        yield return DamageDisplay.DisplayUnitDamage(foundUnit, heal);
    }
}
