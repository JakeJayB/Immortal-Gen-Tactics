using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : UnitAction
{
    // Start is called before the first frame update
    public override string Name { get; protected set; } = "Item";
    public override int MPCost { get; protected set; }
    public override int APCost { get; protected set; } = 0;
    public override int Priority { get; protected set; }
    public override DamageType DamageType { get; protected set; }
    public override int BasePower { get; protected set; }
    public override ActionType ActionType { get; protected set; } = ActionType.Item;
    public override Pattern AttackPattern { get; protected set; }
    public override int Range { get; protected set; }
    public override AIActionScore ActionScore { get; protected set; }
    public override List<Tile> Area(Unit unit)
    {
        throw new NotImplementedException();
    }

    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_item";
    
    public sealed override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    public override float CalculateActionScore(EnemyUnit unit, Vector2Int selectedCell)
    {
        throw new System.NotImplementedException();
    }

    public override void ActivateAction(Unit unit)
    {
        throw new System.NotImplementedException();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        yield return null;
    }
}
