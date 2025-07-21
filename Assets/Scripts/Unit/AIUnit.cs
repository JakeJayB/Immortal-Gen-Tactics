using System.Collections.Generic;
using UnityEngine;

public class AIUnit : Unit {
    // AI Behavior Set
    public AIUnitBehavior AIUnitBehavior;
    public AIBehavior AIBehavior;
    public List<AICondition> AIConditions;

    public Unit targetedUnit;

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
    
    public Unit InitializeAI(Vector3Int initLocation, UnitDirection unitDirection)
    {
        SetInitialPosition(initLocation, unitDirection);
        AIUnitBehavior = GameObj.AddComponent<AIUnitBehavior>();
        AIConditions = new List<AICondition>()
        {
            new AICondition
            {
                Priority = 0,
                Condition = () => RuleBasedAILogic.CurrentHPIsBelowPercent(0.4f, UnitInfo) && RuleBasedAILogic.HasItem(new Potion(), this),
                Action = new Potion()
            },
            new AICondition
            {
                Priority = 9,
                Condition = () => RuleBasedAILogic.CurrentAPIsBelow(1, UnitInfo),
                Action = new Wait()
            },
        };

        UnitRenderer.Render(Resources.Load<Sprite>("Sprites/Units/Test_Enemy/Test_Sprite_Enemy(Down-Left)"));
        return this;
    }

    // TODO: AI doesn't perform any organic decision-making if it doesn't recognize an enemy.
    // TODO: Switch the value of 20 back to the unit's finalMove.
    public List<Unit> FindNearbyUnits()
    {
        List<Unit> nearbyUnits = new List<Unit>();
        
        // Check Units based on Unit's Movement Range for now until finalized
        // It will save an AP for an action once they select and move towards the opponent
        var surroundings = Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[UnitInfo.Vector2CellLocation()],
            30 * (UnitInfo.currentAP), TilePattern.Splash);

        // Don't Count the same tile as the Unit conducting the search
        surroundings.Remove(TilemapCreator.TileLocator[UnitInfo.Vector2CellLocation()]);

        foreach (Tile tile in surroundings)
        {
            var cell = new Vector2Int(tile.TileInfo.CellLocation.x, tile.TileInfo.CellLocation.z);
            if (TilemapCreator.UnitLocator.TryGetValue(cell, out Unit unit)) {
                if (!unit.UnitInfo.IsDead() && unit.UnitInfo.UnitAffiliation != UnitInfo.UnitAffiliation) { nearbyUnits.Add(unit); }
            }
        }

        return nearbyUnits;
    }

    public bool InRange(Unit unit, int range, TilePattern tilePattern)
    {
        var neighborTiles =
            Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[UnitInfo.Vector2CellLocation()], range, tilePattern);

        return neighborTiles.Contains(TilemapCreator.TileLocator[unit.UnitInfo.Vector2CellLocation()]);
    }
}
