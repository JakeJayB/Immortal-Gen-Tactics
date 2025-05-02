using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryManager
{
    private static List<Action> callables;

    private static void InitializeCallables()
    {
        callables = new List<Action>();
    }

    public static void InvokeCleanup()
    {
        for (int i = 0; i < callables.Count; i++)
            callables[i]();
        
        callables = null;
    }

    public static void AddListeners(Action callable)
    {
        
        if (callables == null)
            InitializeCallables();
        
        callables.Add(callable);
    }
}
