using System;
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
    public int ResourceScore { get; set; } = -1;
    public int ReactionScore { get; set; } = -1;
    public Vector3Int PotentialCell { get; private set; }
    public Vector3Int TargetCell { get; set; }
    
    // CalcDamageScore
    // [+] Scores Damage/Healing Done To Ally & Enemy Units
    // [+] Scores Potential Death
    // [+] Scores Potential Revival
    public int CalcDamageScore(AIUnit unitAI, UnitAction action, Vector3Int potentialCell, bool moved = false)
    {
        int damageScore = 0;        // Holds the calculated damage score
        int projectedDamage = 0;    // Amount of damage projected to happen to the unit on tile
        Unit unitOnTile;            // Holds the unit found on an observed tile
        
        switch (action.AttackTilePattern)
        {
            case TilePattern.Direct:
                // Skip checking this tile if no unit exists on it...
                if (!TilemapCreator.UnitLocator.TryGetValue(new Vector2Int(potentialCell.x, potentialCell.z), out unitOnTile)) { break; }
                
                damageScore += CalcDamageToUnit(unitAI, unitOnTile);
                break;

            case TilePattern.Linear:
                foreach (var direction in TilemapUtility.GetDirectionalLinearTilesInRange(
                             TileLocator.SelectableTiles[moved ? new Vector2Int(PotentialCell.x, PotentialCell.z) : unitAI.UnitInfo.Vector2CellLocation()],
                             action.Range))
                {
                    // Only check the direction that contains the potential cell currently being scored
                    if (direction.Contains(
                            TileLocator.SelectableTiles[new Vector2Int(potentialCell.x, potentialCell.z)]))
                    {
                        foreach (var tile in direction) {
                            // Skip to the next tile if no unit exists on this one...
                            if (!TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out unitOnTile)) { continue; }
                            
                            damageScore += CalcDamageToUnit(unitAI, unitOnTile);
                        }

                        break;
                    }
                }
                
                break;
            
            case TilePattern.Rush:
                foreach (var direction in TilemapUtility.GetDirectionalLinearTilesInRange(
                             TileLocator.SelectableTiles[moved ? new Vector2Int(PotentialCell.x, PotentialCell.z) : unitAI.UnitInfo.Vector2CellLocation()],
                             action.Range))
                {
                    
                    // Only check the direction that contains the potential cell currently being scored
                    if (direction.Contains(
                            TileLocator.SelectableTiles[new Vector2Int(potentialCell.x, potentialCell.z)]))
                    {
                        Vector2Int displacement = new Vector2Int(potentialCell.x, potentialCell.z) - unitAI.UnitInfo.Vector2CellLocation();
                        Vector2Int rushDirection = new Vector2Int(Mathf.Clamp(displacement.x, -1, 1), Mathf.Clamp(displacement.y, -1, 1));
                        Unit projectedTarget = Pathfinder.ProjectedRushTarget(unitAI, rushDirection);

                        if (projectedTarget == null) break;
                        damageScore += CalcDamageToUnit(unitAI, projectedTarget);
                        break;
                    }
                }
                
                break;

            case TilePattern.Splash:
                foreach (var splashTile in TilemapUtility.GetSplashTilesInRange(
                             TileLocator.SelectableTiles[new Vector2Int(potentialCell.x, potentialCell.z)],
                             action.Splash))
                {
                    // Continue to the next tile if no unit exists on this current one
                    if (!TilemapCreator.UnitLocator.TryGetValue(splashTile.TileInfo.Vector2CellLocation(), out unitOnTile)) { continue; }
                    
                    damageScore += CalcDamageToUnit(unitAI, unitOnTile);
                }

                break;
            
            case TilePattern.None:
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
    public int CalcPositionScore(AIUnit unitAI, List<Unit> nearbyUnits)
    {
        int positionScore = 0;      // Holds the calculated damage score
        int distance = 0;
        
        // Score higher for DeadZoning a Target
        // TODO: Balance the score calculation.
        if (Pathfinder.DistanceBetweenCells(PotentialCell, TargetCell) < Action.Splash) 
        { positionScore += Mathf.RoundToInt(DamageScore * 0.2f * unitAI.AIBehavior.TacticalPositioning); }
        
        foreach (var nearbyUnit in nearbyUnits) {
            // If the action is move-based, get the distance between the AI unit's potential position and the nearby unit
            // Otherwise, get the distance between the AI unit's current position and nearby unit
            distance = Action.ActionType == ActionType.Move 
                ? Pathfinder.DistanceBetweenCells(PotentialCell, nearbyUnit.UnitInfo.CellLocation)
                : Pathfinder.DistanceBetweenUnits(unitAI, nearbyUnit);
            
            // Add to score if within reaction distance of an ally unit
            // Subtract from score if within reaction distance of an enemy unit
            // TODO: Account for enemy reactions that can reach them.
            // TODO: Calculate the potential damage that could reach them (hindsight).
            // TODO: Create a hindsight function that calculates the danger of discovered reactions from enemy units.
            if (nearbyUnit.UnitInfo.currentAP < 1 || distance > nearbyUnit.UnitInfo.FinalSense) continue;
            positionScore += nearbyUnit.UnitInfo.UnitAffiliation == unitAI.UnitInfo.UnitAffiliation
                ? nearbyUnit.UnitInfo.currentLevel * (int)unitAI.AIBehavior.ReactionAwareness
                : -nearbyUnit.UnitInfo.currentLevel * (int)unitAI.AIBehavior.ReactionAwareness;
        }

        if (ChainSystem.ReactionInProgress) return positionScore;
        
        if (TilemapCreator.UnitLocator.TryGetValue(new Vector2Int(TargetCell.x, TargetCell.z), out var unitOnTile)) {
            positionScore += Mathf.RoundToInt(
                (Pathfinder.DistanceBetweenUnits(unitAI, unitOnTile)
                - Pathfinder.DistanceBetweenCells(PotentialCell, TargetCell)) * unitAI.AIBehavior.TacticalPositioning);
        }
        
        return positionScore;
    }

    // TODO: Calculate the next best action after this current one to contribute its value to the score.
    public int CalcForesightScore(AIUnit unitAI)
    {
        // If the AI Unit is waiting, do not check for foresight
        if (Action.ActionType == ActionType.Wait) { return 0; }

        // If the AI Unit is reacting, do not check for foresight
        if (ChainSystem.ReactionInProgress) { return 0; }

        // If the current action will deplete all remaining AP, do not check for foresight
        if (unitAI.UnitInfo.currentAP - Action.APCost == 0) { return 0; }
        
        int foresightScore = 0;
        int projectedDamage = 0;    // Amount of damage projected to happen to the unit on tile

        if (TilemapCreator.UnitLocator.TryGetValue(new Vector2Int(TargetCell.x, TargetCell.z), out var unitOnTile))
        {
            // Add to score if Potential Cell is within Unit's Striking Range
            int projectedFutureDamage = 0;
            
            // If the action is move-based, get the distance between the AI unit's potential position and the nearby unit
            // Otherwise, get the distance between the AI unit's current position and nearby unit
            int distance = Action.ActionType == ActionType.Move 
                ? Pathfinder.DistanceBetweenCells(PotentialCell, unitOnTile.UnitInfo.CellLocation)
                : Pathfinder.DistanceBetweenUnits(unitAI, unitOnTile);
            
            foreach (var action in unitAI.ActionSet.GetAITurnActions())
            {
                if (action.Range + action.Splash < distance) { continue; }
                if (unitAI.UnitInfo.currentAP - Action.APCost < action.APCost || unitAI.UnitInfo.currentMP - Action.MPCost < action.MPCost) { continue; }
                
                int futureDamage = CalcFutureActionScore(unitAI, action);
            
                // The closer the AI unit is to its target, the higher the score
                // TODO: Figure out how to make the optimal distance formula
                if (action.Range != 0) {
                    futureDamage += distance * distance / action.Range * (int)unitAI.AIBehavior.TacticalPositioning;
                }
                
                if (futureDamage > projectedFutureDamage) { projectedFutureDamage = futureDamage; }
            }
            
            foresightScore += projectedFutureDamage;
        }
        
        return foresightScore;
    }

    public int CalcResourceScore(AIUnit unitAI)
    {
        int resourceScore = 0;

        resourceScore -= Mathf.RoundToInt(Action.MPCost * unitAI.AIBehavior.ResourceManagement);
        resourceScore -= Mathf.RoundToInt(Action.APCost * unitAI.AIBehavior.ResourceManagement);
        
        if (Action.ActionType == ActionType.Wait) {
            resourceScore += Mathf.RoundToInt((unitAI.UnitInfo.FinalAP - unitAI.UnitInfo.currentAP) * unitAI.AIBehavior.ReactionAllocation); 
        }

        return resourceScore;
    }

    public int CalcReactionScore(AIUnit unitAI)
    {
        int reactionScore = 0;
        
        (UnitAction initialAction, Vector2Int target, Unit attacker) = ChainSystem.GetInitialChain();

        Vector2Int projectedLocation = unitAI.UnitInfo.Vector2CellLocation();
        if (Action.ActionType == ActionType.Move) projectedLocation = new Vector2Int(PotentialCell.x, PotentialCell.z);
        else if (Action.AttackTilePattern == TilePattern.Rush)
        {
            Vector2Int displacement = new Vector2Int(PotentialCell.x, PotentialCell.z) - unitAI.UnitInfo.Vector2CellLocation();
            Vector2Int direction = new Vector2Int(Mathf.Clamp(displacement.x, -1, 1), Mathf.Clamp(displacement.y, -1, 1));
            projectedLocation = Pathfinder.ProjectedRushLocation(unitAI, direction);
        }
        
        int projectedDamage =
            DamageCalculator.ProjectDamage(initialAction, attacker.UnitInfo, unitAI.UnitInfo);
        
        if (TilemapUtility.GetTargetedArea(attacker, initialAction, target).Contains(
                TileLocator.SelectableTiles[projectedLocation]))
        {
            if (unitAI.UnitInfo.currentHP - projectedDamage < 1) {
                reactionScore -= unitAI.UnitInfo.FinalHP * (int)unitAI.AIBehavior.Survival;
            } else {
                reactionScore -= projectedDamage * (int)unitAI.AIBehavior.Survival;
            }
        }

        return reactionScore;
    }
    
    // -------------------------------------------------------------------------------------------------------------------

    private int CalcDamageToUnit(AIUnit unitAI, Unit unitOnTile)
    {
        int projectedDamage = 0;    // Projected Damage or Healing Inflicted
        int projectedScore = 0;     // Total Value Projected to Come From Projected Damage
        
        if (unitOnTile.UnitInfo.UnitAffiliation != unitAI.UnitInfo.UnitAffiliation) { // Calc Damage Towards Enemies
            projectedDamage =
                DamageCalculator.ProjectDamage(Action, unitAI.UnitInfo, unitOnTile.UnitInfo);

            projectedScore += unitOnTile.UnitInfo.currentHP - projectedDamage < 1 ?
                unitOnTile.UnitInfo.FinalHP * (int)unitAI.AIBehavior.Aggression :  // If the action kills a unit, score higher
                projectedDamage * (int)unitAI.AIBehavior.Aggression;   // Else, score the original damaging amount
                                
        } else { // Calc Healing Towards Allies
            projectedDamage =
                DamageCalculator.ProjectHealing(Action, unitAI.UnitInfo, unitOnTile.UnitInfo);
                                
            projectedScore += unitOnTile.UnitInfo.IsDead() && Action.DamageType == DamageType.Revival ? 
                unitOnTile.UnitInfo.FinalHP * (int)unitAI.AIBehavior.AllySynergy : // If the action revives a unit, score higher
                projectedDamage * (int)unitAI.AIBehavior.AllySynergy;  // Else, score the original healing amount
        }

        return projectedScore;
    }
    
    // -------------------------------------------------------------------------------------------------------------------

    private int CalcFutureActionScore(AIUnit unitAI, UnitAction futureAction)
    {
        int bestScore = 0;

        foreach (var tile in futureAction.Area(unitAI, Action.GetType() == typeof(Move) ? PotentialCell : null)) {
            int potentialScore = CalcDamageScore(unitAI, futureAction, tile.TileInfo.CellLocation);
            
            // Score higher for DeadZoning a Target
            // TODO: Balance the score calculation.
            if (Pathfinder.DistanceBetweenCells(tile.TileInfo.CellLocation, TargetCell) < futureAction.Splash) 
            { potentialScore += Mathf.RoundToInt(potentialScore * 0.2f * unitAI.AIBehavior.TacticalPositioning); }
            
            if (potentialScore > bestScore) { bestScore = potentialScore; }
        }

        return bestScore;
    }
    
    // -------------------------------------------------------------------------------------------------------------------
    
    public AIActionScore EvaluateScore(UnitAction action, AIUnit unit, Vector3Int potentialCell,
        Vector3Int targetCell, List<Unit> allyUnits, List<Unit> enemyUnits)
    {
        Action = action;
        PotentialCell = potentialCell;
        TargetCell = targetCell;

        DamageScore = CalcDamageScore(unit, action, potentialCell);
        PositionScore = CalcPositionScore(unit, enemyUnits);
        ForesightScore = CalcForesightScore(unit);
        ResourceScore = CalcResourceScore(unit);
        ReactionScore = ChainSystem.ReactionInProgress ? CalcReactionScore(unit) : 0;
        
        return this;
    }

    public int TotalScore() => DamageScore + PositionScore + ForesightScore + ResourceScore + ReactionScore;
    public Vector2Int Vector2PotentialLocation() { return new Vector2Int(PotentialCell.x, PotentialCell.z); }
}
