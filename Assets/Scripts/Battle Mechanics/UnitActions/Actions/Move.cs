using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : UnitAction
{
    public sealed override string Name { get; protected set; } = "Move";
    public override int Priority { get; protected set; } = 0;
    public override ActionType ActionType { get; protected set; } = ActionType.Move;
    public sealed override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_walk";
    public sealed override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    
    public override void ActivateAction(Unit unit)
    {
        UnitMenu.HideMenu();
        ActionUtility.ShowSelectableTilesForAction(unit, Name);
        ChainSystem.HoldPotentialChain(this, unit);
        MapCursor.ActionState();
    }

    public override IEnumerator ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        // Remove the Location the Unit is currently at in UnitLocator
        TilemapCreator.UnitLocator.Remove(new Vector2Int(unit.unitInfo.CellLocation.x, unit.unitInfo.CellLocation.z));
        
        // Updates the location as the Unit moves
        yield return unit.unitMovement.Move(unit, selectedCell);
        
        // Adds the location of the tile the Unit ended at in UnitLocator
        TilemapCreator.UnitLocator.Add(new Vector2Int(unit.unitInfo.CellLocation.x, unit.unitInfo.CellLocation.z), unit);

        if (!unit.GetComponent<EnemyUnit>()) { MapCursor.currentUnit = selectedCell; }
    }
}
