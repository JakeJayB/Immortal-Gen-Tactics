using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class UnitAILogic : MonoBehaviour
{

    // This is test based on EnemyUnit's Move stat
    // The less AP that are needed to reach a unit, the more it will prioritize them
    public static Unit PrioritizeUnit(EnemyUnit AI, List<Unit> units)
    {
        (Unit, int) prioritizedUnit = (null, 0);
        foreach (var unit in units)
        {
            if (prioritizedUnit.Item1 != null && !prioritizedUnit.Item1.gameObj) { prioritizedUnit = (unit, Pathfinder.DistanceBetweenUnits(AI, unit) / AI.unitInfo.FinalMove); }

            int usedAP = Pathfinder.DistanceBetweenUnits(prioritizedUnit.Item1, unit) / AI.unitInfo.FinalMove;
            if (AI.InRange(unit, 1, Pattern.Linear)) { usedAP -= 2; }
            if (usedAP < prioritizedUnit.Item2) { prioritizedUnit = (unit, usedAP); }
        }

        return prioritizedUnit.Item1;
    }
}
