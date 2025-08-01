using System.Collections;
using System.Collections.Generic;
using IGT.Core;
using IGT.Systems;
using UnityEngine;

public class Rush : UnitAction
{
    public override string Name { get; protected set; } = "Rush";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 1;
    public override int BasePower { get; protected set; } = 0;
    public override int Range { get; protected set; } = 0;
    public override int Splash { get; protected set; } = 0;
    public override int Priority { get; protected set; } = 1;
    public override DamageType DamageType { get; protected set; } = DamageType.Physical;
    public override ActionType ActionType { get; protected set; } = ActionType.Attack;
    public override UnitClass ClassType { get; protected set; } = UnitClass.Soldier;
    public override TilePattern AttackTilePattern { get; protected set; } = TilePattern.Rush;
    public override AIActionScore ActionScore { get; protected set; }
    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) {
        Range = unit.UnitInfo.FinalMove;    // Update Range Based On Unit's Movement
        
        return Rangefinder.GetTilesInRange(TileLocator.SelectableTiles[hypoCell.HasValue
                ? new Vector2Int(hypoCell.Value.x, hypoCell.Value.z)
                : unit.UnitInfo.Vector2CellLocation()],
            Range, AttackTilePattern);
    }
    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_attack";

    public override float CalculateActionScore(AIUnit unit, Vector2Int selectedCell) {
        ActionScore = null;
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");

        Area(unit, null);
        foreach (var direction in TilemapUtility.GetDirectionalLinearTilesInRange(
                     TileLocator.SelectableTiles[unit.UnitInfo.Vector2CellLocation()],
                     Range)) {
            foreach (var tile in direction) {
                AIActionScore newScore = new AIActionScore().EvaluateScore(this, unit, tile.TileInfo.CellLocation,
                    TilemapCreator.UnitLocator[selectedCell].UnitInfo.CellLocation, new List<Unit>(), AIUnitScanner.FindNearbyUnits(unit));
            
                Debug.Log("Heuristic Score at Tile " + tile.TileInfo.CellLocation + ": " + newScore.TotalScore());
                if (ActionScore == null || newScore.TotalScore() > ActionScore.TotalScore()) ActionScore = newScore;
                break;
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
        
        Vector2Int originCell = unit.UnitInfo.Vector2CellLocation();
        Vector2Int displacement = selectedCell - originCell;
        Vector2Int direction = new Vector2Int(Mathf.Clamp(displacement.x, -1, 1), Mathf.Clamp(displacement.y, -1, 1));
        Vector2Int previousCell = originCell;

        for (int i = 1; i <= Range; i++) {
            Vector2Int nextCell = originCell + direction * i;

            // Stop the unit from trying to traverse null tile locations
            if (!TileLocator.SelectableTiles.TryGetValue(nextCell, out var tile)) {
                TilemapCreator.UnitLocator.Remove(originCell);
                yield return UnitMovement.Move(unit, previousCell);
                TilemapCreator.UnitLocator.Add(previousCell, unit);
                yield break;
            }
            
            if (TilemapCreator.UnitLocator.TryGetValue(nextCell, out var targetUnit)) {
                // Remove the Location the Unit is currently at in UnitLocator
                TilemapCreator.UnitLocator.Remove(originCell);
                
                // Updates the location as the Unit moves
                yield return UnitMovement.Move(unit, previousCell);
                
                int damage = DamageCalculator.DealDamage(this, unit.UnitInfo, targetUnit.UnitInfo);
                SoundFXManager.PlaySoundFXClip("SwordHit", 0.45f);
                yield return DamageDisplay.DisplayUnitDamage(targetUnit, damage);
                Debug.Log("Attack: unit attacked! HP: " + targetUnit.UnitInfo.currentHP + "/" + targetUnit.UnitInfo.FinalHP);
                int blowbackStrength = (Range - i) / 2 < 1 ? 1 : (Range - i) / 2;
                yield return new BlowbackBFX(blowbackStrength, direction).Inflict(targetUnit);
                
                var endLocation = TilemapCreator.UnitLocator.TryGetValue(nextCell, out var stillThere)
                    ? previousCell : nextCell;
                
                if (unit.UnitInfo.Vector2CellLocation() != endLocation) { yield return UnitMovement.Move(unit, endLocation); }
                
                // Adds the location of the tile the Unit ended at in UnitLocator
                TilemapCreator.UnitLocator.Add(endLocation, unit);
                break;
            }

            previousCell = nextCell;

            if (i != Range) continue;
            
            // Remove the Location the Unit is currently at in UnitLocator
            TilemapCreator.UnitLocator.Remove(originCell);
        
            // Updates the location as the Unit moves
            yield return UnitMovement.Move(unit, previousCell);
        
            // Adds the location of the tile the Unit ended at in UnitLocator
            TilemapCreator.UnitLocator.Add(previousCell, unit);
        }
        
        yield return null;
    }
}
