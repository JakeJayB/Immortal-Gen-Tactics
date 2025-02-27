using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitAction
{
    public abstract string Name { get; protected set; }
    public abstract int Priority { get; protected set; }
    public abstract ActionType ActionType { get; protected set; }
    public abstract string SlotImageAddress { get; protected set; }

    public abstract Sprite SlotImage();

    public abstract void ActivateAction(Unit unit);
    public abstract void ExecuteAction(Unit unit, Vector2Int selectedCell);
}
