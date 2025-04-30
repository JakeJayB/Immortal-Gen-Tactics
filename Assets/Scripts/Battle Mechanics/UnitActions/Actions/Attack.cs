using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Attack : UnitAction
{
    // Start is called before the first frame update
    public sealed override string Name { get; protected set; } = "Attack";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 1;
    public override int Priority { get; protected set; } = 1;
    public override DamageType DamageType { get; protected set; } = DamageType.Physical;
    public override int BasePower { get; protected set; } = 0;
    public sealed override ActionType ActionType { get; protected set; } = ActionType.Attack;
    public override Pattern AttackPattern { get; protected set; } = Pattern.Linear;
    public override int Range { get; protected set; } = 1; // FIX!! -- Needs to Reflect Unit's Weapon Range
    public override AIActionScore ActionScore { get; protected set; }
    public override int Splash { get; protected set; } = 0;
    public override List<Tile> Area(Unit unit)
    {
        return Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[unit.unitInfo.Vector2CellLocation()], Range,
            AttackPattern);
    }

    public sealed override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_attack";

    public sealed override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    public override float CalculateActionScore(EnemyUnit unit, Vector2Int selectedCell)
    {
        ActionScore = new AIActionScore();
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");
        Debug.Log("Initial Heuristic Score: " + ActionScore.TotalScore());

        foreach (var direction in TilemapUtility.GetDirectionalLinearTilesInRange(
                     TilemapCreator.TileLocator[unit.unitInfo.Vector2CellLocation()],
                     Range))
        {
            foreach (var tile in direction)
            {
                if (TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out Unit foundUnit))
                {
                    if (foundUnit.unitInfo.IsDead()) { continue; }
                    
                    AIActionScore newScore = new AIActionScore().EvaluateScore(this, unit, tile.TileInfo.CellLocation,
                        foundUnit.unitInfo.CellLocation, new List<Unit>(), unit.FindNearbyUnits());
            
                    Debug.Log("Heuristic Score at Tile " + tile.TileInfo.CellLocation + ": " + newScore.TotalScore());
                    if (newScore.TotalScore() > ActionScore.TotalScore()) ActionScore = newScore;

                    break;
                }
            }
        }

        Debug.Log("Best Heuristic Score: " + ActionScore.TotalScore());
        Debug.Log("Decided Cell Location: " + ActionScore.PotentialCell);
        return ActionScore.TotalScore();
    }

    public override void ActivateAction(Unit unit)
    {
        UnitMenu.HideMenu();
        ActionUtility.ShowSelectableTilesForAction(Area(unit));
        ChainSystem.HoldPotentialChain(this, unit);
        MapCursor.ActionState();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        // Spend an Action Point to execute the Action
        PayAPCost(unit);
        
        Vector2Int originCell = new Vector2Int(unit.unitInfo.CellLocation.x, unit.unitInfo.CellLocation.z);
        Vector2Int displacement = selectedCell - originCell;

        Vector2Int direction = new Vector2Int(Mathf.Clamp(displacement.x, -1, 1), Mathf.Clamp(displacement.y, -1, 1));
        int numOfCells = Mathf.Max(Mathf.Abs(displacement.x), Mathf.Abs(displacement.y));

        for (int i = 1; i <= numOfCells; i++)
        {
            Vector2Int nextCell = originCell + direction * i;
            if (TilemapCreator.UnitLocator.TryGetValue(nextCell, out var targetUnit))
            {
                int damage = DamageCalculator.DealDamage(this, unit.unitInfo, targetUnit.unitInfo);
                SoundFXManager.PlaySoundFXClip("SwordHit", 0.45f);
                yield return DamageDisplay.DisplayUnitDamage(targetUnit.unitInfo, damage);
                Debug.Log("Attack: unit attacked! HP: " + targetUnit.unitInfo.currentHP + "/" + targetUnit.unitInfo.finalHP);
            }
        }
        
        yield return null;
    }
}