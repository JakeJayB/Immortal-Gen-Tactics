using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAITargeting : MonoBehaviour
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
    public Unit TargetUnit { get; set; }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // Aggression Score
    // Scores higher the closer an enemy unit is towards death.
    private int CalcAggressionScore(EnemyUnit unitAI, Unit potentialUnit) {
        return Mathf.RoundToInt((float)(9999 - potentialUnit.unitInfo.currentHP) / 9999 * unitAI.Aggression);
    }

    // Tactical Awareness Score
    // Completed!!
    // -> Possible Improvement <Calculate and Account for Y-Distance (Vertical)>
    public int CalcSurvivalScore(EnemyUnit unit, Vector3Int potentialCell, Vector3Int targetCell) {
        int score = 0;
        int distance = Pathfinder.DistanceBetweenCells(potentialCell, targetCell);
        
        score += distance * Mathf.RoundToInt(unit.TacticalPositioning);
        //Debug.Log("Unit " + potentialCell + " Distance Score: " + score);
        return score;
    }
    
    // Tactical Awareness Score
    // Completed!!
    // -> Possible Improvement <Calculate and Account for Y-Distance (Vertical)>
    public int CalcTacticalPositioningScore(EnemyUnit unit, Vector3Int potentialCell, Vector3Int targetCell) {
        int score = 0;
        int currentDistance = Pathfinder.DistanceBetweenCells(unit.unitInfo.CellLocation, targetCell);
        int distance = Pathfinder.DistanceBetweenCells(potentialCell, targetCell);

        if (TilemapCreator.UnitLocator.TryGetValue(new Vector2Int(targetCell.x, targetCell.z), out var unitOnTile))
        {
            // Add to score if Potential Cell is within Unit's Striking Range
            int projectedFutureDamage = 0;
            foreach (var action in unit.unitInfo.ActionSet.GetAllAttackActions())
            {
                if (action.Range < distance) { continue; }
                if (unit.unitInfo.currentAP < action.APCost || unit.unitInfo.currentMP < action.MPCost) { continue; }
            
                unitOnTile = TilemapCreator.UnitLocator[new Vector2Int(targetCell.x, targetCell.z)];
                int futureDamage = 0; 
            
                if (distance <= action.Range && unit.unitInfo.UnitAffiliation != unitOnTile.unitInfo.UnitAffiliation) { 
                    futureDamage = DamageCalculator.ProjectDamage(action,
                        unit.unitInfo, unitOnTile.unitInfo) * Mathf.RoundToInt(unit.Aggression); }
            
                if (futureDamage > projectedFutureDamage) { projectedFutureDamage = futureDamage; }
            }

            score += projectedFutureDamage;
        }
        
        score += (currentDistance - distance) * Mathf.RoundToInt(unit.TacticalPositioning);
        //Debug.Log("Unit " + potentialCell + " Distance Score: " + score);
        return score;
    }
    
    // Ally Synergy Score
    // TODO: Check health to see how close they are to death (Debate hp ratio vs. remaining hp)
    // TODO: Check if unitAI is able to provide buffs to unit.
    // TODO: Find an easy way to determine if the unit is able to provide support to the ally.
            // Can the AI 'reach' the unit?
            // Can the AI 'use' the action to support the unit?
    public int CalcAllySynergyScore(EnemyUnit unitAI, Unit potentialUnit) {
        return Mathf.RoundToInt((float)(9999 - potentialUnit.unitInfo.currentHP) / 9999 * unitAI.AllySynergy);
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
        //Debug.Log("Unit " + potentialCell + " Reaction Awareness Score: " + score);
        return score;
    }
    
    // Reaction Allocation Score
    // Completed!!
    public int CalcReactionAllocationScore(EnemyUnit unit) {
        return 20 * (unit.unitInfo.currentAP - Action.APCost) * Mathf.RoundToInt(unit.ReactionAllocation);
    }

    public UnitAITargeting EvaluateScore(EnemyUnit unitAI)
    {
        foreach (var potentialUnit in TilemapCreator.UnitLocator.Values)
        {
            if (unitAI.unitInfo.UnitAffiliation == potentialUnit.unitInfo.UnitAffiliation) {
                AggressionScore = CalcAggressionScore(unitAI, potentialUnit);
            } else {
                AllySynergyScore = CalcAllySynergyScore(unitAI, potentialUnit);
            }
            //SurvivalScore = 0;
            //TacticalPosScore = CalcTacticalPositioningScore(unit, potentialCell, targetCell);
            //AllySynergyScore = CalcAllySynergyScore(unit, potentialCell, enemyUnits);
            //ResourceManagementScore = 0;
            //ReactionAwarenessScore = CalcReactionAwarenessScore(unit, potentialCell, enemyUnits);
            //ReactionAllocationScore = 0;
        }
        
        
        
        return this;
    }

    public int TotalScore() => AggressionScore + SurvivalScore + TacticalPosScore + AllySynergyScore + ResourceManagementScore + ReactionAwarenessScore + ReactionAllocationScore;
}
