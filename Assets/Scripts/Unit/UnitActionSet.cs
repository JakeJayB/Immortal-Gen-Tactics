using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class UnitActionSet
{
    private Dictionary<ActionType, List<UnitAction>> unitActions;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public UnitActionSet()
    {
        unitActions = new Dictionary<ActionType, List<UnitAction>>()
        {
            { ActionType.Move, new List<UnitAction>() { new Move() } },
            { ActionType.React, new List<UnitAction>() {  } },
            { ActionType.Attack, new List<UnitAction>() { new Attack() } },
            { ActionType.Wait, new List<UnitAction>() { new Wait() } },
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
        
        foreach (var actionList in unitActions.Values) {
            allActions.AddRange(actionList);
        }
        return allActions;
    }
    
    public List<UnitAction> GetAllReactions()
    {
        List<UnitAction> allReactions = new List<UnitAction>();
        
        foreach (var actionList in unitActions) {
            if (actionList.Key != ActionType.Move && actionList.Key != ActionType.Wait) {
                allReactions.AddRange(actionList.Value);
            }
        }
        return allReactions;
    }
    

    public List<UnitAction> GetAllAttackActions() {
        List<UnitAction> attackActions = new List<UnitAction>();
        attackActions.AddRange(unitActions[ActionType.Attack]);
        return attackActions;
    }
}
