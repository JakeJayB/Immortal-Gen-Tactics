using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AIUnit : Unit
{
    // AI Behavior Set
    public AIUnitBehavior AIUnitBehavior;
    public List<AIBehavior> AIBehavior;
    
    // AI Behavioral Factors
    public float Aggression;                  // Values Damage Dealt & Kills
    public float Survival;                    // Values Avoiding Damage & Death
    public float TacticalPositioning;         // Values Advantageous Positioning
    public float AllySynergy;                 // Values Team-Based Actions
    public float ResourceManagement;          // Values Optimal Resource Balancing (MP, AP, Items)
    public float ReactionAwareness;           // Values Minimal Reaction Opportunities from Opponent
    public float ReactionAllocation;          // Values Saving AP for Reactions

    public Unit targetedUnit;

    public AIUnit(GameObject gameObj, UnitDefinitionData unitData) : base(gameObj, unitData) {
        if (unitData == null) return;

        Aggression = unitData.Behaviors.Aggression;
        Survival = unitData.Behaviors.Survival;
        TacticalPositioning = unitData.Behaviors.TacticalPositioning;
        AllySynergy = unitData.Behaviors.AllySynergy;
        ResourceManagement = unitData.Behaviors.ResourceManagement;
        ReactionAwareness = unitData.Behaviors.ReactionAwareness;
        ReactionAllocation = unitData.Behaviors.ReactionAllocation;
    }
    
    public Unit InitializeAI(Vector3Int initLocation, UnitDirection unitDirection)
    {
        unitInfo.CellLocation = initLocation;
        unitInfo.UnitDirection = unitDirection;
        unitInfo.sprite = Resources.Load<Sprite>("Sprites/Units/Test_Enemy/Test_Sprite_Enemy(Down-Left)");
        
        AIUnitBehavior = gameObj.AddComponent<AIUnitBehavior>();
        AIBehavior = new List<AIBehavior>()
        {
            new AIBehavior
            {
                Priority = 0,
                Condition = () => RuleBasedAILogic.CurrentHPIsBelowPercent(0.4f, unitInfo) && RuleBasedAILogic.HasItem(new Potion(), this),
                Action = new Potion()
            },
            new AIBehavior
            {
                Priority = 9,
                Condition = () => RuleBasedAILogic.CurrentAPIsBelow(1, unitInfo),
                Action = new Wait()
            },
        };

        SpriteRenderer spriteRender = gameObj.GetComponent<SpriteRenderer>();
        UnitRenderer unitRenderer = new UnitRenderer(spriteRender);
        unitRenderer.Render(initLocation, unitDirection);
        
        // TODO: Fix this so that sprite renderer or some other script handles updating the sprites properly
        spriteRender.sprite = Resources.Load<Sprite>("Sprites/Units/Test_Enemy/Test_Sprite_Enemy(Down-Left)");
        return this;
    }

    private void InitializeAIBehaviors() {
        var unitInitializer = gameObj.GetComponent<UnitDefinitionData>().Behaviors;
        if (unitInitializer == null) return;

        Aggression = unitInitializer.Aggression;
        Survival = unitInitializer.Survival;
        TacticalPositioning = unitInitializer.TacticalPositioning;
        AllySynergy = unitInitializer.AllySynergy;
        ResourceManagement = unitInitializer.ResourceManagement;
        ReactionAwareness = unitInitializer.ReactionAwareness;
        ReactionAllocation = unitInitializer.ReactionAllocation;
    }

    // TODO: AI doesn't perform any organic decision-making if it doesn't recognize an enemy.
    // TODO: Switch the value of 20 back to the unit's finalMove.
    public List<Unit> FindNearbyUnits()
    {
        List<Unit> nearbyUnits = new List<Unit>();
        
        // Check Units based on Unit's Movement Range for now until finalized
        // It will save an AP for an action once they select and move towards the opponent
        var surroundings = Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[unitInfo.Vector2CellLocation()],
            30 * (unitInfo.currentAP), Pattern.Splash);

        // Don't Count the same tile as the Unit conducting the search
        surroundings.Remove(TilemapCreator.TileLocator[unitInfo.Vector2CellLocation()]);

        foreach (Tile tile in surroundings)
        {
            var cell = new Vector2Int(tile.TileInfo.CellLocation.x, tile.TileInfo.CellLocation.z);
            if (TilemapCreator.UnitLocator.TryGetValue(cell, out Unit unit)) {
                if (!unit.unitInfo.IsDead() && unit.unitInfo.UnitAffiliation != unitInfo.UnitAffiliation) { nearbyUnits.Add(unit); }
            }
        }

        return nearbyUnits;
    }

    public bool InRange(Unit unit, int range, Pattern pattern)
    {
        var neighborTiles =
            Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[unitInfo.Vector2CellLocation()], range, pattern);

        return neighborTiles.Contains(TilemapCreator.TileLocator[unit.unitInfo.Vector2CellLocation()]);
    }
}
