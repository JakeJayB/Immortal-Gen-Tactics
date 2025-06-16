using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class AIActionScore
{
    public UnitAction Action { get; set; }
    public int DamageScore { get; set; } = -1;
    public int PositionScore { get; set; } = -1;
    public int ForesightScore { get; set; } = -1;
    public Vector3Int PotentialCell { get; private set; }
    public Vector3Int TargetCell { get; set; }
    
    // CalcDamageScore
    // [+] Scores Damage/Healing Done To Ally & Enemy Units
    // [+] Scores Potential Death
    // [+] Scores Potential Revival
    public int CalcDamageScore(EnemyUnit unitAI)
    {
        int damageScore = 0;        // Holds the calculated damage score
        int projectedDamage = 0;    // Amount of damage projected to happen to the unit on tile
        Unit unitOnTile;            // Holds the unit found on an observed tile
        
        switch (Action.AttackPattern)
        {
            case Pattern.Direct:
                // Skip checking this tile if no unit exists on it...
                if (!TilemapCreator.UnitLocator.TryGetValue(new Vector2Int(PotentialCell.x, PotentialCell.z), out unitOnTile)) { break; }
                
                damageScore += CalcDamageToUnit(unitAI, unitOnTile);
                break;

            case Pattern.Linear:
                foreach (var direction in TilemapUtility.GetDirectionalLinearTilesInRange(
                             TilemapCreator.TileLocator[unitAI.unitInfo.Vector2CellLocation()],
                             Action.Range))
                {
                    // Only check the direction that contains the potential cell currently being scored
                    if (direction.Contains(
                            TilemapCreator.TileLocator[new Vector2Int(PotentialCell.x, PotentialCell.z)]))
                    {
                        foreach (var tile in direction)
                        {
                            // Skip to the next tile if no unit exists on this one...
                            if (!TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out unitOnTile)) { break; }
                            
                            damageScore += CalcDamageToUnit(unitAI, unitOnTile);
                        }

                        break;
                    }
                }
                
                break;

            case Pattern.Splash:
                foreach (var splashTile in TilemapUtility.GetSplashTilesInRange(
                             TilemapCreator.TileLocator[new Vector2Int(PotentialCell.x, PotentialCell.z)],
                             Action.Splash))
                {
                    // Continue to the next tile if no unit exists on this current one
                    if (!TilemapCreator.UnitLocator.TryGetValue(splashTile.TileInfo.Vector2CellLocation(), out unitOnTile)) { continue; }
                    
                    damageScore += CalcDamageToUnit(unitAI, unitOnTile);
                }

                break;
            
            case Pattern.None:
                break;

            default:
                Debug.LogError("ERROR: UnitAction requires 'AttackPattern'. (CalcAggressionScore)");
                break;
        }
        
        return damageScore;
    }

    // CalcPositionScore
    // [+] Scores Nearby Units Who Sense the AI Unit
    // [+] Scores Proximity Between AI Unit and Target
    public int CalcPositionScore(EnemyUnit unitAI, List<Unit> nearbyUnits)
    {
        int positionScore = 0;      // Holds the calculated damage score
        int distance = 0;
        
        // Score higher for DeadZoning a Target
        // TODO: Balance the score calculation.
        if (Pathfinder.DistanceBetweenCells(PotentialCell, TargetCell) < Action.Splash) 
        { positionScore += Mathf.RoundToInt(DamageScore * 0.2f * unitAI.TacticalPositioning); }
        
        foreach (var nearbyUnit in nearbyUnits) {
            // If the action is move-based, get the distance between the AI unit's potential position and the nearby unit
            // Otherwise, get the distance between the AI unit's current position and nearby unit
            distance = Action.ActionType == ActionType.Move 
                ? Pathfinder.DistanceBetweenCells(PotentialCell, nearbyUnit.unitInfo.CellLocation)
                : Pathfinder.DistanceBetweenUnits(unitAI, nearbyUnit);
            
            // Add to score if within reaction distance of an ally unit
            // Subtract from score if within reaction distance of an enemy unit
            // TODO: Account for enemy reactions that can reach them.
            // TODO: Calculate the potential damage that could reach them (hindsight).
            // TODO: Create a hindsight function that calculates the danger of discovered reactions from enemy units.
            if (nearbyUnit.unitInfo.currentAP < 1 || distance > nearbyUnit.unitInfo.finalSense) continue;
            positionScore += nearbyUnit.unitInfo.UnitAffiliation == unitAI.unitInfo.UnitAffiliation
                ? nearbyUnit.unitInfo.currentLevel * (int)unitAI.ReactionAwareness
                : -nearbyUnit.unitInfo.currentLevel * (int)unitAI.ReactionAwareness;
        }

        if (TilemapCreator.UnitLocator.TryGetValue(new Vector2Int(TargetCell.x, TargetCell.z), out var unitOnTile)) {
            positionScore += Mathf.RoundToInt(
                (Pathfinder.DistanceBetweenUnits(unitAI, unitOnTile)
                - Pathfinder.DistanceBetweenCells(PotentialCell, TargetCell)) * unitAI.TacticalPositioning);
        }
        
        return positionScore;
    }

    // TODO: Calculate the next best action after this current one to contribute its value to the score.
    public int CalcForesightScore(EnemyUnit unitAI)
    {
        // If the AI Unit is waiting, do not check for foresight
        if (Action.ActionType == ActionType.Wait) { return 0; }

        // If the current action will deplete all remaining AP, do not check for foresight
        if (unitAI.unitInfo.currentAP - Action.APCost == 0) { return 0; }
        
        int foresightScore = 0;
        int projectedDamage = 0;    // Amount of damage projected to happen to the unit on tile

        if (TilemapCreator.UnitLocator.TryGetValue(new Vector2Int(TargetCell.x, TargetCell.z), out var unitOnTile))
        {
            // Add to score if Potential Cell is within Unit's Striking Range
            int projectedFutureDamage = 0;
            
            // If the action is move-based, get the distance between the AI unit's potential position and the nearby unit
            // Otherwise, get the distance between the AI unit's current position and nearby unit
            int distance = Action.ActionType == ActionType.Move 
                ? Pathfinder.DistanceBetweenCells(PotentialCell, unitOnTile.unitInfo.CellLocation)
                : Pathfinder.DistanceBetweenUnits(unitAI, unitOnTile);
            
            foreach (var action in unitAI.unitInfo.ActionSet.GetAITurnActions())
            {
                if (action.Range + action.Splash < distance) { continue; }
                if (unitAI.unitInfo.currentAP - Action.APCost < action.APCost || unitAI.unitInfo.currentMP - Action.MPCost < action.MPCost) { continue; }
                
                int futureDamage = 0; 
            
                if (distance <= action.Range + action.Splash && unitAI.unitInfo.UnitAffiliation != unitOnTile.unitInfo.UnitAffiliation) { 
                    projectedDamage = DamageCalculator.ProjectDamage(action,
                        unitAI.unitInfo, unitOnTile.unitInfo);
                    futureDamage += unitOnTile.unitInfo.currentHP - projectedDamage < 1 ?
                        unitOnTile.unitInfo.finalHP * (int)unitAI.Aggression :  // If the action kills a unit, score higher
                        projectedDamage * (int)unitAI.Aggression;   // Else, score the original damaging amount
                }
                else if (distance <= action.Range + action.Splash && unitAI.unitInfo.UnitAffiliation == unitOnTile.unitInfo.UnitAffiliation) {
                    projectedDamage = DamageCalculator.ProjectHealing(action,
                        unitAI.unitInfo, unitOnTile.unitInfo);
                    futureDamage += unitOnTile.unitInfo.IsDead() && Action.DamageType == DamageType.Revival ? 
                        unitOnTile.unitInfo.finalHP * (int)unitAI.AllySynergy : // If the action revives a unit, score higher
                        projectedDamage * (int)unitAI.AllySynergy;  // Else, score the original healing amount
                    
                }
            
                // The closer the AI unit is to its target, the higher the score
                // TODO: Figure out how to make the optimal distance formula
                if (action.Range != 0) {
                    futureDamage += distance * distance / action.Range * (int)unitAI.TacticalPositioning;
                }
                
                if (futureDamage > projectedFutureDamage) { projectedFutureDamage = futureDamage; }
            }
            
            foresightScore += projectedFutureDamage;
        }
        
        return foresightScore;
    }

    public int CalcResourceScore(EnemyUnit unitAI)
    {
        int resourceScore = 0;

        resourceScore += unitAI.unitInfo.currentMP - Action.MPCost * (int)unitAI.ResourceManagement;
        resourceScore += unitAI.unitInfo.currentAP - Action.APCost * (int)unitAI.ResourceManagement;
        
        if (Action.ActionType == ActionType.Wait) {
            resourceScore += (unitAI.unitInfo.currentAP - Action.APCost) * (int)Mathf.Pow(unitAI.ReactionAllocation, 1); 
        }

        return resourceScore;
    }

    // -------------------------------------------------------------------------------------------------------------------

    private int CalcDamageToUnit(EnemyUnit unitAI, Unit unitOnTile)
    {
        int projectedDamage = 0;    // Projected Damage or Healing Inflicted
        int projectedScore = 0;     // Total Value Projected to Come From Projected Damage
        
        if (unitOnTile.unitInfo.UnitAffiliation != unitAI.unitInfo.UnitAffiliation) { // Calc Damage Towards Enemies
            projectedDamage =
                DamageCalculator.ProjectDamage(Action, unitAI.unitInfo, unitOnTile.unitInfo);

            projectedScore += unitOnTile.unitInfo.currentHP - projectedDamage < 1 ?
                unitOnTile.unitInfo.finalHP * (int)unitAI.Aggression :  // If the action kills a unit, score higher
                projectedDamage * (int)unitAI.Aggression;   // Else, score the original damaging amount
                                
        } else { // Calc Healing Towards Allies
            projectedDamage =
                DamageCalculator.ProjectHealing(Action, unitAI.unitInfo, unitOnTile.unitInfo);
                                
            projectedScore += unitOnTile.unitInfo.IsDead() && Action.DamageType == DamageType.Revival ? 
                unitOnTile.unitInfo.finalHP * (int)unitAI.AllySynergy : // If the action revives a unit, score higher
                projectedDamage * (int)unitAI.AllySynergy;  // Else, score the original healing amount
        }

        return projectedScore;
    }
    
    // -------------------------------------------------------------------------------------------------------------------

    public AIActionScore EvaluateScore(UnitAction action, EnemyUnit unit, Vector3Int potentialCell,
        Vector3Int targetCell, List<Unit> allyUnits, List<Unit> enemyUnits)
    {
        Action = action;
        PotentialCell = potentialCell;
        TargetCell = targetCell;

        var targetUnit = TilemapCreator.UnitLocator[new Vector2Int(targetCell.x, targetCell.z)];

        DamageScore = CalcDamageScore(unit);
        PositionScore = CalcPositionScore(unit, enemyUnits);
        ForesightScore = CalcForesightScore(unit);
        
        return this;
    }

    public int TotalScore() => DamageScore + PositionScore + ForesightScore;
    public Vector2Int Vector2PotentialLocation() { return new Vector2Int(PotentialCell.x, PotentialCell.z); }
}
