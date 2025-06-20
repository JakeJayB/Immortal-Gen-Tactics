using System.Linq;
using UnityEngine;

public class RuleBasedAILogic
{
    // HP-Based Rules
    public static bool CurrentHPIsAbovePercent(float percentThreshold, UnitInfo unitInfo) {
        var healthPercent = unitInfo.currentHP / (float)unitInfo.FinalHP;
        return healthPercent > percentThreshold;
    }
    public static bool CurrentHPIsBelowPercent(float percentThreshold, UnitInfo unitInfo) {
        var healthPercent = unitInfo.currentHP / (float)unitInfo.FinalHP;
        return healthPercent < percentThreshold;
    }

    // AP-Based Rules
    public static bool CurrentAPIsAbove(int threshold, UnitInfo unitInfo) { return unitInfo.currentAP > threshold; }
    public static bool CurrentAPIsBelow(int threshold, UnitInfo unitInfo) { return unitInfo.currentAP < threshold; }
    
    // Item-Based Rules
    public static bool HasItem(UnitAction item, UnitInfo unitInfo) {
        return unitInfo.ActionSet.GetAllTurnActions().Any(action => action.GetType() == item.GetType());
    }
}
