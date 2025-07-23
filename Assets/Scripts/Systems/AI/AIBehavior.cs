public class AIBehavior {
    public float Aggression;                  // Values Damage Dealt & Kills
    public float Survival;                    // Values Avoiding Damage & Death
    public float TacticalPositioning;         // Values Advantageous Positioning
    public float AllySynergy;                 // Values Team-Based Actions
    public float ResourceManagement;          // Values Optimal Resource Balancing (MP, AP, Items)
    public float ReactionAwareness;           // Values Minimal Reaction Opportunities from Opponent
    public float ReactionAllocation;          // Values Saving AP for Reactions

    public AIBehavior(float aggression, float survival, float tacticalPositioning, float allySynergy,
        float resourceManagement, float reactionAwareness, float reactionAllocation)
    {
        Aggression = aggression;
        Survival = survival;
        TacticalPositioning = tacticalPositioning;
        AllySynergy = allySynergy;
        ResourceManagement = resourceManagement;
        ReactionAwareness = reactionAwareness;
        ReactionAllocation = reactionAllocation;
    }
}
