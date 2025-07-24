using System;
using System.Linq;

[Serializable]
public class ItemDefinitionData {
    public int[] StorageAItems;
    public int[] StorageBItems;
    public int[] All() { return StorageAItems.Concat(StorageBItems).ToArray(); }
}
