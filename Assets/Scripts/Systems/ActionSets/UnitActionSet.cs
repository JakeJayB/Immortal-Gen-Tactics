using System.Collections.Generic;
using System.Linq;
using IGT.Core;
using UnityEngine;

public class UnitActionSet {
    private Unit unit;
    
    private Dictionary<ActionType, List<UnitAction>> unitActions = new() {
        { ActionType.Attack, new List<UnitAction>() { new Attack() } }
    };

    private Dictionary<UnitClass, List<UnitAction>> learnedActions = new();
    
    public UnitActionSet(Unit unit) { this.unit = unit; }
    
    public void Initialize(UnitDefinitionData unitData) {
        var actions = unit is AIUnit ? unitData.GetAIActions() : unitData.GetActions();
        foreach (var actionID in actions) {
            if (actionID < 0) { continue; }
            UnitAction action = UnitActionLibrary.FindAction(actionID);
            if (action != null) LearnAction(action); 
        }
    }

    private void LearnAction(UnitAction action) {
        if (!learnedActions.TryGetValue(action.ClassType, out List<UnitAction> actionList)) {
            actionList = new List<UnitAction>();
            learnedActions[action.ClassType] = actionList;
        }

        if (ContainsAction(learnedActions[action.ClassType], action)) {
            Debug.LogError($"ERROR: Unit already knows the action {action.Name}");
            return;
        }

        actionList.Add(action);
        Debug.Log($"Unit has learned {action.Name}!");
    }
    
    public void AddAction(UnitAction action) {
        if (!unitActions.TryGetValue(action.ActionType, out List<UnitAction> actionList)) {
            actionList = new List<UnitAction>();
            unitActions[action.ActionType] = actionList;
        }

        if (action.ActionType != ActionType.Item && ContainsAction(unitActions[action.ActionType], action)) {
            Debug.LogError($"ERROR: Unit already knows the action {action.Name}");
            return;
        }

        actionList.Add(action);
        Debug.Log($"Unit has learned {action.Name}!");
    }

    public void RemoveAction(UnitAction action) {
        if (unitActions.TryGetValue(action.ActionType, out var actionList)) {
            var existing = actionList.FirstOrDefault(a => a.GetType() == action.GetType());
            if (existing != null) actionList.Remove(existing);
        }
    }

    private bool ContainsAction(List<UnitAction> actions, UnitAction action) {
        return actions.Any(actionInList => actionInList.GetType() == action.GetType());
    }
    
    // TODO: Have actions flattened in the following order -> Movement, Weapon, Class, Secondary, Accessory, Wait
    private List<UnitAction> FlattenActions(bool distinct = false) {
        return (distinct
            ? unitActions.Values.SelectMany(list => list).Distinct()
            : unitActions.Values.SelectMany(list => list))
            .Concat(learnedActions[unit.UnitInfo.Class]).ToList();
    }

    public List<UnitAction> GetAllTurnActions() {
        List<UnitAction> turnActions = new List<UnitAction> { new Move() };
        turnActions.AddRange(FlattenActions());
        turnActions.Add(new Wait());
        return turnActions;
    }
    
    public List<UnitAction> GetAITurnActions() {
        List<UnitAction> turnActions = FlattenActions(true);
        turnActions.Add(new Move());
        turnActions.Add(new Wait());
        return turnActions;
    }
    
    public List<UnitAction> GetAllReactions() {
        List<UnitAction> allReactions = new List<UnitAction> { new Evade() };
        allReactions.AddRange(FlattenActions());
        return allReactions;
    }
    
    public List<UnitAction> GetAIReactions() {
        List<UnitAction> allReactions = new List<UnitAction> { new Evade() };
        allReactions.AddRange(FlattenActions(true));
        allReactions.Add(new DoNothing());
        return allReactions;
    }
    
    public List<UnitAction> GetAllUnitActions() {
        return FlattenActions();
    }

    public List<UnitAction> GetAllActionsOfType(ActionType actionType) {
        return unitActions.TryGetValue(actionType, out var list)
            ? new List<UnitAction>(list)
            : new List<UnitAction>();
    }
}
