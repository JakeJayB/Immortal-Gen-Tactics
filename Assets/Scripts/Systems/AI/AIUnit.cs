using System.Collections.Generic;
using UnityEngine;

public class AIUnit : Unit {
    // AI Behavior Set
    public AITurnHandler AITurnHandler;
    public AIBehavior AIBehavior;
    public List<AICondition> AIConditions;
    
    public Unit targetedUnit;   // Unit that AI is currently targeting

    public AIUnit(GameObject gameObj,UnitDefinitionData unitData, SpriteRenderer spriteRenderer) 
        : base(gameObj, unitData, spriteRenderer) {
        if (unitData == null) return;

        AIBehavior = new AIBehavior(
            unitData.Behaviors.Aggression,
            unitData.Behaviors.Survival,
            unitData.Behaviors.TacticalPositioning,
            unitData.Behaviors.AllySynergy,
            unitData.Behaviors.ResourceManagement,
            unitData.Behaviors.ReactionAwareness,
            unitData.Behaviors.ReactionAllocation);
    }
    
    public void InitializeAI(Vector3Int initLocation, UnitDirection unitDirection) {
        SetInitialPosition(initLocation, unitDirection);
        InitializeConditions();
        AITurnHandler = GameObj.AddComponent<AITurnHandler>();
        UnitRenderer.Render(Resources.Load<Sprite>("Sprites/Units/Test_Enemy/Test_Sprite_Enemy(Down-Left)"));
    }

    private void InitializeConditions() {
        AIConditions = new List<AICondition>() {
            new AICondition {
                Priority = 0,
                Condition = () => RuleBasedAILogic.CurrentHPIsBelowPercent(0.4f, UnitInfo) && RuleBasedAILogic.HasItem(new Potion(), this),
                Action = new Potion()
            },
            new AICondition {
                Priority = 9,
                Condition = () => RuleBasedAILogic.CurrentAPIsBelow(1, UnitInfo),
                Action = new Wait()
            },
        };
    }
}
