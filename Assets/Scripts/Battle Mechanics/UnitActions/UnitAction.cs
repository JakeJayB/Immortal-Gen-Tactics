using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Wait is forced to use MonoBehaviour to start the coroutine.
// TODO: Fix this by creating a direction selector and having it start the coroutine instead.
public abstract class UnitAction
{
    public abstract string Name { get; protected set; }
    public abstract int APCost { get; protected set; }
    public abstract int Priority { get; protected set; }
    public abstract ActionType ActionType { get; protected set; }
    public abstract string SlotImageAddress { get; protected set; }

    public abstract Sprite SlotImage();

    public abstract void ActivateAction(Unit unit);
    public abstract IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell);

    protected void PayAPCost(Unit unit) { unit.unitInfo.currentAP -= APCost; }
}
