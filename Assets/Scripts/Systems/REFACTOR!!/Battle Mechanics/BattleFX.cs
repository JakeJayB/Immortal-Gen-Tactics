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
        
        Vector2Int startCell = unit.UnitInfo.Vector2CellLocation();
        Vector2Int previousCell = startCell;
        
        for (int i = 1; i <= strength; i++)
        {
            Vector2Int nextCell = startCell + direction * i;

            // Stop the unit from trying to traverse null tile locations
            if (!TileLocator.SelectableTiles.TryGetValue(nextCell, out var tile))
            {
                TilemapCreator.UnitLocator.Remove(startCell);
                yield return UnitMovement.Move(unit, previousCell);
                TilemapCreator.UnitLocator.Add(previousCell, unit);
                
                int damage = DamageCalculator.DamageFixedAmount((int)(5 * (1 + 0.20 * (strength - 1))), unit.UnitInfo);
                SoundFXManager.PlaySoundFXClip("SwordHit", 0.45f);
                yield return DamageDisplay.DisplayUnitDamage(unit, damage);
                
                yield break;
            }
            
            if (TilemapCreator.UnitLocator.TryGetValue(nextCell, out var targetUnit))
            {
                // Remove the Location the Unit is currently at in UnitLocator
                TilemapCreator.UnitLocator.Remove(startCell);
        
                // Updates the location as the Unit moves
                yield return UnitMovement.Move(unit, previousCell);
                
                int damageA = DamageCalculator.DamageFixedAmount((int)(5 * (1 + 0.10 * (strength - 1))), unit.UnitInfo);
                int damageB = DamageCalculator.DamageFixedAmount((int)(5 * (1 + 0.10 * (strength - 1))), targetUnit.UnitInfo);
                SoundFXManager.PlaySoundFXClip("SwordHit", 0.45f);
                yield return DamageDisplay.DisplayUnitDamage(unit, damageA);
                yield return DamageDisplay.DisplayUnitDamage(targetUnit, damageB);
                yield return InflictBlowback(targetUnit, (strength - i) / 2, direction);

                var endLocation = TilemapCreator.UnitLocator.TryGetValue(nextCell, out var stillThere)
                    ? previousCell : nextCell;
                
                if (unit.UnitInfo.Vector2CellLocation() != endLocation) { yield return UnitMovement.Move(unit, endLocation); }
                
                // Adds the location of the tile the Unit ended at in UnitLocator
                TilemapCreator.UnitLocator.Add(endLocation, unit);
                break;
            }

            previousCell = nextCell;

            if (i != strength) continue;
            
            // Remove the Location the Unit is currently at in UnitLocator
            TilemapCreator.UnitLocator.Remove(startCell);
        
            // Updates the location as the Unit moves
            yield return UnitMovement.Move(unit, previousCell);
        
            // Adds the location of the tile the Unit ended at in UnitLocator
            TilemapCreator.UnitLocator.Add(previousCell, unit);
        }
    }
}
