using System.Collections;
using System.Collections.Generic;
using IGT.Core;
using UnityEngine;

public class Attack : UnitAction {
    public sealed override string Name { get; protected set; } = "Attack";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 1;
    public override int BasePower { get; protected set; } = 0;
    public override int Range { get; protected set; } = 1; // FIX!! -- Needs to Reflect Unit's Weapon Range
    public override int Splash { get; protected set; } = 0;
    public override int Priority { get; protected set; } = 1;
    public override DamageType DamageType { get; protected set; } = DamageType.Physical;
    public sealed override ActionType ActionType { get; protected set; } = ActionType.Attack;
    public override UnitClass ClassType { get; protected set; } = UnitClass.None;
    public override TilePattern AttackTilePattern { get; protected set; } = TilePattern.Linear;
    public override AIActionScore ActionScore { get; protected set; }
    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) {
        return Rangefinder.GetTilesInRange(TileLocator.SelectableTiles[hypoCell.HasValue
                ? new Vector2Int(hypoCell.Value.x, hypoCell.Value.z)
                : unit.UnitInfo.Vector2CellLocation()],
            Range, AttackTilePattern);
    }

    public sealed override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_attack";
    
    public override float CalculateActionScore(AIUnit unit, Vector2Int selectedCell) {
        ActionScore = null;
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");

        foreach (var direction in TilemapUtility.GetDirectionalLinearTilesInRange(
                     TileLocator.SelectableTiles[unit.UnitInfo.Vector2CellLocation()],
                     Range)) {
            foreach (var tile in direction) {
                if (TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out Unit foundUnit)) {
                    if (foundUnit.UnitInfo.IsDead()) { continue; }
                    
                    AIActionScore newScore = new AIActionScore().EvaluateScore(this, unit, tile.TileInfo.CellLocation,
                        foundUnit.UnitInfo.CellLocation, new List<Unit>(), AIUnitScanner.FindNearbyUnits(unit));
            
                    Debug.Log("Heuristic Score at Tile " + tile.TileInfo.CellLocation + ": " + newScore.TotalScore());
                    if (ActionScore == null || newScore.TotalScore() > ActionScore.TotalScore()) ActionScore = newScore;
                    break;
                }
            }
        }

        Debug.Log("Best Heuristic Score: " + (ActionScore == null ? "N/A" : ActionScore.TotalScore()));
        return ActionScore?.TotalScore() ?? -9999;
    }

    public override void ActivateAction(Unit unit) {
        UnitMenu.HideMenu();
        ActionUtility.ShowSelectableTilesForAction(Area(unit, null));
        ChainSystem.HoldPotentialChain(this, unit);
        MapCursor.ActionState();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell) {
        PayAPCost(unit);    // Spend an Action Point to execute the Action
        
        Vector2Int originCell = new Vector2Int(unit.UnitInfo.CellLocation.x, unit.UnitInfo.CellLocation.z);
        Vector2Int displacement = selectedCell - originCell;
        Vector2Int direction = new Vector2Int(Mathf.Clamp(displacement.x, -1, 1), 
            Mathf.Clamp(displacement.y, -1, 1));
        
        int numOfCells = Mathf.Max(Mathf.Abs(displacement.x), Mathf.Abs(displacement.y));

        for (int i = 1; i <= numOfCells; i++) {
            Vector2Int nextCell = originCell + direction * i;
            if (TilemapCreator.UnitLocator.TryGetValue(nextCell, out var targetUnit)) {
                int damage = DamageCalculator.DealDamage(this, unit.UnitInfo, targetUnit.UnitInfo);
                SoundFXManager.PlaySoundFXClip("SwordHit", 0.45f);
                yield return DamageDisplay.DisplayUnitDamage(targetUnit, damage);
                Debug.Log("Attack: unit attacked! HP: " + targetUnit.UnitInfo.currentHP + "/" + targetUnit.UnitInfo.FinalHP);
            }
        }
        
        yield return null;
    }
}