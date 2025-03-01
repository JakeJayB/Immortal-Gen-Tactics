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

    public override void ExecuteAction(Unit unit, Vector2Int selectedCell)
    {
        
        unit.transform.position = TilemapCreator.TileLocator[selectedCell].TileObj.transform.localPosition + new Vector3(0, 0.3f, 0);
        unit.unitInfo.CellLocation = TilemapCreator.TileLocator[selectedCell].TileInfo.CellLocation + Vector3Int.up;

        TilemapCreator.UnitLocator.Remove(MapCursor.currentUnit);
        TilemapCreator.UnitLocator.Add(selectedCell, unit);

        MapCursor.currentUnit = selectedCell;
    }
}
