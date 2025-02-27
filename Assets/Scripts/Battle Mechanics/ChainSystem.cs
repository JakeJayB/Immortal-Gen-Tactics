using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ChainSystem
{
    private static List<(UnitAction action, Vector2Int target, Unit unit)> Chain = new List<(UnitAction action, Vector2Int target, Unit unit)>();
    private static int ChainCount => Chain.Count;
    
    private static (UnitAction, Vector2Int, Unit) PotentialChain;

    public static void HoldPotentialChain(UnitAction action, Unit unit) {
        PotentialChain = new ValueTuple<UnitAction, Vector2Int, Unit>(action, new Vector2Int(), unit);
    }

    public static void ReleasePotentialChain() {
        PotentialChain = new ValueTuple<UnitAction, Vector2Int, Unit>();
    }
    
    public static void AddAction(Vector2Int target)
    {
        Chain.Add((PotentialChain.Item1, target, PotentialChain.Item3));
        ReleasePotentialChain();
        HeapifyUp(ChainCount - 1);
    }

    public static void ExecuteNextAction()
    {
        if (ChainCount == 0) throw new InvalidOperationException("ChainSystem is empty!");
        
        UnitAction action = Chain[0].action;
        Vector2Int target = Chain[0].target;
        Unit unit = Chain[0].unit;
        
        Chain[0] = Chain[^1];
        Chain.RemoveAt(ChainCount - 1);

        if (ChainCount > 0) { HeapifyDown(0); }

        action.ExecuteAction(unit, target);
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
}
