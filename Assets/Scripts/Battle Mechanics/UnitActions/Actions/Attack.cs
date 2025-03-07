using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : UnitAction
{
    // Start is called before the first frame update
    public sealed override string Name { get; protected set; } = "Attack";
    public override int Priority { get; protected set; }
    public sealed override ActionType ActionType { get; protected set; } = ActionType.Weapon;
    public sealed override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_attack";

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
        Vector2Int originCell = new Vector2Int(unit.unitInfo.CellLocation.x, unit.unitInfo.CellLocation.z);
        Vector2Int displacement = selectedCell - originCell;

        Vector2Int direction = new Vector2Int(Mathf.Clamp(displacement.x, -1, 1), Mathf.Clamp(displacement.y, -1, 1));
        int numOfCells = Mathf.Max(Mathf.Abs(displacement.x), Mathf.Abs(displacement.y));

        for (int i = 1; i <= numOfCells; i++)
        {
            Vector2Int nextCell = originCell + direction * i;
            if (TilemapCreator.UnitLocator.TryGetValue(nextCell, out var targetUnit))
            {
                targetUnit.unitInfo.finalHP -= unit.unitInfo.finalAttack;
                Debug.Log("Attack: unit attacked!");
            }
        }
        
        yield return null;
    }
}