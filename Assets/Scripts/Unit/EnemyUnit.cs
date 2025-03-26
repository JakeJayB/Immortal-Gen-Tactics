using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

// TODO: Create a dynamic behavior that scans tiles for units to target move for.
public class EnemyUnit : Unit
{
    // AI Behavioral Factors
    public float Aggressive { get; private set; } = 2;      // Values Damage Dealt
    public float Defensive { get; private set; }            // Values Damage Mitigated
    public float Accuracy { get; private set; }             // Values Actions with Higher Chances of Hitting
    public float Active { get; private set; }               // Values AP spent as Turn Actions
    public float SelfCentered { get; private set; }         // Values Supporting their Self
    public float Supportive { get; private set; }           // Values Supporting Allies
    
    // Start is called before the first frame update
    void Start()
    {
        unitInfo = GetComponent<UnitInfo>();
        unitMovement = GetComponent<UnitMovement>();
        
        // Remove following lines after EnemyUnits are properly
        // implemented through the json files.
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Units/Test_Enemy/Test_Sprite_Enemy(Down-Left)");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            StartTurn();
        }
    }
    
    public static Unit InitializeAI(Vector3Int initLocation, UnitDirection unitDirection)
    {
        GameObject gameObj = new GameObject("Enemy Unit " + initLocation);
        EnemyUnit unit = gameObj.AddComponent<EnemyUnit>();
        unit.gameObj = gameObj;

        unit.unitInfo = gameObj.AddComponent<UnitInfo>();
        unit.unitInfo.CellLocation = initLocation;
        unit.unitInfo.UnitDirection = unitDirection;
        unit.unitInfo.sprite = Resources.Load<Sprite>("Sprites/Units/Test_Enemy/Test_Sprite_Enemy(Down-Left)");

        unit.unitEquipment = new UnitEquipment(unit.unitInfo);

        unit.unitRenderer = gameObj.AddComponent<UnitRenderer>();
        unit.unitRenderer.Render(initLocation, unitDirection);

        unit.gameObj.AddComponent<BillboardEffect>();
        unit.unitMovement = unit.gameObj.AddComponent<UnitMovement>();
        return unit;
    }

    public void StartTurn() { StartCoroutine(DecideTurnOption()); }

    // TODO: Make the executions of UnitActions as coroutines so that no future instructions will
    // TODO: execute until they finish. This is especially important for EnemyAI Action Calls.
    public IEnumerator DecideTurnOption()
    {
        while (unitInfo.currentAP > 0)
        {
            var targetUnit = UnitAILogic.PrioritizeUnit(this, FindNearbyUnits());
            Debug.Log("Enemy Unit Targeting: " + targetUnit.name);
            
            // TODO: Replace the currentUnit value with units that the enemy can scan for nearby units instead
            if (InRange(targetUnit, 1, Pattern.Linear))
            {
                Debug.Log("Enemy Unit Attacking...");
                ChainSystem.HoldPotentialChain(new Attack(), this);
                yield return ChainSystem.AddAction(new Vector2Int(targetUnit.unitInfo.CellLocation.x, targetUnit.unitInfo.CellLocation.z));
            }
            else
            {
                var tempPath =
                    Pathfinder.FindPath(
                        TilemapCreator.TileLocator[new Vector2Int(unitInfo.CellLocation.x, unitInfo.CellLocation.z)],
                        TilemapCreator.TileLocator[new Vector2Int(targetUnit.unitInfo.CellLocation.x, targetUnit.unitInfo.CellLocation.z)]);
                var chosenTile = DecideTile(targetUnit, tempPath);
                Debug.Log("Moving to tile: " + chosenTile.TileInfo.CellLocation + "(Towards: " + new Vector2Int(chosenTile.TileInfo.CellLocation.x, chosenTile.TileInfo.CellLocation.z));
                ChainSystem.HoldPotentialChain(new Move(), this);
                yield return ChainSystem.AddAction(new Vector2Int(chosenTile.TileInfo.CellLocation.x, chosenTile.TileInfo.CellLocation.z));
            }

            yield return ChainSystem.ExecuteChain();
        }
       
        EndTurn();
    }
    
    private void EndTurn()
    {
        StartCoroutine(new Wait().ExecuteAction(this, new Vector2Int(unitInfo.CellLocation.x, unitInfo.CellLocation.z)));
    }

    private List<Unit> FindNearbyUnits()
    {
        List<Unit> nearbyUnits = new List<Unit>();
        
        // Check Units based on Unit's Movement Range for now until finalized
        // It will save an AP for an action once they select and move towards the opponent
        var surroundings = Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[unitInfo.Vector2CellLocation()],
            unitInfo.finalMove * (unitInfo.currentAP), Pattern.Splash);

        // Don't Count the same tile as the Unit conducting the search
        surroundings.Remove(TilemapCreator.TileLocator[unitInfo.Vector2CellLocation()]);

        foreach (Tile tile in surroundings)
        {
            var cell = new Vector2Int(tile.TileInfo.CellLocation.x, tile.TileInfo.CellLocation.z);
            if (TilemapCreator.UnitLocator.TryGetValue(cell, out Unit unit)) nearbyUnits.Add(unit);
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
