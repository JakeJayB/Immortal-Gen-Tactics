using System.Collections;
using System.Collections.Generic;
using IGT.Core;
using UnityEngine;

public class SplashSpell : UnitAction
{
    public override string Name { get; protected set; } = "SplashSpell";
    public override int MPCost { get; protected set; } = 5;
    public override int APCost { get; protected set; } = 1;
    public override int BasePower { get; protected set; } = 10;
    public override int Range { get; protected set; } = 3;
    public override int Splash { get; protected set; } = 1;
    public override int Priority { get; protected set; } = 3;
    public override DamageType DamageType { get; protected set; } = DamageType.Magic;
    public override ActionType ActionType { get; protected set; } = ActionType.Attack;
    public override UnitClass ClassType { get; protected set; } = UnitClass.Mage;
    public override TilePattern AttackTilePattern { get; protected set; } = TilePattern.Splash;
    public override AIActionScore ActionScore { get; protected set; }
    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) {
        return Rangefinder.GetTilesInRange(TileLocator.SelectableTiles[hypoCell.HasValue
                ? new Vector2Int(hypoCell.Value.x, hypoCell.Value.z)
                : unit.UnitInfo.Vector2CellLocation()],
            Range, TilePattern.Splash);
    }
    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_attack";
    
    public override float CalculateActionScore(AIUnit unit, Vector2Int selectedCell) {
        ActionScore = new AIActionScore();
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");

        foreach (var tile in Area(unit, null)) {
            foreach (var targetedTile in TilemapUtility.GetSplashTilesInRange(tile, Splash)) {
                if (TilemapCreator.UnitLocator.TryGetValue(targetedTile.TileInfo.Vector2CellLocation(), out Unit foundUnit)) {
                    if (foundUnit.UnitInfo.IsDead()) { continue; }
                    
                    AIActionScore newScore = new AIActionScore().EvaluateScore(this, unit, tile.TileInfo.CellLocation,
                        foundUnit.UnitInfo.CellLocation, new List<Unit>(), AIUnitScanner.FindNearbyUnits(unit));
                    
                    if (newScore.TotalScore() > ActionScore.TotalScore()) ActionScore = newScore;
                    break;
                }
            }
        }
        Debug.Log("Best Heuristic Score: " + ActionScore.TotalScore());
        Debug.Log("Decided Cell Location: " + ActionScore.PotentialCell);
        return ActionScore.TotalScore();
    }

    public override void ActivateAction(Unit unit) {
        UnitMenu.HideMenu();
        ActionUtility.ShowSelectableTilesForAction(Area(unit, null));
        ChainSystem.HoldPotentialChain(this, unit);
        MapCursor.ActionState();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell) {
        PayAPCost(unit);    // Spend the Action Points to execute the Action
        PayMPCost(unit);    // Spend the Magic Points needed to execute the Action

        foreach (var tile in Rangefinder.GetTilesInRange(TileLocator.SelectableTiles[selectedCell], Splash,
                     AttackTilePattern)) {
            if (TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out var targetUnit)) {
                int damage = DamageCalculator.DealDamage(this, unit.UnitInfo, targetUnit.UnitInfo);
                SoundFXManager.PlaySoundFXClip("MagicHit", 0.45f);
                yield return DamageDisplay.DisplayUnitDamage(targetUnit, damage);
            }
        }
        yield return null;
    }
}
