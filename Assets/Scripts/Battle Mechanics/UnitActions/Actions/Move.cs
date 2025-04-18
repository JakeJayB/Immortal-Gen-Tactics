using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : UnitAction
{
    public sealed override string Name { get; protected set; } = "Move";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 1;
    public override int Priority { get; protected set; } = 0;
    public override DamageType DamageType { get; protected set; } = DamageType.None;
    public override int BasePower { get; protected set; } = 0;
    public override ActionType ActionType { get; protected set; } = ActionType.Move;
    public override Pattern AttackPattern { get; protected set; } = Pattern.None;
    public override int Range { get; protected set; } = 0;
    public override AIActionScore ActionScore { get; protected set; }
    public override int Splash { get; protected set; }
    public override List<Tile> Area(Unit unit) {
        return Rangefinder.GetTilesInRange(TilemapCreator.TileLocator[unit.unitInfo.Vector2CellLocation()],
            unit.unitInfo.finalMove, Pattern.Splash);
    }

    public sealed override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_walk";
    public sealed override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    public override float CalculateActionScore(EnemyUnit unit, Vector2Int selectedCell)
    {
        ActionScore = new AIActionScore();
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");
        Debug.Log("Initial Heuristic Score: " + ActionScore.TotalScore());

        foreach (var tile in Area(unit))
        {
            if (TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out Unit foundUnit)) { continue; }

            foreach (var nearbyUnit in unit.FindNearbyUnits())
            {
                AIActionScore newScore = new AIActionScore().EvaluateScore(this, unit, tile.TileInfo.CellLocation,
                    nearbyUnit.unitInfo.CellLocation, new List<Unit>(), unit.FindNearbyUnits());
            
                // Debug.Log("Heuristic Score at Tile " + tile.TileInfo.CellLocation + ": " + newScore.TotalScore());
                if (newScore.TotalScore() > ActionScore.TotalScore()) ActionScore = newScore;
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
        // Have AI Units show their range of movement before moving
        if (unit.GetComponent<EnemyUnit>())
        {
            ActionUtility.ShowSelectableTilesForAction(unit, Name);
            yield return new WaitForSeconds(2.0f);
            ActionUtility.HideSelectableTilesForAction(unit);
        }
        
        // Spend an Action Point to execute the Action
        --unit.unitInfo.currentAP;
        
        // Remove the Location the Unit is currently at in UnitLocator
        TilemapCreator.UnitLocator.Remove(new Vector2Int(unit.unitInfo.CellLocation.x, unit.unitInfo.CellLocation.z));
        
        // Updates the location as the Unit moves
        yield return unit.unitMovement.Move(unit, selectedCell);
        
        // Adds the location of the tile the Unit ended at in UnitLocator
        TilemapCreator.UnitLocator.Add(new Vector2Int(unit.unitInfo.CellLocation.x, unit.unitInfo.CellLocation.z), unit);

        if (!unit.GetComponent<EnemyUnit>()) { MapCursor.currentUnit = selectedCell; }
    }
}
