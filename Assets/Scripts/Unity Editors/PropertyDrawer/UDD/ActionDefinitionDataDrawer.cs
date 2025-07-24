using UnityEditor;

#if UNITY_EDITOR
namespace IGT.UnityEditor
{
    [CustomPropertyDrawer(typeof(ActionDefinitionData))]
    public class ActionDefinitionDataDrawer : PropertyDrawer {
        [UnitActionIDDropdown(UnitActionType.Action)] public int[] Actions;
    }
}
#endif
