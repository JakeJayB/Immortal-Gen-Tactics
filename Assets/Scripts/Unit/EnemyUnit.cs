using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyUnit : Unit
{
    // AI Behavior Set
    private List<AIBehavior> AIBehavior;
    
    // AI Behavioral Factors
    public float Aggression;                  // Values Damage Dealt & Kills
    public float Survival;                    // Values Avoiding Damage & Death
    public float TacticalPositioning;         // Values Advantageous Positioning
    public float AllySynergy;                 // Values Team-Based Actions
    public float ResourceManagement;          // Values Optimal Resource Balancing (MP, AP, Items)
    public float ReactionAwareness;           // Values Minimal Reaction Opportunities from Opponent
    public float ReactionAllocation;          // Values Saving AP for Reactions

    public Unit targetedUnit;

    // Start is called before the first frame update
    void Start()
    {
        unitInfo = GetComponent<UnitInfo>();
        AIBehavior = new List<AIBehavior>()
        {
            new AIBehavior
            {
                Priority = 0,
                Condition = () => RuleBasedAILogic.CurrentHPIsBelowPercent(0.4f, unitInfo) && RuleBasedAILogic.HasItem(new Potion(), unitInfo),
                Action = new Potion()
            },
            new AIBehavior
            {
                Priority = 9,
                Condition = () => RuleBasedAILogic.CurrentAPIsBelow(1, unitInfo),
                Action = new Wait()
            },
        };
        
        // Remove following lines after EnemyUnits are properly
        // implemented through the json files.
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Units/Test_Enemy/Test_Sprite_Enemy(Down-Left)");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public static Unit InitializeAI(Vector3Int initLocation, UnitDirection unitDirection)
    {
        GameObject gameObj = Instantiate(Resources.Load<GameObject>("Prefabs/Unit/Enemy"));
        EnemyUnit unit = gameObj.AddComponent<EnemyUnit>();
        unit.gameObj = gameObj;
        
        unit.InitializeAIBehaviors();
        unit.unitInfo = gameObj.GetComponent<UnitInfo>();
        
        unit.unitInfo.CellLocation = initLocation;
        unit.unitInfo.UnitDirection = unitDirection;
        unit.unitInfo.sprite = Resources.Load<Sprite>("Sprites/Units/Test_Enemy/Test_Sprite_Enemy(Down-Left)");

        SpriteRenderer spriteRender = gameObj.GetComponent<SpriteRenderer>();
        UnitRenderer unitRenderer = new UnitRenderer(spriteRender);
        unitRenderer.Render(initLocation, unitDirection);

        unit.gameObj.AddComponent<BillboardEffect>();
        return unit;
    }

    private void InitializeAIBehaviors() {
        var unitInitializer = GetComponent<UnitInitializer>().Behaviors;
        if (unitInitializer == null) return;

        Aggression = unitInitializer.Aggression;
        Survival = unitInitializer.Survival;
        TacticalPositioning = unitInitializer.TacticalPositioning;
        AllySynergy = unitInitializer.AllySynergy;
        ResourceManagement = unitInitializer.ResourceManagement;
        ReactionAwareness = unitInitializer.ReactionAwareness;
        ReactionAllocation = unitInitializer.ReactionAllocation;
    }

    public void StartTurn() 
    {
        CanvasUI.ShowTurnUnitInfoDisplay(unitInfo);
        StartCoroutine(DecideAction()); 
    }

    public IEnumerator React()
    {
        CanvasUI.ShowTurnUnitInfoDisplay(unitInfo);
        yield return StartCoroutine(DecideAction()); 
    }
    
    private IEnumerator DecideAction()
    {
        var actionDetermined = false;
        var isReacting = ChainSystem.ReactionInProgress;
        
        AIBehavior = AIBehavior.OrderBy(a => a.Priority).ToList();
        
        // Decide Target
        targetedUnit = !targetedUnit ? new UnitAITargeting().EvaluateScore(this).TargetUnit : targetedUnit;

        foreach (var behavior in AIBehavior)
        {
            if (!behavior.Condition()) continue;

            actionDetermined = true;
            Debug.Log("AIUnit performing " + behavior.Action.Name + "!");
            ChainSystem.HoldPotentialChain(behavior.Action, this);
            yield return ChainSystem.AddAction(new Vector2Int(unitInfo.CellLocation.x, unitInfo.CellLocation.z));
            break;
        }

        if (!actionDetermined)
        {
            Debug.Log($"!!!!!!!!!!!!{targetedUnit.unitInfo.Vector2CellLocation()} is the target!!!!!!!!!!!!!!");
            var nearbyUnit = targetedUnit.unitInfo.Vector2CellLocation();
            
            // Softmax Implementation
            List<UnitAction> turnActions = isReacting
                ? unitInfo.ActionSet.GetAIReactions()
                : unitInfo.ActionSet.GetAITurnActions();
            List<UnitAction> potentialActions = new();
            List<float> actionScores = new List<float>();
            foreach (var action in turnActions)
            {
                if (unitInfo.currentAP < action.APCost || unitInfo.currentMP < action.MPCost) { continue; }

                /*
                if ((targetedUnit.unitInfo.UnitAffiliation == UnitAffiliation.Enemy &&
                     action.DamageType is DamageType.Physical or DamageType.Magic) ||
                    (targetedUnit.unitInfo.UnitAffiliation == UnitAffiliation.Player &&
                     action.DamageType == DamageType.Healing))
                    continue;
                */
                
                potentialActions.Add(action);
                actionScores.Add(action.CalculateActionScore(this, nearbyUnit) / 2); // TODO: Fix Score Balancing to Prevent Softmax Overflow
            }

            UnitAction chosenAction = SoftmaxAILogic.DetermineAction(potentialActions, actionScores);
        
            Debug.Log(name + " choose to " + chosenAction.Name + " this turn.");
            ChainSystem.HoldPotentialChain(chosenAction, this);
            yield return ChainSystem.AddAction(chosenAction.ActionScore.Vector2PotentialLocation());
            
            // If AI Unit selects an action that impacts the target unit, de-prioritize them
            if (chosenAction.ActionType != ActionType.Move) { targetedUnit = null; }
        }

        if (isReacting) {
            targetedUnit = null;
            yield return null;
        }
        else if (ChainSystem.Peek().GetType() == typeof(Wait)) {
            yield return new WaitForSeconds(2f);
            yield return ChainSystem.ExecuteChain();
            targetedUnit = null;
        }
        else {
            yield return ChainSystem.ExecuteChain();
            if (targetedUnit && targetedUnit.unitInfo.IsDead()) { targetedUnit = null; }
            StartCoroutine(DecideAction());
        }
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
