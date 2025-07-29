using UnityEngine;

public abstract class Storage : UnitAction
{
    public abstract int Capacity { get; protected set; }
    public abstract UnitAction[] Items { get; protected set; }

    public void Initialize(int[] items) {
        foreach (var item in items) {
            StoreItem(UnitActionLibrary.FindAction(item));
        }
    }
    
    protected void StoreItem(UnitAction newItem) {
        for (int i = 0; i < Capacity; i++) {
            if (Items[i] == null) { Items[i] = newItem; return; }
        }
        
        Debug.LogError("ERROR: Storage for 'Pouch' accessory is full.");
    }
    
    protected void UseItem(int itemIndex) {
        Items[itemIndex] = null;
        OrganizeItems(itemIndex);
    }

    protected void OrganizeItems(int itemIndex) {
        for (int i = itemIndex; i < Capacity - 2; i++) {
            if (Items[i + 1] == null) return;
            Items[i] = Items[i + 1];
            Items[i + 1] = null;
        }
    }
    public UnitAction[] GetItems() { return Items; }
}
