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
    public int AggressionScore { get; set; } = -1;
    public int SurvivalScore { get; set; } = -1;
    public int TacticalPosScore { get; set; } = -1;
    public int AllySynergyScore { get; set; } = -1;
    public int ResourceManagementScore { get; set; } = -1;
    public int ReactionAwarenessScore { get; set; } = -1;
    public int ReactionAllocationScore { get; set; } = -1;
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
            if (nearbyUnit.unitInfo.currentAP < 1 || distance > nearbyUnit.unitInfo.finalSense) continue;
            positionScore += nearbyUnit.unitInfo.UnitAffiliation == unitAI.unitInfo.UnitAffiliation
                ? nearbyUnit.unitInfo.currentLevel * (int)unitAI.ReactionAwareness
                : -nearbyUnit.unitInfo.currentLevel * (int)unitAI.ReactionAwareness;
        }
        
        return positionScore;
    }

    // TODO: Don't count the foresight score if the wait score is less than or equal to 
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

            // If no action is within range of the AI unit and still has a value of zero,
            // have the AI unit move as close as possible.
            if (distance != 0) {
                //projectedFutureDamage += projectedFutureDamage == 0
                    //? 20 / distance * (int)unitAI.TacticalPositioning
                    //: 0;

                projectedFutureDamage += 20 / distance * (int)unitAI.TacticalPositioning;
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
    
    // Aggression Score
    // Calculates 
    // In-Progress
    // -> Missing <# of Enemies Killed * Aggression - # of Allies Killed * Ally Synergy>
    public int CalcAggressionScore(EnemyUnit unit, Vector3Int potentialCell, Vector3Int targetCell) {
        int score = 0;
        int distance = Pathfinder.DistanceBetweenCells(potentialCell, targetCell);
        Unit unitOnTile;
        
        // Add to score if Potential Cell is within Unit's Striking Range
        int projectedFutureDamage = 0;
        foreach (var action in unit.unitInfo.ActionSet.GetAllUnitActions())
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
                            if (!TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out unitOnTile)) { break; }
                        
                            int calcDamage = DamageCalculator.ProjectDamage(Action, unit.unitInfo, unitOnTile.unitInfo);
                        
                            projectedDamage += unitOnTile.unitInfo.UnitAffiliation != unit.unitInfo.UnitAffiliation ? 
                                calcDamage * Mathf.RoundToInt(unit.Aggression) : -calcDamage * Mathf.RoundToInt(unit.Aggression);
                                //calcDamage * Mathf.RoundToInt(unit.Aggression) : -calcDamage * Mathf.RoundToInt(unit.AllySynergy);
                            
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
                    if (!TilemapCreator.UnitLocator.TryGetValue(splashTile.TileInfo.Vector2CellLocation(), out unitOnTile)) { continue; }
                    
                    int calcDamage = DamageCalculator.ProjectDamage(Action, unit.unitInfo, unitOnTile.unitInfo);
                    
                    projectedDamage += unitOnTile.unitInfo.UnitAffiliation != unit.unitInfo.UnitAffiliation ? 
                        calcDamage * Mathf.RoundToInt(unit.Aggression) : -calcDamage * Mathf.RoundToInt(unit.Aggression);
                        //calcDamage * Mathf.RoundToInt(unit.Aggression) : -calcDamage * Mathf.RoundToInt(unit.AllySynergy);
                }

                break;
            
            case Pattern.None:
                break;

            default:
                Debug.LogError("ERROR: UnitAction requires 'AttackPattern'. (CalcAggressionScore)");
                break;
        }

        score += projectedDamage + projectedFutureDamage;
        //Debug.Log("Unit " + potentialCell + " Aggression Score: " + score);
        return score;
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
            foreach (var action in unit.unitInfo.ActionSet.GetAllUnitActions())
            {
                if (action.Range < distance) { continue; }
                if (unit.unitInfo.currentAP < action.APCost || unit.unitInfo.currentMP < action.MPCost) { continue; }
            
                unitOnTile = TilemapCreator.UnitLocator[new Vector2Int(targetCell.x, targetCell.z)];
                int futureDamage = 0; 
            
                if (distance <= action.Range && unit.unitInfo.UnitAffiliation != unitOnTile.unitInfo.UnitAffiliation) { 
                    futureDamage = DamageCalculator.ProjectDamage(action,
                        unit.unitInfo, unitOnTile.unitInfo) * Mathf.RoundToInt(unit.Aggression); }
                else if (distance <= action.Range && unit.unitInfo.UnitAffiliation == unitOnTile.unitInfo.UnitAffiliation) {
                    futureDamage = DamageCalculator.ProjectHealing(action,
                        unit.unitInfo, unitOnTile.unitInfo) * Mathf.RoundToInt(unit.AllySynergy);
                }
            
                if (futureDamage > projectedFutureDamage) { projectedFutureDamage = futureDamage; }
            }

            score += projectedFutureDamage;
        }
        
        score += (currentDistance - distance) * Mathf.RoundToInt(unit.TacticalPositioning);
        //Debug.Log("Unit " + potentialCell + " Distance Score: " + score);
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
                
                projectedHealing = DamageCalculator.ProjectHealing(Action, unit.unitInfo, unitOnTile.unitInfo) * (int)unit.AllySynergy;
                
                //projectedHealing *= unitOnTile.unitInfo.UnitAffiliation == unit.unitInfo.UnitAffiliation ?
                    //1 * Mathf.RoundToInt(unit.AllySynergy): -1 * Mathf.RoundToInt(unit.Aggression);
                
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
                            projectedHealing = calcDamage * Mathf.RoundToInt(unit.AllySynergy);
                            
                            //projectedHealing += unitOnTile.unitInfo.UnitAffiliation == unit.unitInfo.UnitAffiliation ? 
                                //calcDamage * Mathf.RoundToInt(unit.AllySynergy) : -calcDamage * Mathf.RoundToInt(unit.Aggression);
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
                    projectedHealing = calcDamage * Mathf.RoundToInt(unit.AllySynergy);
                        
                    //projectedHealing += unitOnTile.unitInfo.UnitAffiliation == unit.unitInfo.UnitAffiliation ? 
                        //calcDamage * Mathf.RoundToInt(unit.AllySynergy) : -calcDamage * Mathf.RoundToInt(unit.Aggression);
                }

                break;

            case Pattern.None:
                break;
            
            default:
                Debug.LogError("ERROR: UnitAction requires 'AttackPattern'. (CalcAggressionScore)");
                break;
        }

        synergy += projectedHealing;
        //Debug.Log("Unit " + potentialCell + " Ally Synergy Score: " + synergy);
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
        //Debug.Log("Unit " + potentialCell + " Reaction Awareness Score: " + score);
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

        var targetUnit = TilemapCreator.UnitLocator[new Vector2Int(targetCell.x, targetCell.z)];

        DamageScore = CalcDamageScore(unit);
        PositionScore = CalcPositionScore(unit, enemyUnits);
        ForesightScore = CalcForesightScore(unit);
        //AggressionScore = targetUnit.unitInfo.UnitAffiliation == UnitAffiliation.Player ? CalcAggressionScore(unit, potentialCell, targetCell) : 0;
        //SurvivalScore = 0;
        //TacticalPosScore = CalcTacticalPositioningScore(unit, potentialCell, targetCell);
        //AllySynergyScore = targetUnit.unitInfo.UnitAffiliation == UnitAffiliation.Enemy ? CalcAllySynergyScore(unit, potentialCell, enemyUnits) : 0;
        //ResourceManagementScore = 0;
        //ReactionAwarenessScore = CalcReactionAwarenessScore(unit, potentialCell, enemyUnits);
        //ReactionAllocationScore = 0;
        
        return this;
    }

    public int TotalScore() => DamageScore + PositionScore + ForesightScore;
    public Vector2Int Vector2PotentialLocation() { return new Vector2Int(PotentialCell.x, PotentialCell.z); }
}
