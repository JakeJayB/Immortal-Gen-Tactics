using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIMovementScoring
{
    public int DistanceScore { get; set; } = -1;
    public int ReactionRisk { get; set; } = -1;
    public int AllySynergy { get; set; } = -1;
    public Vector3Int PotentialCell { get; set; }
    public Vector3Int TargetCell { get; set; }

    public int CalcDistanceScore(Vector3Int potentialCell, Vector3Int targetCell) {
        int distance = Pathfinder.DistanceBetweenCells(potentialCell, targetCell);
        Debug.Log("Unit " + potentialCell + " Distance Score: " + Mathf.Max(0, 100 - distance * 10));
        return Mathf.Max(0, 100 - distance * 10);
    }

    public int CalcReactionRisk(Vector3Int potentialCell, List<Unit> enemyUnits) {
        int risk = 0;
        
        foreach (var enemy in enemyUnits) {
            int distance = Pathfinder.DistanceBetweenCells(potentialCell, enemy.unitInfo.CellLocation);
            if (distance <= enemy.unitInfo.finalSense) risk += 10;
        }

        Debug.Log("Unit " + potentialCell + " Reaction Risk Score: " + risk);
        return risk;
    }

    public int CalcAllySynergy(Vector3Int potentialCell, List<Unit> allyUnits)
    {
        int synergy = 0;
        
        foreach (var ally in allyUnits) {
            int distance = Pathfinder.DistanceBetweenCells(potentialCell, ally.unitInfo.CellLocation);
            if (distance <= ally.unitInfo.finalSense) synergy += 10;
        }

        Debug.Log("Unit " + potentialCell + " Ally Synergy Score: " + synergy);
        return synergy;
    }

    public AIMovementScoring EvaluateMovementScore(Vector3Int potentialCell, Vector3Int targetCell, List<Unit> allyUnits, List<Unit> enemyUnits)
    {
        AIMovementScoring score = new AIMovementScoring
        {
            DistanceScore = CalcDistanceScore(potentialCell, targetCell),
            ReactionRisk = CalcReactionRisk(potentialCell, enemyUnits),
            AllySynergy = CalcAllySynergy(potentialCell, allyUnits),
            PotentialCell = potentialCell,
            TargetCell = targetCell
        };

        return score;
    }

    public int TotalScore() => DistanceScore + ReactionRisk + AllySynergy;
}
