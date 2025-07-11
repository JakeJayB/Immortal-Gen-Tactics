using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitAITargeting
{
    public UnitAction Action { get; set; }
    public int AggressionScore { get; set; } = -1;
    public int SurvivalScore { get; set; } = -1;
    public int TacticalPosScore { get; set; } = -1;
    public int AllySynergyScore { get; set; } = -1;
    public int ResourceManagementScore { get; set; } = -1;
    public int ReactionAwarenessScore { get; set; } = -1;
    public int ReactionAllocationScore { get; set; } = -1;
    public Unit TargetUnit { get; set; }

    private int CalcPriorityScore(AIUnit unitAI, Unit potentialUnit)
    {
        // Skip the calculation process if enemy unit is already dead
        if (potentialUnit.UnitInfo.IsDead() &&
            potentialUnit.UnitInfo.UnitAffiliation != unitAI.UnitInfo.UnitAffiliation) { return -1; }
        
        int score = 0;
        int distance = Pathfinder.DistanceBetweenUnits(unitAI, potentialUnit);
        
        
        // TODO: This factor needs to be apart of the Score Functions so that they're only applied if they have an action that can impact the unit.
        // Score Factor 1: HP Ratio
        // The Lower the Health, the Higher the Score
        //score += Mathf.RoundToInt(potentialUnit.unitInfo.FinalHP / (float)potentialUnit.unitInfo.currentHP * 
                                  //(potentialUnit.unitInfo.UnitAffiliation == UnitAffiliation.Enemy ? unitAI.Aggression : unitAI.AllySynergy));

        // Score Factor 2: Distance Away
        if (unitAI != potentialUnit) {
            // score += Mathf.RoundToInt(unitAI.unitInfo.finalMove * unitAI.unitInfo.currentAP / (float)distance);
            score += unitAI.UnitInfo.FinalMove * (ChainSystem.ReactionInProgress ? 1 : unitAI.UnitInfo.currentAP) - distance;
        }
        
        // Score Factor 3: Action Impact
        score += potentialUnit.UnitInfo.UnitAffiliation != unitAI.UnitInfo.UnitAffiliation
            ? CalcAggressionScore(unitAI, potentialUnit)
            : CalcAllySynergyScore(unitAI, potentialUnit);
        
        // Score Factor 2: Unit Impact
        //score *= Mathf.RoundToInt(potentialUnit.unitInfo.currentLevel);
        
        return score;
    }
    
    // Aggression Score
    // Scores higher the closer an enemy unit is towards death.
    private int CalcAggressionScore(AIUnit unitAI, Unit potentialUnit) {
        int score = 0;
        int distance = Pathfinder.DistanceBetweenUnits(unitAI, potentialUnit);
        
        // Score Factor 2: Damage Impact
        // If an action would knock out the targeted unit, add a large amount to the score...
        // ...otherwise, the more damage projected, the higher the score
        // Deduct MP Cost and AP Cost needed for each specific action
        int projectedFutureDamage = 0;
        foreach (var action in unitAI.ActionSet.GetAllAttackActions())
        {
            // Skip Actions that Heal an Enemy
            // Skip Actions that can't be Currently Used by the UnitAI
            // Skip Reactions that aren't in Range
            if (action.DamageType == DamageType.Healing) { continue; }
            if (unitAI.UnitInfo.currentMP < action.MPCost) { continue; }
            if (ChainSystem.ReactionInProgress && action.Range + action.Splash < distance) { continue; }
            
            // Instantiate the damage value the current action will have in the future
            int futureDamage = 0;
            
            // Max Possible Distance UnitAI can be from the Potential Unit to Perform the Action on their Current Turn
            int maxPossibleDistance =
                action.Range + unitAI.UnitInfo.FinalMove * (unitAI.UnitInfo.currentAP - action.APCost);
            
            // Additional AP Needed to Move Towards Potential Unit to Perform Action
            int additionalAPCost = (distance - action.Range) / unitAI.UnitInfo.FinalMove;
            
            if (unitAI.UnitInfo.UnitAffiliation != potentialUnit.UnitInfo.UnitAffiliation) { 
                futureDamage = DamageCalculator.ProjectDamage(action,
                    unitAI.UnitInfo, potentialUnit.UnitInfo); }

            if (potentialUnit.UnitInfo.currentHP - futureDamage <= 0) { futureDamage = Mathf.RoundToInt(potentialUnit.UnitInfo.FinalHP); }
            futureDamage -= Mathf.RoundToInt(action.MPCost * unitAI.ResourceManagement);
            futureDamage -= Mathf.RoundToInt((action.APCost + additionalAPCost) * unitAI.ReactionAllocation);
            futureDamage *= (int)unitAI.Aggression;
            if (futureDamage > projectedFutureDamage) { projectedFutureDamage = futureDamage; }
        }

        score += projectedFutureDamage;
        
        if (projectedFutureDamage > 0) {
            score += Mathf.RoundToInt(potentialUnit.UnitInfo.FinalHP /
            (float)potentialUnit.UnitInfo.currentHP * (int)unitAI.Aggression); 
        }
        
        return score;
    }

    // Tactical Awareness Score
    // Completed!!
    // -> Possible Improvement <Calculate and Account for Y-Distance (Vertical)>
    public int CalcSurvivalScore(AIUnit unit, Vector3Int potentialCell, Vector3Int targetCell) {
        int score = 0;
        int distance = Pathfinder.DistanceBetweenCells(potentialCell, targetCell);
        
        score += distance * Mathf.RoundToInt(unit.TacticalPositioning);
        //Debug.Log("Unit " + potentialCell + " Distance Score: " + score);
        return score;
    }
    
    // Tactical Awareness Score
    // Completed!!
    // -> Possible Improvement <Calculate and Account for Y-Distance (Vertical)>
    public int CalcTacticalPositioningScore(AIUnit unit, Vector3Int potentialCell, Vector3Int targetCell) {
        int score = 0;
        int currentDistance = Pathfinder.DistanceBetweenCells(unit.UnitInfo.CellLocation, targetCell);
        int distance = Pathfinder.DistanceBetweenCells(potentialCell, targetCell);

        if (TilemapCreator.UnitLocator.TryGetValue(new Vector2Int(targetCell.x, targetCell.z), out var unitOnTile))
        {
            // Add to score if Potential Cell is within Unit's Striking Range
            int projectedFutureDamage = 0;
            foreach (var action in unit.ActionSet.GetAllAttackActions())
            {
                if (action.Range < distance) { continue; }
                if (unit.UnitInfo.currentAP < action.APCost || unit.UnitInfo.currentMP < action.MPCost) { continue; }
            
                unitOnTile = TilemapCreator.UnitLocator[new Vector2Int(targetCell.x, targetCell.z)];
                int futureDamage = 0; 
            
                if (distance <= action.Range && unit.UnitInfo.UnitAffiliation != unitOnTile.UnitInfo.UnitAffiliation) { 
                    futureDamage = DamageCalculator.ProjectDamage(action,
                        unit.UnitInfo, unitOnTile.UnitInfo) * Mathf.RoundToInt(unit.Aggression); }
            
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
    public int CalcAllySynergyScore(AIUnit unitAI, Unit potentialUnit) {
        int score = 0;
        int distance = Pathfinder.DistanceBetweenUnits(unitAI, potentialUnit);
        
        // Score Factor 1: HP Ratio
        // The lower the ratio, the higher the score
        score += Mathf.RoundToInt(potentialUnit.UnitInfo.FinalHP / (float)potentialUnit.UnitInfo.currentHP * unitAI.AllySynergy);
        
        // Score Factor 2: Healing Impact
        // If an action would knock out the targeted unit, add a large amount to the score...
        // ...otherwise, the more healing projected, the higher the score
        // Deduct MP Cost and AP Cost needed for each specific action
        int projectedFutureDamage = 0;
        foreach (var action in unitAI.ActionSet.GetAllUnitActions())
        {
            // Skip Actions that Damage an Ally
            // Skip Actions that can't be Currently Used by the UnitAI
            if (action.DamageType is DamageType.Physical or DamageType.Magic ) { continue; }
            if (action.DamageType == DamageType.Healing &&
                potentialUnit.UnitInfo.currentHP == potentialUnit.UnitInfo.FinalHP) { continue; }
            if (unitAI.UnitInfo.currentMP < action.MPCost) { continue; }
            if (unitAI.UnitInfo.IsDead() && action.DamageType == DamageType.Healing) { continue; }
            if (ChainSystem.ReactionInProgress && action.Range + action.Splash < distance) { continue; }
            
            
            // Instantiate the damage value the current action will have in the future
            int futureHealing = 0;
            
            // Max Possible Distance UnitAI can be from the Potential Unit to Perform the Action on their Current Turn
            int maxPossibleDistance =
                action.Range + unitAI.UnitInfo.FinalMove * (unitAI.UnitInfo.currentAP - action.APCost);
            
            // Additional AP Needed to Move Towards Potential Unit to Perform Action
            int additionalAPCost = (distance - action.Range) / unitAI.UnitInfo.FinalMove;
            
            // Calculate the Value of Potential Healing
            if (unitAI.UnitInfo.UnitAffiliation == potentialUnit.UnitInfo.UnitAffiliation) { 
                futureHealing = DamageCalculator.ProjectHealing(action,
                    unitAI.UnitInfo, potentialUnit.UnitInfo); }

            // Calculate Bonus If Unit Will Be Revived
            if (potentialUnit.UnitInfo.IsDead() && action.DamageType == DamageType.Revival) {
                futureHealing += potentialUnit.UnitInfo.FinalHP;
            }
            
            futureHealing -= Mathf.RoundToInt(action.MPCost * unitAI.ResourceManagement);
            futureHealing -= Mathf.RoundToInt((action.APCost + additionalAPCost) * unitAI.ReactionAllocation);
            futureHealing *= (int)unitAI.AllySynergy;
            if (futureHealing > projectedFutureDamage) { projectedFutureDamage = futureHealing; }
        }

        score += projectedFutureDamage;

        if (projectedFutureDamage > 0) {
            score += Mathf.RoundToInt(potentialUnit.UnitInfo.FinalHP /
                (float)potentialUnit.UnitInfo.currentHP * (int)unitAI.AllySynergy);
        }
        
        return score;
    }
    
    // Resource Management Score
    // Completed!!
    public int CalcResourceManagementScore(AIUnit unit) {
        return -(unit.UnitInfo.currentMP - Action.MPCost) / unit.UnitInfo.FinalMP * Mathf.RoundToInt(unit.ResourceManagement);
    }

    // Reaction Awareness Score
    // Completed!!
    public int CalcReactionAwarenessScore(AIUnit unit, Vector3Int potentialCell, List<Unit> enemyUnits) {
        int score = 0;
        int threatLevel = 0;
        
        foreach (var enemy in enemyUnits) {
            int distance = Pathfinder.DistanceBetweenCells(potentialCell, enemy.UnitInfo.CellLocation);
            if (distance <= enemy.UnitInfo.FinalSense) {
                score -= 10;
                threatLevel -= enemy.UnitInfo.currentLevel;
            }
        }

        score += threatLevel;
        score *= Mathf.RoundToInt(unit.ReactionAwareness);
        //Debug.Log("Unit " + potentialCell + " Reaction Awareness Score: " + score);
        return score;
    }
    
    // Reaction Allocation Score
    // Completed!!
    public int CalcReactionAllocationScore(AIUnit unit) {
        return 20 * (unit.UnitInfo.currentAP - Action.APCost) * Mathf.RoundToInt(unit.ReactionAllocation);
    }

    // TODO: Implement Softmax to organically choose a unit to target (Most AI will target the same unit)
    public UnitAITargeting EvaluateScore(AIUnit unitAI)
    {
        var score = 0;
        foreach (var potentialUnit in TilemapCreator.UnitLocator.Values)
        {
            var newScore = CalcPriorityScore(unitAI, potentialUnit);
            Debug.Log(potentialUnit.GameObj.name + " target score = " + newScore);
            //var newScore = unitAI.unitInfo.UnitAffiliation == potentialUnit.unitInfo.UnitAffiliation ? 
                //CalcAggressionScore(unitAI, potentialUnit) : CalcAllySynergyScore(unitAI, potentialUnit);

            //SurvivalScore = 0;
            //TacticalPosScore = CalcTacticalPositioningScore(unit, potentialCell, targetCell);
            //AllySynergyScore = CalcAllySynergyScore(unit, potentialCell, enemyUnits);
            //ResourceManagementScore = 0;
            //ReactionAwarenessScore = CalcReactionAwarenessScore(unit, potentialCell, enemyUnits);
            //ReactionAllocationScore = 0;
            if (newScore <= score) continue;
            score = newScore;
            TargetUnit = potentialUnit;
        }
        
        
        
        return this;
    }

    public int TotalScore() => AggressionScore + SurvivalScore + TacticalPosScore + AllySynergyScore + ResourceManagementScore + ReactionAwarenessScore + ReactionAllocationScore;
}
