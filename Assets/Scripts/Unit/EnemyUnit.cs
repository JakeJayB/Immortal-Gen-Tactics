using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

// TODO: Create a dynamic behavior that scans tiles for units to target move for.
public class EnemyUnit : Unit
{
    // AI Behavior Set
    public List<AIBehavior> AIBehavior;
    
    // AI Behavioral Factors
    public float Aggression { get; private set; } = 5;             // Values Damage Dealt & Kills
    public float Survival { get; private set; } = 1;                // Values Avoiding Damage & Death
    public float TacticalPositioning { get; private set; } = 1;    // Values Advantageous Positioning
    public float AllySynergy { get; private set; } = 0;             // Values Team-Based Actions
    public float ResourceManagement { get; private set; }           // Values Optimal Resource Balancing (MP, AP, Items)
    public float ReactionAwareness { get; private set; } = 1;       // Values Minimal Reaction Opportunities from Opponent
    public float ReactionAllocation { get; private set; } = 0;      // Values Saving AP for Reactions
    
    // Start is called before the first frame update
    void Start()
    {
        unitInfo = GetComponent<UnitInfo>();
        unitMovement = GetComponent<UnitMovement>();
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
        GameObject gameObj = new GameObject("Enemy Unit " + initLocation);
        EnemyUnit unit = gameObj.AddComponent<EnemyUnit>();
        unit.gameObj = gameObj;

        unit.unitInfo = gameObj.AddComponent<UnitInfo>();
        unit.unitInfo.CellLocation = initLocation;
        unit.unitInfo.UnitDirection = unitDirection;
        unit.unitInfo.UnitAffiliation = UnitAffiliation.Enemy;
        unit.unitInfo.sprite = Resources.Load<Sprite>("Sprites/Units/Test_Enemy/Test_Sprite_Enemy(Down-Left)");

        unit.unitEquipment = new UnitEquipment(unit.unitInfo);

        unit.unitRenderer = gameObj.AddComponent<UnitRenderer>();
        unit.unitRenderer.Render(initLocation, unitDirection);

        unit.gameObj.AddComponent<BillboardEffect>();
        unit.unitMovement = unit.gameObj.AddComponent<UnitMovement>();
        return unit;
    }

    public void StartTurn() 
    {
        CanvasUI.ShowTurnUnitInfoDisplay(unitInfo);
        StartCoroutine(DecideAction()); 
    }

    // TODO: Make the executions of UnitActions as coroutines so that no future instructions will
    // TODO: execute until they finish. This is especially important for EnemyAI Action Calls.
    private IEnumerator DecideAction()
    {
        var actionDetermined = false;
        
        AIBehavior = AIBehavior.OrderBy(a => a.Priority).ToList();

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
            float bestScore = -9999;
            UnitAction bestAction = new Wait();
            if (FindNearbyUnits().Count > 0)
            {
                var nearbyUnit = FindNearbyUnits()[0].unitInfo.Vector2CellLocation();
                
                bestAction.CalculateActionScore(this, nearbyUnit);
        
                foreach (var action in unitInfo.ActionSet.GetAllTurnActions())
                {
                    if (unitInfo.currentAP < action.APCost || unitInfo.currentMP < action.MPCost) { continue; }
                    
                    float newScore = action.CalculateActionScore(this, nearbyUnit);
                    if (newScore > bestScore) { 
                        bestScore = newScore;
                        bestAction = action;
                    }
                }
            }
            else
            {
                bestAction.CalculateActionScore(this, unitInfo.Vector2CellLocation());
            }
        
            Debug.Log(name + " choose to " + bestAction.Name + " this turn.");
            ChainSystem.HoldPotentialChain(bestAction, this);
            yield return ChainSystem.AddAction(bestAction.ActionScore.Vector2PotentialLocation());
        }
        
        if (ChainSystem.Peek().GetType() == typeof(Wait)) {
            yield return new WaitForSeconds(2f);
            yield return ChainSystem.ExecuteChain();
        }
        else {
            yield return ChainSystem.ExecuteChain();
            StartCoroutine(DecideAction());
        }
    }
    
    private void EndTurn()
    {
        Debug.Log(name + " is activating potion from its EndTurn()!");
        StartCoroutine(new Wait().ExecuteAction(this, new Vector2Int(unitInfo.CellLocation.x, unitInfo.CellLocation.z)));
    }

    // TODO: AI doesn't perform any organic decision-making if it doesn't recognize an enemy.
    // TODO: Switch the value of 20 back to the unit's finalMove.
    public List<Unit> FindNearbyUnits()
    {
        List<Unit> nearbyUnits = new List<Unit>();
        
        // Check Units based on Unit's Movement Range for now until finalized
        // It will save an AP for an action once they select and move towards the opponent
        var surroundings = Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[unitInfo.Vector2CellLocation()],
            20 * (unitInfo.currentAP), Pattern.Splash);

        // Don't Count the same tile as the Unit conducting the search
        surroundings.Remove(TilemapCreator.TileLocator[unitInfo.Vector2CellLocation()]);

        foreach (Tile tile in surroundings)
        {
            var cell = new Vector2Int(tile.TileInfo.CellLocation.x, tile.TileInfo.CellLocation.z);
            if (TilemapCreator.UnitLocator.TryGetValue(cell, out Unit unit)) {
                if (!unit.unitInfo.IsDead()) { nearbyUnits.Add(unit); }
            }
        }

        return nearbyUnits;
    }
    
    private Tile DecideTile(Unit targetUnit, List<Tile> tempPath)
    {
        Tile chosenTile = null;
        List<Tile> shortestPath = tempPath;

        foreach (Tile tile in Rangefinder.GetTilesInRange(
                     TilemapCreator.TileLocator[new Vector2Int(unitInfo.CellLocation.x, unitInfo.CellLocation.z)],
                     unitInfo.finalMove, Pattern.Splash))
        {
            Vector2Int tileCell = new Vector2Int(tile.TileInfo.CellLocation.x, tile.TileInfo.CellLocation.z);
            
            if (!TilemapCreator.UnitLocator.ContainsKey(tileCell))
            {
                List<Tile> foundPath = Pathfinder.FindPath(TilemapCreator.TileLocator[tileCell],
                    TilemapCreator.TileLocator[new Vector2Int(targetUnit.unitInfo.CellLocation.x, targetUnit.unitInfo.CellLocation.z)]);

                if (foundPath.Count < shortestPath.Count)
                {
                    shortestPath = foundPath;
                    chosenTile = tile;
                }
            }
        }

        return chosenTile ?? TilemapCreator.TileLocator[new Vector2Int(shortestPath[^1].TileInfo.CellLocation.x, shortestPath[^1].TileInfo.CellLocation.z)];
    }

    public bool InRange(Unit unit, int range, Pattern pattern)
    {
        var neighborTiles =
            Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[unitInfo.Vector2CellLocation()], range, pattern);

        return neighborTiles.Contains(TilemapCreator.TileLocator[unit.unitInfo.Vector2CellLocation()]);
    }
}
