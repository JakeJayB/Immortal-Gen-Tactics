using UnityEditor;

namespace IGT.UnityEditor
{
    [CustomPropertyDrawer(typeof(ActionDefinitionData))]
    public class ItemDefinitionDataDrawer : PropertyDrawer {
        [UnitActionIDDropdown(UnitActionType.Item)] public int[] StorageAItems;
        [UnitActionIDDropdown(UnitActionType.Item)] public int[] StorageBItems;
    }
}
