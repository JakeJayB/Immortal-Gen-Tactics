using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : UnitAction
{
    // Start is called before the first frame update
    public sealed override string Name { get; protected set; } = "Attack";
    public override int Priority { get; protected set; }
    public sealed override ActionType ActionType { get; protected set; } = ActionType.Weapon;
    public sealed override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_attack";

    public sealed override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    public override void ActivateAction(Unit unit)
    {
        UnitMenu.HideMenu();
        ActionUtility.ShowSelectableTilesForAction(unit, Name);
        ChainSystem.HoldPotentialChain(this, unit);
        MapCursor.ActionState();
        //throw new System.NotImplementedException();

    }

    public override void ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        // unit = unit attacking, selectedCell = unit being attacked
        throw new System.NotImplementedException();
    }
}
