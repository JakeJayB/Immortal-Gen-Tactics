using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Storage : UnitAction
{
    public abstract int Capacity { get; protected set; }
    public abstract List<UnitAction> Items { get; protected set; }
    
    protected void StoreItem(UnitAction newItem)
    {
        for (int i = 0; i < Items.Count; i++) {
            if (Items[i] == null) { Items[i] = newItem; return; }
        }
        
        Debug.LogError("ERROR: Storage for 'Pouch' accessory is full.");
    }

    protected void UseItem(UnitAction item) { Items.Remove(item); }
}
