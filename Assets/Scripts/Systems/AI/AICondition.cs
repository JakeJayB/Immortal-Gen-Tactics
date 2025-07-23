using System;

public class AICondition
{
    public int Priority { get; set; }
    public Func<bool> Condition { get; set; }
    public UnitAction Action { get; set; }
}
