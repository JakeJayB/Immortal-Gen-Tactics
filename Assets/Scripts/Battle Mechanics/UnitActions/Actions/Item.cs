using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : UnitAction
{
    // Start is called before the first frame update
    public override string Name { get; protected set; } = "Item";
    public override int APCost { get; protected set; } = 0;
    public override int Priority { get; protected set; }
    public override ActionType ActionType { get; protected set; } = ActionType.Item;
    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_item";
    
    public sealed override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    public override void ActivateAction(Unit unit)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        yield return null;
    }
}
