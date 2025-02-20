using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : UnitAction
{
    // Start is called before the first frame update
    public override string Name { get; protected set; } = "Item";
    public override ActionType ActionType { get; protected set; } = ActionType.Item;
    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_item";
    
    public sealed override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
}
