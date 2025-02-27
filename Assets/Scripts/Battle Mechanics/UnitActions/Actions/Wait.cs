using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wait : UnitAction
{
    public sealed override string Name { get; protected set; } = "Wait";
    public override int Priority { get; protected set; }
    public override ActionType ActionType { get; protected set; } = ActionType.Wait;
    public sealed override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_wait";
    public sealed override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    public override void ActivateAction(Unit unit)
    {
        throw new System.NotImplementedException();
    }

    public override void ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        throw new System.NotImplementedException();
    }
}
