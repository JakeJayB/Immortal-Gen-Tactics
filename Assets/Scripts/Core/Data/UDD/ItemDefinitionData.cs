using System;
using System.Linq;

[Serializable]
public class ItemDefinitionData {
    [UnitActionIDDropdown(UnitActionType.Item)] public int[] StorageAItems;
    [UnitActionIDDropdown(UnitActionType.Item)] public int[] StorageBItems;

    public int[] All() { return StorageAItems.Concat(StorageBItems).ToArray(); }
}
