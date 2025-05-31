using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UnitActionSet
{
    private Dictionary<ActionType, List<UnitAction>> unitActions;

    public UnitActionSet()
    {
        unitActions = new Dictionary<ActionType, List<UnitAction>>()
        {
            { ActionType.Attack, new List<UnitAction>() { new Attack() } },
            { ActionType.React, new List<UnitAction>() {  } },
        };
    }

    public void AddAction(UnitAction action)
    {
        if (!unitActions.TryGetValue(action.ActionType, out List<UnitAction> actionList)) {
            actionList = new List<UnitAction>();
            unitActions[action.ActionType] = actionList;
        }

        if (UnitActionExistsInSet(unitActions[action.ActionType], action)) {
            Debug.LogError($"ERROR: Unit already knows the action {action.Name}");
            return;
        }

        actionList.Add(action);
        Debug.Log($"Unit has learned {action.Name}!");
    }

    public void RemoveAction(UnitAction action) {
        var actionTypeSet = unitActions[action.ActionType];
        
        if (UnitActionExistsInSet(actionTypeSet, action)) {
            actionTypeSet.Remove(actionTypeSet.First(a => a.GetType() == action.GetType()));
        }
    }

    private bool UnitActionExistsInSet(List<UnitAction> actions, UnitAction action)
    {
        foreach (UnitAction actionInList in actions) {
            if (actionInList.GetType() == action.GetType()) { return true; }
        }
        return false;
    }

    public List<UnitAction> GetAllTurnActions()
    {
        List<UnitAction> allActions = new List<UnitAction>();
        
        // Add all Turn-Based Actions
        allActions.Add(new Move());
        foreach (var actionList in unitActions.Values) { allActions.AddRange(actionList); }
        allActions.Add(new Wait());
        
        return allActions;
    }
    
    public List<UnitAction> GetAllReactions()
    {
        List<UnitAction> allReactions = new List<UnitAction>();
        
        // Add all Reaction-Based Actions
        allReactions.Add(new Evade());
        foreach (var actionList in unitActions.Values) { allReactions.AddRange(actionList); }
        return allReactions;
    }
    
    public List<UnitAction> GetAllUnitActions()
    {
        List<UnitAction> allReactions = new List<UnitAction>();
        
        // Add all Actions unique to the unit
        foreach (var actionList in unitActions.Values) { allReactions.AddRange(actionList); }
        return allReactions;
    }

    public List<UnitAction> GetAllAttackActions() {
        List<UnitAction> attackActions = new List<UnitAction>();
        attackActions.AddRange(unitActions[ActionType.Attack]);
        return attackActions;
    }
    
    public List<UnitAction> GetAITurnActions() {
        List<UnitAction> allActions = new List<UnitAction>();
        
        // Add all Turn-Based Actions
        // Setting Move after Unique Actions to prevent bad movement
        foreach (var actionList in unitActions.Values) { allActions.AddRange(actionList); }
        allActions.Add(new Move());
        //allActions.Add(new Wait());
        
        return allActions;
    }
    
    public List<UnitAction> GetAllItems() {
        List<UnitAction> attackActions = new List<UnitAction>();
        attackActions.AddRange(unitActions[ActionType.Item]);
        return attackActions;
    }
}
