using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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

    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell)
    {
        return new List<Tile>();
    }

    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_item";
    
    public override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    public override float CalculateActionScore(EnemyUnit unit, Vector2Int selectedCell)
    {
        return -9999;
    }
    
    private int Capacity = 3;
    private List<UnitAction> Storage = new List<UnitAction>() {
        new Potion(),
        new Potion(),
        new Potion()
    };

    public void StoreItem(UnitAction newItem)
    {
        if (Storage.Count > Capacity) { return; }
        
        for (int i = 0; i < Storage.Count; i++) {
            if (Storage[i] == null) { Storage[i] = newItem; return; }
        }
        
        Debug.LogError("ERROR: Storage for 'Pouch' accessory is full.");
    }

    public void UseItem(UnitAction item) { Storage.Remove(item); }
    
    public override void ActivateAction(Unit unit)
    {
        UnitMenu.SubMenu = this;
        UnitMenu.InSubMenu = true;
        
        // Instantiate items as UnitActions
        var pouchActions = new List<UnitAction>();
        foreach (var pocket in Storage)
        {
            if (pocket == null) { continue; }
            pouchActions.Add(pocket);
        }
        pouchActions.Add(new Back());
        
        UnitMenu.DisplayUnitSubMenu(unit, pouchActions);
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        UseItem(ChainSystem.CurrentChain.Item1);
        yield return null;
    }
}
