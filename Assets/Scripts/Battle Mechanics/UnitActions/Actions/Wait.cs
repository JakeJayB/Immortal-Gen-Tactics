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
        UnitMenu.HideMenu();
        MapCursor.EndMove();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        // TODO: Have ActivateAction() call a direction selector for the player to choose which
        // TODO: direction to face the unit. Once selected, it should call this function to
        // TODO: end the move after instead of ActivateAction().
        
        MapCursor.EndMove();
        yield return null;
    }
}
