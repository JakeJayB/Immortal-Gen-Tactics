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
    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) {
        return Rangefinder.GetMoveTilesInRange(TilemapCreator.TileLocator[hypoCell.HasValue
                ? new Vector2Int(hypoCell.Value.x, hypoCell.Value.z)
                : unit.unitInfo.Vector2CellLocation()],
            unit.unitInfo.finalMove);
    }

    public sealed override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_walk";
    public sealed override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    public override float CalculateActionScore(EnemyUnit unit, Vector2Int selectedCell)
    {
        ActionScore = new AIActionScore();
        Debug.Log(Name + " Action Score Assessment ------------------------------------------------------");
        Debug.Log("Initial Heuristic Score: " + ActionScore.TotalScore());

        foreach (var tile in Area(unit, null))
        {
            if (TilemapCreator.UnitLocator.TryGetValue(tile.TileInfo.Vector2CellLocation(), out Unit foundUnit)) { continue; }

            AIActionScore newScore = new AIActionScore().EvaluateScore(this, unit, tile.TileInfo.CellLocation,
                TilemapCreator.UnitLocator[selectedCell].unitInfo.CellLocation, new List<Unit>(), unit.FindNearbyUnits());
            
            // Debug.Log("Heuristic Score at Tile " + tile.TileInfo.CellLocation + ": " + newScore.TotalScore());
            if (newScore.TotalScore() > ActionScore.TotalScore()) ActionScore = newScore;
            
            /*foreach (var nearbyUnit in unit.FindNearbyUnits())
            {
                if (nearbyUnit.unitInfo.UnitAffiliation == unit.unitInfo.UnitAffiliation) { continue; }
                
                AIActionScore newScore = new AIActionScore().EvaluateScore(this, unit, tile.TileInfo.CellLocation,
                    TilemapCreator.UnitLocator[selectedCell].unitInfo.CellLocation, new List<Unit>(), unit.FindNearbyUnits());
            
                // Debug.Log("Heuristic Score at Tile " + tile.TileInfo.CellLocation + ": " + newScore.TotalScore());
                if (newScore.TotalScore() > ActionScore.TotalScore()) ActionScore = newScore;
            }*/
        }

        Debug.Log("Best Heuristic Score: " + ActionScore.TotalScore());
        //Debug.Log("Decided Cell Location: " + ActionScore.PotentialCell);
        return ActionScore.TotalScore();
    }

    public override void ActivateAction(Unit unit)
    {
        UnitMenu.HideMenu();
        ActionUtility.ShowSelectableTilesForMove(Area(unit, null));
        ChainSystem.HoldPotentialChain(this, unit);
        MapCursor.ActionState();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        // Have AI Units show their range of movement before moving
        if (unit.GetComponent<EnemyUnit>())
        {
            ActionUtility.ShowSelectableTilesForMove(Area(unit, null));
            yield return new WaitForSeconds(2.0f);
            ActionUtility.HideSelectableTilesForAction(Area(unit, null));
        }
        
        // Spend an Action Point to execute the Action
        --unit.unitInfo.currentAP;
        
        // Remove the Location the Unit is currently at in UnitLocator
        TilemapCreator.UnitLocator.Remove(new Vector2Int(unit.unitInfo.CellLocation.x, unit.unitInfo.CellLocation.z));
        
        // Updates the location as the Unit moves
        yield return unit.unitMovement.Move(unit, selectedCell);
        
        // Adds the location of the tile the Unit ended at in UnitLocator
        TilemapCreator.UnitLocator.Add(new Vector2Int(unit.unitInfo.CellLocation.x, unit.unitInfo.CellLocation.z), unit);

/*        if(unit.unitInfo.UnitAffiliation == UnitAffiliation.Player)
        {
            CanvasUI.ShowTurnUnitInfoDisplay(unit.unitInfo);
            CanvasUI.HideTargetUnitInfoDisplay();
        }
        else
        {
            CanvasUI.HideTurnUnitInfoDisplay();
            CanvasUI.ShowTargetUnitInfoDisplay(unit.unitInfo);
        }*/
        CanvasUI.ShowTurnUnitInfoDisplay(unit.unitInfo);
        CanvasUI.HideTargetUnitInfoDisplay();

        if (!unit.GetComponent<EnemyUnit>()) { MapCursor.currentUnit = selectedCell; }
    }
}
