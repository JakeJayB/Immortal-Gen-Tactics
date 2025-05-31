using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class SoftmaxAILogic
{
    public static UnitAction DetermineAction(List<UnitAction> potentialActions, List<float> actionScores, float temperature = 1.75f)
    {
        // Filter out negative scores (Absolutely no current value)
        (potentialActions, actionScores) = FilterNegatives(potentialActions, actionScores);
        
        int count = potentialActions.Count;
        float[] expScores = new float[count];
        float total = 0f;

        // Apply softmax
        for (int i = 0; i < count; i++)
        {
            expScores[i] = Mathf.Exp(actionScores[i] / temperature);
            total += expScores[i];
        }

        // Normalize to probabilities
        float[] probabilities = new float[count];
        for (int i = 0; i < count; i++)
        {
            probabilities[i] = expScores[i] / total;
        }

        // Choose based on weighted random
        float rand = UnityEngine.Random.value;
        float cumulative = 0f;

        for (int i = 0; i < count; i++)
        {
            cumulative += probabilities[i];
            Debug.Log($"Checking action {potentialActions[i]}, rand: {rand}, cumulative: {cumulative}");
            if (rand <= cumulative)
                return potentialActions[i];
        }

        // Fallback (shouldn't happen)
        return potentialActions[count - 1];
    }

    private static (List<UnitAction>, List<float>) FilterNegatives(List<UnitAction> potentialActions, List<float> actionScores)
    {
        List<UnitAction> filteredActions = new();
        List<float> filteredScores = new();

        for (int i = 0; i < potentialActions.Count; i++)
        {
            if (actionScores[i] >= 0)
            {
                filteredActions.Add(potentialActions[i]);
                filteredScores.Add(actionScores[i]);
            }
        }
        
        return (filteredActions, filteredScores);
    }
}
