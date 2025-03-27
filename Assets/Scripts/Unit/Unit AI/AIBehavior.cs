using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehavior
{
    public int Priority { get; set; }
    public Func<bool> Condition { get; set; }
    public UnitAction Action { get; set; }
}
