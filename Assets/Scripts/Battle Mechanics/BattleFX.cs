using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleFX : MonoBehaviour
{
    public static void InflictBFX()
    {
        
    }

    public static IEnumerator InflictBlowback(Unit unit, int strength, Vector2Int direction)
    {
        if (strength < 1) { yield break; }
        
        Vector2Int startCell = unit.unitInfo.Vector2CellLocation();
        Vector2Int previousCell = startCell;
        
        for (int i = 1; i <= strength; i++)
        {
            Vector2Int nextCell = startCell + direction * i;

            // Stop the unit from trying to traverse null tile locations
            if (!TilemapCreator.TileLocator.TryGetValue(nextCell, out var tile))
            {
                TilemapCreator.UnitLocator.Remove(startCell);
                yield return unit.unitMovement.Move(unit, previousCell);
                TilemapCreator.UnitLocator.Add(previousCell, unit);
                yield break;
            }
            
            if (TilemapCreator.UnitLocator.TryGetValue(nextCell, out var targetUnit))
            {
                // Remove the Location the Unit is currently at in UnitLocator
                TilemapCreator.UnitLocator.Remove(startCell);
        
                // Updates the location as the Unit moves
                yield return unit.unitMovement.Move(unit, previousCell);
                
                int damage = DamageCalculator.DamageFixedAmount((int)(5 * (1 + 0.15 * (strength - 1))), unit.unitInfo);
                SoundFXManager.PlaySoundFXClip("SwordHit", 0.45f);
                yield return DamageDisplay.DisplayUnitDamage(targetUnit.unitInfo, damage);
                Debug.Log("Attack: unit attacked! HP: " + targetUnit.unitInfo.currentHP + "/" + targetUnit.unitInfo.FinalHP);
                yield return InflictBlowback(targetUnit, (strength - i) / 2, direction);
                
                // Adds the location of the tile the Unit ended at in UnitLocator
                TilemapCreator.UnitLocator.Add(previousCell, unit);
                break;
            }

            previousCell = nextCell;

            if (i != strength) continue;
            
            // Remove the Location the Unit is currently at in UnitLocator
            TilemapCreator.UnitLocator.Remove(startCell);
        
            // Updates the location as the Unit moves
            yield return unit.unitMovement.Move(unit, previousCell);
        
            // Adds the location of the tile the Unit ended at in UnitLocator
            TilemapCreator.UnitLocator.Add(previousCell, unit);
        }
    }
}
