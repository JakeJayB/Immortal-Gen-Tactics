using System;
using System.Linq;

[Serializable]
public class UnitDefinitionData
{
    public StatsDefinitionData BaseStats = new();
    public ActionDefinitionData Actions = new();
    public ItemDefinitionData Items = new();
    public EquipmentDefinitionData Equipment = new();
    public AIDefinitionData Behaviors = new();
    public UnitAffiliation UnitAffiliation;
    public bool IsUnitAIControlled = false;
    
    public int[] GetActions() { return Actions.Actions; }
    public int[] GetItems() { return Items.All(); }
    public int[] GetEquipment() { return Equipment.All(); }
    public float[] GetBehaviors() { return Behaviors.All(); }
    public int[] GetAIActions() {
        var actions = Actions?.Actions ?? Array.Empty<int>();
        var items = Items?.All() ?? Array.Empty<int>();
        return actions.Concat(items).ToArray();
    }
}
