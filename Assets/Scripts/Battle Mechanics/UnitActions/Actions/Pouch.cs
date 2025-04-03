using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pouch : UnitAction
{
    public override string Name { get; protected set; } = "Pouch";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 0;
    public override int Priority { get; protected set; } = 0;
    public override DamageType DamageType { get; protected set; } = DamageType.None;
    public override int BasePower { get; protected set; } = 0;
    public override ActionType ActionType { get; protected set; } = ActionType.Accessory;
    public override Pattern AttackPattern { get; protected set; } = Pattern.None;
    public override int Range { get; protected set; } = 0;
    public override AIActionScore ActionScore { get; protected set; }
    public override int Splash { get; protected set; }

    public override List<Tile> Area(Unit unit)
    {
        throw new NotImplementedException();
    }

    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_item";
    
    public override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    public override float CalculateActionScore(EnemyUnit unit, Vector2Int selectedCell)
    {
        return -9999;
    }

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
