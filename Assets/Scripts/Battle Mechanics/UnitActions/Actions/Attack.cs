using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Attack : UnitAction
{
    // Start is called before the first frame update
    public sealed override string Name { get; protected set; } = "Attack";
    public override int APCost { get; protected set; } = 1;
    public override int Priority { get; protected set; } = 1;
    public sealed override ActionType ActionType { get; protected set; } = ActionType.Weapon;
    public sealed override string SlotImageAddress { get; protected set; } = "Sprites/UnitMenu/Slots/igt_attack";

    public sealed override Sprite SlotImage() { return Resources.Load<Sprite>(SlotImageAddress); }
    public override float HeuristicScore(EnemyUnit unit, Vector2Int selectedCell)
    {
        // Check if Target is within the area of the Action
        if (ActionUtility.DetermineParameters("Attack", unit).Item1.Contains(TilemapCreator.TileLocator[selectedCell])) { return -1; }
        
        // Get the TargetUnit
        var targetUnit = TilemapCreator.UnitLocator[selectedCell];
        
        // Return the Heuristic Score
        // [ Expected Damage Dealt * Aggressive Behavior ]
        return (unit.unitInfo.finalAttack - targetUnit.unitInfo.finalDefense) * unit.Aggressive;
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