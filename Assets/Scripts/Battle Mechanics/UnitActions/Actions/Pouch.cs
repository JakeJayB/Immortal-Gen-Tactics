using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pouch : UnitAction
{
    public override string Name { get; protected set; } = "Pouch";
    public override int APCost { get; protected set; } = 0;
    public override int Priority { get; protected set; } = 0;
    public override ActionType ActionType { get; protected set; } = ActionType.Accessory;
    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_item";
    
    public override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    
    public override void ActivateAction(Unit unit)
    {
        UnitMenu.InSubMenu = true;
        UnitMenu.DisplayUnitMenu(new List<UnitAction> { new Potion(), new Potion(), new Potion(), new Back() });
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        throw new System.NotImplementedException();
    }
}
