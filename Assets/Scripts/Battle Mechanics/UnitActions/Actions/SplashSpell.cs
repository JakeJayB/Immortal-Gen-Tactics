using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashSpell : UnitAction
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override string Name { get; protected set; } = "SplashSpell (Test)";
    public override int MPCost { get; protected set; } = 5;
    public override int APCost { get; protected set; } = 1;
    public override int Priority { get; protected set; } = 3;
    public override DamageType DamageType { get; protected set; } = DamageType.Magic;
    public override int BasePower { get; protected set; } = 10;
    public override ActionType ActionType { get; protected set; } = ActionType.Attack;
    public override Pattern AttackPattern { get; protected set; } = Pattern.Splash;
    public override int Range { get; protected set; } = 3;
    public override AIActionScore ActionScore { get; protected set; }
    public override int Splash { get; protected set; } = 1;
    public override List<Tile> Area(Unit unit) {
        return Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[unit.unitInfo.Vector2CellLocation()],
            Range, Pattern.Splash);
    }

    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_attack";
    public override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }

    public override float CalculateActionScore(EnemyUnit unit, Vector2Int selectedCell)
    {
        ActionScore = new AIActionScore();
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");
        Debug.Log("Initial Heuristic Score: " + ActionScore.TotalScore());

        foreach (var tile in Area(unit))
        {
            foreach (var targetedTile in TilemapUtility.GetSplashTilesInRange(tile, Splash))
            {
                if (TilemapCreator.UnitLocator.TryGetValue(targetedTile.TileInfo.Vector2CellLocation(), out Unit foundUnit))
                {
                    AIActionScore newScore = new AIActionScore().EvaluateScore(this, unit, tile.TileInfo.CellLocation,
                        foundUnit.unitInfo.CellLocation, new List<Unit>(), unit.FindNearbyUnits());
            
                    Debug.Log("Heuristic Score at Tile " + tile.TileInfo.CellLocation + ": " + newScore.TotalScore());
                    if (newScore.TotalScore() > ActionScore.TotalScore()) ActionScore = newScore;

                    break;
                }
            }
        }

        Debug.Log("Best Heuristic Score: " + ActionScore.TotalScore());
        Debug.Log("Decided Cell Location: " + ActionScore.PotentialCell);
        return ActionScore.TotalScore();
    }

    public override void ActivateAction(Unit unit)
    {
        UnitMenu.HideMenu();
        ActionUtility.ShowSelectableTilesForAction(unit, Name);
        ChainSystem.HoldPotentialChain(this, unit);
        MapCursor.ActionState();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        // Spend an Action Point to execute the Action
        PayAPCost(unit);

        foreach (var tile in Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[selectedCell], Splash,
                     AttackPattern))
        {
            if (TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out var targetUnit))
            {
                DamageCalculator.DealDamage(DamageType, unit.unitInfo, targetUnit.unitInfo);
                Debug.Log("Attack: unit attacked! HP: " + targetUnit.unitInfo.currentHP + "/" + targetUnit.unitInfo.finalHP);
            }
        }
        
        yield return null;
    }
}
