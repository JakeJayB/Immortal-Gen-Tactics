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
        yield return unit.unitMovement.Move(unit, selectedCell);

        unit.unitInfo.CellLocation = TilemapCreator.TileLocator[selectedCell].TileInfo.CellLocation;
        TilemapCreator.UnitLocator.Remove(new Vector2Int(unit.unitInfo.CellLocation.x, unit.unitInfo.CellLocation.z));
        TilemapCreator.UnitLocator.Add(selectedCell, unit);

        if (!unit.GetComponent<EnemyUnit>()) { MapCursor.currentUnit = selectedCell; }
    }
}
