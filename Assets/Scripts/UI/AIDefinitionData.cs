using System;

[Serializable]
public class AIDefinitionData {
    public float Aggression;
    public float Survival;
    public float TacticalPositioning;
    public float AllySynergy;
    public float ResourceManagement;
    public float ReactionAwareness;
    public float ReactionAllocation;

    public float[] All() {
        return new [] { Aggression, Survival, TacticalPositioning, AllySynergy, ResourceManagement, ReactionAwareness,
            ReactionAllocation };
    }
}
