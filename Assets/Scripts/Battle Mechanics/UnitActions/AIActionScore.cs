using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class AIActionScore
{
    public UnitAction Action { get; set; }
    public int AggressionScore { get; set; } = -1;
    public int SurvivalScore { get; set; } = -1;
    public int TacticalPosScore { get; set; } = -1;
    public int AllySynergyScore { get; set; } = -1;
    public int ResourceManagementScore { get; set; } = -1;
    public int ReactionAwarenessScore { get; set; } = -1;
    public int ReactionAllocationScore { get; set; } = -1;
    public Vector3Int PotentialCell { get; private set; }
    public Vector3Int TargetCell { get; set; }
    
    // Aggression Score
    // In-Progress
    // -> Missing <# of Enemies Killed * Aggression - # of Allies Killed * Ally Synergy>
    public int CalcAggressionScore(EnemyUnit unit, Vector3Int potentialCell, Vector3Int targetCell) {
        int score = 0;
        int distance = Pathfinder.DistanceBetweenCells(potentialCell, targetCell);
        Unit unitOnTile;
        
        // Add to score if Potential Cell is within Unit's Striking Range
        int projectedFutureDamage = 0;
        foreach (var action in unit.unitInfo.ActionSet.GetAllAttackActions())
        {
            if (unit.unitInfo.currentAP < action.APCost || unit.unitInfo.currentMP < action.MPCost) { continue; }
            
            unitOnTile = TilemapCreator.UnitLocator[new Vector2Int(targetCell.x, targetCell.z)];
            int futureDamage = 0; 
            
            if (distance <= action.Range && unit.unitInfo.UnitAffiliation != unitOnTile.unitInfo.UnitAffiliation) { 
                futureDamage = DamageCalculator.ProjectDamage(action,
                    unit.unitInfo, unitOnTile.unitInfo) * Mathf.RoundToInt(unit.Aggression); }
            
            if (futureDamage > projectedFutureDamage) { projectedFutureDamage = futureDamage; }
        }
        
        // Add highest potential damage to score
        int projectedDamage = 0;
        
        switch (Action.AttackPattern)
        {
            case Pattern.Direct:
                if (!TilemapCreator.UnitLocator.TryGetValue(new Vector2Int(PotentialCell.x, PotentialCell.z), out unitOnTile)) { break; }
                if (unitOnTile == unit) { break; }
                
                projectedDamage = DamageCalculator.ProjectDamage(Action, unit.unitInfo, unitOnTile.unitInfo);
                
                projectedDamage *= unitOnTile.unitInfo.UnitAffiliation != unit.unitInfo.UnitAffiliation ?
                    1 * Mathf.RoundToInt(unit.Aggression): -1 * Mathf.RoundToInt(unit.AllySynergy);
                
                break;

            case Pattern.Linear:
                foreach (var direction in TilemapUtility.GetDirectionalLinearTilesInRange(
                             TilemapCreator.TileLocator[unit.unitInfo.Vector2CellLocation()],
                             Action.Range))
                {
                    if (direction.Contains(
                            TilemapCreator.TileLocator[new Vector2Int(PotentialCell.x, PotentialCell.z)]))
                    {
                        foreach (var tile in direction)
                        {
                            if (!TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out unitOnTile)) { continue; }
                        
                            int calcDamage = DamageCalculator.ProjectDamage(Action, unit.unitInfo, unitOnTile.unitInfo);
                        
                            projectedDamage += unitOnTile.unitInfo.UnitAffiliation != unit.unitInfo.UnitAffiliation ? 
                                calcDamage * Mathf.RoundToInt(unit.Aggression) : -calcDamage * Mathf.RoundToInt(unit.AllySynergy);
                        }

                        break;
                    }
                }
                
                break;

            case Pattern.Splash:
                foreach (var splashTile in TilemapUtility.GetSplashTilesInRange(
                             TilemapCreator.TileLocator[new Vector2Int(PotentialCell.x, PotentialCell.z)],
                             Action.Range))
                {
                    if (!TilemapCreator.UnitLocator.TryGetValue(splashTile.TileInfo.Vector2CellLocation(), out unitOnTile)) { break; }
                    
                    int calcDamage = DamageCalculator.ProjectDamage(Action, unit.unitInfo, unitOnTile.unitInfo);
                    
                    projectedDamage = unitOnTile.unitInfo.UnitAffiliation != unit.unitInfo.UnitAffiliation ? 
                        calcDamage * Mathf.RoundToInt(unit.Aggression) : -calcDamage * Mathf.RoundToInt(unit.AllySynergy);
                }

                break;
            
            case Pattern.None:
                break;

            default:
                Debug.LogError("ERROR: UnitAction requires 'AttackPattern'. (CalcAggressionScore)");
                break;
        }

        score += projectedDamage + projectedFutureDamage;
        Debug.Log("Unit " + potentialCell + " Aggression Score: " + score);
        return score;
    }

    // Tactical Awareness Score
    // Completed!!
    // -> Possible Improvement <Calculate and Account for Y-Distance (Vertical)>
    public int CalcSurvivalScore(EnemyUnit unit, Vector3Int potentialCell, Vector3Int targetCell) {
        int score = 0;
        int distance = Pathfinder.DistanceBetweenCells(potentialCell, targetCell);
        
        score += distance * Mathf.RoundToInt(unit.TacticalPositioning);
        Debug.Log("Unit " + potentialCell + " Distance Score: " + score);
        return score;
    }
    
    // Tactical Awareness Score
    // Completed!!
    // -> Possible Improvement <Calculate and Account for Y-Distance (Vertical)>
    public int CalcTacticalPositioningScore(EnemyUnit unit, Vector3Int potentialCell, Vector3Int targetCell) {
        int score = 0;
        int distance = Pathfinder.DistanceBetweenCells(potentialCell, targetCell);
        
        score += distance * Mathf.RoundToInt(unit.TacticalPositioning);
        Debug.Log("Unit " + potentialCell + " Distance Score: " + score);
        return score;
    }
    
    // Ally Synergy Score
    // Completed!!
    public int CalcAllySynergyScore(EnemyUnit unit, Vector3Int potentialCell, List<Unit> nearbyUnits)
    {
        int synergy = 0;
        
        // 
        foreach (var nearbyUnit in nearbyUnits) {
            int distance = Pathfinder.DistanceBetweenCells(potentialCell, nearbyUnit.unitInfo.CellLocation);
            if (nearbyUnit.unitInfo.UnitAffiliation == unit.unitInfo.UnitAffiliation || distance > nearbyUnit.unitInfo.finalSense) continue;
            synergy += 10 * Mathf.RoundToInt(unit.AllySynergy);
        }

        // Return Synergy only if the Action does not heal
        if (Action.DamageType != DamageType.Healing) { return synergy; }
        
        // Add highest potential recovery to score
        int projectedHealing = 0;
        Unit unitOnTile;
        
        switch (Action.AttackPattern)
        {
            case Pattern.Direct:
                if (!TilemapCreator.UnitLocator.TryGetValue(new Vector2Int(PotentialCell.x, PotentialCell.z), out unitOnTile)) { break; }
                
                projectedHealing = DamageCalculator.ProjectHealing(Action, unit.unitInfo, unitOnTile.unitInfo);
                
                projectedHealing *= unitOnTile.unitInfo.UnitAffiliation == unit.unitInfo.UnitAffiliation ?
                    1 * Mathf.RoundToInt(unit.AllySynergy): -1 * Mathf.RoundToInt(unit.Aggression);
                
                break;

            case Pattern.Linear:
                foreach (var direction in TilemapUtility.GetDirectionalLinearTilesInRange(
                             TilemapCreator.TileLocator[new Vector2Int(PotentialCell.x, PotentialCell.z)],
                             Action.Range))
                {
                    if (direction.Contains(
                            TilemapCreator.TileLocator[new Vector2Int(PotentialCell.x, PotentialCell.z)]))
                    {
                        foreach (var tile in direction)
                        {
                            if (!TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out unitOnTile)) { break; }
                        
                            int calcDamage = DamageCalculator.ProjectHealing(Action, unit.unitInfo, unitOnTile.unitInfo);
                        
                            projectedHealing += unitOnTile.unitInfo.UnitAffiliation == unit.unitInfo.UnitAffiliation ? 
                                calcDamage * Mathf.RoundToInt(unit.AllySynergy) : -calcDamage * Mathf.RoundToInt(unit.Aggression);
                        }

                        break;
                    }
                }
                
                break;

            case Pattern.Splash:
                foreach (var splashTile in TilemapUtility.GetSplashTilesInRange(
                             TilemapCreator.TileLocator[new Vector2Int(PotentialCell.x, PotentialCell.z)],
                             Action.Range))
                {
                    if (!TilemapCreator.UnitLocator.TryGetValue(splashTile.TileInfo.Vector2CellLocation(), out unitOnTile)) { break; }
                    
                    int calcDamage = DamageCalculator.ProjectHealing(Action, unit.unitInfo, unitOnTile.unitInfo);
                        
                    projectedHealing += unitOnTile.unitInfo.UnitAffiliation == unit.unitInfo.UnitAffiliation ? 
                        calcDamage * Mathf.RoundToInt(unit.AllySynergy) : -calcDamage * Mathf.RoundToInt(unit.Aggression);
                }

                break;

            case Pattern.None:
                break;
            
            default:
                Debug.LogError("ERROR: UnitAction requires 'AttackPattern'. (CalcAggressionScore)");
                break;
        }

        synergy += projectedHealing;
        Debug.Log("Unit " + potentialCell + " Ally Synergy Score: " + synergy);
        return synergy;
    }
    
    // Resource Management Score
    // Completed!!
    public int CalcResourceManagementScore(EnemyUnit unit) {
        return -(unit.unitInfo.currentMP - Action.MPCost) / unit.unitInfo.finalMP * Mathf.RoundToInt(unit.ResourceManagement);
    }

    // Reaction Awareness Score
    // Completed!!
    public int CalcReactionAwarenessScore(EnemyUnit unit, Vector3Int potentialCell, List<Unit> enemyUnits) {
        int score = 0;
        int threatLevel = 0;
        
        foreach (var enemy in enemyUnits) {
            int distance = Pathfinder.DistanceBetweenCells(potentialCell, enemy.unitInfo.CellLocation);
            if (distance <= enemy.unitInfo.finalSense) {
                score -= 10;
                threatLevel -= enemy.unitInfo.currentLevel;
            }
        }

        score += threatLevel;
        score *= Mathf.RoundToInt(unit.ReactionAwareness);
        Debug.Log("Unit " + potentialCell + " Reaction Awareness Score: " + score);
        return score;
    }
    
    // Reaction Allocation Score
    // Completed!!
    public int CalcReactionAllocationScore(EnemyUnit unit) {
        return 20 * (unit.unitInfo.currentAP - Action.APCost) * Mathf.RoundToInt(unit.ReactionAllocation);
    }

    public AIActionScore EvaluateScore(UnitAction action, EnemyUnit unit, Vector3Int potentialCell, Vector3Int targetCell, List<Unit> allyUnits, List<Unit> enemyUnits)
    {
        Action = action;
        PotentialCell = potentialCell;
        TargetCell = targetCell;
        
        AggressionScore = CalcAggressionScore(unit, potentialCell, targetCell);
        SurvivalScore = 0;
        TacticalPosScore = CalcTacticalPositioningScore(unit, potentialCell, targetCell);
        AllySynergyScore = CalcAllySynergyScore(unit, potentialCell, allyUnits);
        ResourceManagementScore = CalcResourceManagementScore(unit);
        ReactionAwarenessScore = CalcReactionAwarenessScore(unit, potentialCell, enemyUnits);
        ReactionAllocationScore = CalcReactionAllocationScore(unit);
        
        return this;
    }

    public int TotalScore() => AggressionScore + SurvivalScore + TacticalPosScore + AllySynergyScore + ResourceManagementScore + ReactionAwarenessScore + ReactionAllocationScore;
    public Vector2Int Vector2PotentialLocation() { return new Vector2Int(PotentialCell.x, PotentialCell.z); }
}
