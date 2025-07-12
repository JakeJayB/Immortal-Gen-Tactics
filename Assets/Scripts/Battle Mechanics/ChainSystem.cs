using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class ChainSystem
{
    private static List<(UnitAction action, Vector2Int target, Unit unit)> Chain = new List<(UnitAction action, Vector2Int target, Unit unit)>();
    private static int ChainCount => Chain.Count;
    public static (UnitAction, Vector2Int, Unit) CurrentChain;
    private static (UnitAction, Vector2Int, Unit) PotentialChain;
    public static bool ReactionInProgress = false;
    public static Unit ReactingUnit = null;


    public static void Clear()
    {
        Chain.Clear();
        ReactionInProgress = false;
        ReactingUnit = null;
        PotentialChain = new ValueTuple<UnitAction, Vector2Int, Unit>();
        CurrentChain = new ValueTuple<UnitAction, Vector2Int, Unit>();
    }

    public static void RegisterCleanup() =>
        MemoryManager.AddListeners(Clear);
    

    public static void HoldPotentialChain(UnitAction action, Unit unit) {
        PotentialChain = new ValueTuple<UnitAction, Vector2Int, Unit>(action, new Vector2Int(), unit);
    }

    public static void ReleasePotentialChain() {
        PotentialChain = new ValueTuple<UnitAction, Vector2Int, Unit>();
    }
    
    // This function might need to be an IEnumerator as well as ReactionPhase() so that
    // no executions happen during the chaining process.
    public static IEnumerator AddAction(Vector2Int target)
    {
        Chain.Add((PotentialChain.Item1, target, PotentialChain.Item3));
        ReleasePotentialChain();
        HeapifyUp(ChainCount - 1);

        if (UnitIsReacting()) { ReactionInProgress = false; } // If a Unit is reacting, end the process
        else { yield return ReactionPhase(target); } // Else, initiate a new reaction phase
    }

    public static IEnumerator ExecuteChain()
    {
        while (ChainCount > 0)
        {
            UnitAction action = Chain[0].action;
            Vector2Int target = Chain[0].target;
            Unit unit = Chain[0].unit;

            CurrentChain = Chain[0];
            Chain[0] = Chain[^1];
            Chain.RemoveAt(ChainCount - 1);

            if (ChainCount > 0) { HeapifyDown(0); }

            yield return action.ExecuteAction(unit, target);
        }
        
        TurnSystem.IsGameOver();
    }

    private static IEnumerator ReactionPhase(Vector2Int target)
    {
        var targetedArea = TilemapUtility.GetTargetedArea(Chain[0].unit, Chain[0].action, Chain[0].target);
        TilemapUtility.ShowTargetedArea(targetedArea);

        if (Chain[0].action.ActionType == ActionType.Attack)
        {
            foreach (var unit in TilemapCreator.UnitLocator.Values)
            {
                //var unitCell = new Vector2Int(unit.unitInfo.CellLocation.x, unit.unitInfo.CellLocation.z);
                var unitCell = unit.UnitInfo.Vector2CellLocation();
                var unitSense = Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[unitCell], unit.UnitInfo.FinalSense, TilePattern.Splash);

                // Unit cannot react if they don't have any available AP
                // Unit cannot react if they are dead
                // Unit cannot react if they already added an action to the ChainSystem
                // Unit cannot react if they are too far to sense the initial action
                // TODO: Fix this rule!!! Unit cannot react if the action is not an attack type
                if (unit.UnitInfo.currentAP <= 0 || unit.UnitInfo.IsDead() || Chain.Any(chain => chain.Item3 == unit) ||
                    !unitSense.Contains(TilemapCreator.TileLocator[target])) continue;
            
                Debug.Log($"Unit {unit.GameObj.name} already has action in chain: " + Chain.Any(chain => chain.Item3 == unit));
                Debug.Log($"Unit {unit.GameObj.name} can sense the nearby action: " + unitSense.Contains(TilemapCreator.TileLocator[target]));

                if (unit is AIUnit unitAI) {
                    ReactionInProgress = true;
                    yield return unitAI.AIUnitBehavior.React(unitAI);
                } // If the unit is an AI Enemy, do a specific instruction
                else {
                    yield return OfferChainReaction(unit);
                } // Else, offer the player the ability to react
            }
        }
        
        TilemapUtility.HideTargetedArea(targetedArea);
        yield return null;
    }

    private static IEnumerator OfferChainReaction(Unit unit)
    {
        Debug.Log("Unit " + unit.GameObj.name + " should be reacting...");
        ReactionInProgress = true;
        ReactingUnit = unit;
        Vector2Int unitCell = unit.UnitInfo.Vector2CellLocation();
        MapCursor.currentUnit = unitCell;
        MapCursor.SetHoverCell(unitCell);
        MapCursor.SetGameObjActive();
        CanvasUI.ShowTargetUnitInfoDisplay(unit.UnitInfo);

        yield return UnitMenu.ShowMenu(unit);
        yield return new WaitUntil(ReactionHasEnded);

        ReactingUnit = null;
        if (TurnSystem.CurrentUnit is AIUnit) { MapCursor.SetGameObjInactive(); }
        Debug.Log("Unit finished using reaction menu...");
    }
    
    public static UnitAction Peek()
    {
        if (ChainCount == 0) throw new InvalidOperationException("ChainSystem is empty!");
        return Chain[0].action;
    }

    private static void HeapifyUp(int index)
    {
        while (index > 0)
        {
            int parent = (index - 1) / 2;
            if (Chain[index].action.Priority >= Chain[parent].action.Priority) break;
            SwapActions(index, parent);
            index = parent;
        }
    }

    private static void HeapifyDown(int index)
    {
        while (true)
        {
            int leftChild = 2 * index + 1;
            int rightChild = 2 * index + 2;
            int smallest = index;

            if (leftChild < ChainCount && Chain[leftChild].action.Priority < Chain[smallest].action.Priority)
            {
                smallest = leftChild;
            }
            
            if (rightChild < ChainCount && Chain[rightChild].action.Priority < Chain[smallest].action.Priority)
            {
                smallest = rightChild;
            }

            if (smallest == index) break;

            SwapActions(index, smallest);
            index = smallest;
        }
    }

    private static void SwapActions(int x, int y) { (Chain[x], Chain[y]) = (Chain[y], Chain[x]); }

    public static bool UnitIsReacting() { return ReactionInProgress; }
    private static bool ReactionHasEnded() { return !ReactionInProgress; }

    public static (UnitAction action, Vector2Int target, Unit unit) GetInitialChain() {
        return Chain.FirstOrDefault(c => c.unit == TurnSystem.CurrentUnit);
    }
    
    public static (UnitAction action, Vector2Int target, Unit unit) GetUnitChain(Unit unit) {
        return Chain.FirstOrDefault(c => c.unit == unit);
    }
}
