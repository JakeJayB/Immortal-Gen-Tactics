using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Evade : UnitAction
{
    public override string Name { get; protected set; } = "Evade";
    public override int MPCost { get; protected set; } = 0;
    public override int APCost { get; protected set; } = 1;
    public override int Priority { get; protected set; } = 0;
    public override DamageType DamageType { get; protected set; } = DamageType.None;
    public override int BasePower { get; protected set; } = 0;
    public override ActionType ActionType { get; protected set; } = ActionType.React;
    public override Pattern AttackPattern { get; protected set; } = Pattern.None;
    public override int Range { get; protected set; } = 0;
    public override AIActionScore ActionScore { get; protected set; }
    public override int Splash { get; protected set; }
    public override List<Tile> Area(Unit unit, Vector3Int? hypoCell) {
        return Rangefinder.GetMoveTilesInRange(TilemapCreator.TileLocator[unit.unitInfo.Vector2CellLocation()],
            unit.unitInfo.finalEvade);
    }

    public override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_walk";
    public override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }

    public override float CalculateActionScore(EnemyUnit unit, Vector2Int selectedCell)
    {
        throw new System.NotImplementedException();
    }

    public override void ActivateAction(Unit unit) {
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
        
        CanvasUI.ShowTargetUnitInfoDisplay(unit.unitInfo);

        if (!unit.GetComponent<EnemyUnit>()) { MapCursor.currentUnit = selectedCell; }
    }
}
