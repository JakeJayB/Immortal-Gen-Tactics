using System.Collections;
using System.Collections.Generic;
using IGT.Core;
using UnityEngine;

public class Ether : Item {
    public override string Name { get; protected set; } = "Ether";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 1;
    public override int BasePower { get; protected set; } = 10;
    public override int Range { get; protected set; } = 1;
    public override int Splash { get; protected set; } = 0;
    public override int Priority { get; protected set; } = 0;
    public override DamageType DamageType { get; protected set; } = DamageType.Healing;
    public override ActionType ActionType { get; protected set; } = ActionType.Item;
    public override UnitClass ClassType { get; protected set; }
    public override TilePattern AttackTilePattern { get; protected set; } = TilePattern.Direct;
    public override AIActionScore ActionScore { get; protected set; }
    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) {
        return TilemapUtility.GetSplashTilesInRange(TileLocator.SelectableTiles[hypoCell.HasValue
            ? new Vector2Int(hypoCell.Value.x, hypoCell.Value.z)
            : unit.UnitInfo.Vector2CellLocation()], Range);
    }
    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_item";

    // TODO: Fix this. Borrows from Potion.
    public override float CalculateActionScore(AIUnit unit, Vector2Int selectedCell) {
        ActionScore = new AIActionScore();
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");
        
        ActionScore.EvaluateScore(this, unit, TileLocator.SelectableTiles[unit.UnitInfo.Vector2CellLocation()].TileInfo.CellLocation,
            AIUnitScanner.FindNearbyUnits(unit)[0].UnitInfo.CellLocation, new List<Unit>(), AIUnitScanner.FindNearbyUnits(unit));
        
        Debug.Log("Best Heuristic Score: " + (ActionScore.TotalScore() < 0 ? "N/A" : ActionScore.TotalScore()));
        Debug.Log("Decided Cell Location: " + ActionScore.PotentialCell);
        return ActionScore.TotalScore() < 0 ? -9999 : ActionScore.TotalScore();
    }
    
    public override void ActivateAction(Unit unit) {
        AccessStorage(UnitMenu.SubMenu);
        UnitMenu.HideMenu();
        ActionUtility.ShowSelectableTilesForMove(Area(unit, null)); // TODO: OPTIMIZE THIS LATER!!
        ChainSystem.HoldPotentialChain(this, unit);
        MapCursor.ActionState();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell) {
        PayAPCost(unit);                            // Spend an Action Point to execute the Action
        yield return Consume(unit, selectedCell);   // Consume Item and Remove it from ActionSet
        
        if (TilemapCreator.UnitLocator.TryGetValue(selectedCell, out var foundUnit)) {
            // Heal Unit by Specified Amount
            SoundFXManager.PlaySoundFXClip("HealPotion", 0.45f);
            yield return DamageDisplay.DisplayUnitDamage(foundUnit, DamageCalculator.HealFixedAmountMP(BasePower, foundUnit.UnitInfo));

            if (unit.UnitInfo.UnitAffiliation == UnitAffiliation.Player) { CanvasUI.ShowTurnUnitInfoDisplay(unit.UnitInfo); }
            else { CanvasUI.ShowTargetUnitInfoDisplay(unit.UnitInfo); }
        }
        
        Debug.Log(unit.GameObj.name + " is using a potion. HP: " + unit.UnitInfo.currentHP + "/" + unit.UnitInfo.FinalHP);
        yield return null;
    }
}
