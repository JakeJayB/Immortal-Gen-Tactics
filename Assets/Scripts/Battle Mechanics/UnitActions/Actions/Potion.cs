using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : UnitAction
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string Name { get; protected set; } = "Potion";
    public override int APCost { get; protected set; } = 1;
    public override int Priority { get; protected set; } = 1;
    public override ActionType ActionType { get; protected set; } = ActionType.Item;
    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_item";
    public override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }

    public override void ActivateAction(Unit unit)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        throw new System.NotImplementedException();
    }
}
