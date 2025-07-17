using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Pouch : Storage {
    public override string Name { get; protected set; } = "Pouch";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 0;
    public override int BasePower { get; protected set; } = 0;
    public override int Range { get; protected set; } = 0;
    public override int Splash { get; protected set; } = 0;
    public override int Priority { get; protected set; } = 0;
    public override DamageType DamageType { get; protected set; } = DamageType.None;
    public override ActionType ActionType { get; protected set; } = ActionType.Storage;
    public override TilePattern AttackTilePattern { get; protected set; } = TilePattern.None;
    public override AIActionScore ActionScore { get; protected set; }
    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) { return new List<Tile>(); }
    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_item";
    
    public override float CalculateActionScore(AIUnit unit, Vector2Int selectedCell) { return -9999; }
    
    public sealed override int Capacity { get; protected set; } = 3;
    public sealed override UnitAction[] Items { get; protected set; }
    public Pouch() { Items = new UnitAction[Capacity]; }
    
    public override void ActivateAction(Unit unit) {
        UnitMenu.SubMenu = this;
        UnitMenu.InSubMenu = true;
        
        // Instantiate items as UnitActions
        var pouchActions = new List<UnitAction>();
        foreach (var pocket in Items) {
            if (pocket == null) { continue; }
            pouchActions.Add(pocket);
        }
        pouchActions.Add(new Back());
        
        UnitMenu.DisplayUnitSubMenu(unit, pouchActions);
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell) {
        UseItem(UnitMenuCursor.slotIndex);
        yield return null;
    }
}
