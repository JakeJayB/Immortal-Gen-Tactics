using System;

[Serializable]
public class ActionDefinitionData {
    [UnitActionIDDropdown(UnitActionType.Action)] public int[] Actions;
}
