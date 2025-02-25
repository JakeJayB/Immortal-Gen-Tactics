using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : UnitAction
{
    public sealed override string Name { get; protected set; } = "Move";
    public override ActionType ActionType { get; protected set; } = ActionType.Move;
    public sealed override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_walk";
    public sealed override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    public override void ActivateAction(Unit unit)
    {
        UnitMenu.HideMenu();
        ActionUtility.ShowSelectableTilesForAction(unit);
        MapCursor.ActionState();
    }

    public override void ExecuteAction(Unit unit)
    {
        throw new System.NotImplementedException();
    }
}
