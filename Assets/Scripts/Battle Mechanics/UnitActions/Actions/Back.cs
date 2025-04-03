using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Back : UnitAction
{
    public override string Name { get; protected set; } = "Back";
    public override int MPCost { get; protected set; }
    public override int APCost { get; protected set; }
    public override int Priority { get; protected set; }
    public override DamageType DamageType { get; protected set; }
    public override int BasePower { get; protected set; }
    public override ActionType ActionType { get; protected set; }
    public override Pattern AttackPattern { get; protected set; }
    public override int Range { get; protected set; }
    public override AIActionScore ActionScore { get; protected set; }
    public override int Splash { get; protected set; }

    public override List<Tile> Area(Unit unit)
    {
        throw new NotImplementedException();
    }

    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_back";
    
    public override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    public override float CalculateActionScore(EnemyUnit unit, Vector2Int selectedCell)
    {
        throw new System.NotImplementedException();
    }

    public override void ActivateAction(Unit unit)
    {
        UnitMenu.InSubMenu = false;
        UnitMenu.DisplayUnitMenu(unit.unitInfo.ActionSet.GetAllActions());
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        throw new System.NotImplementedException();
    }
}
