using System.Collections;
using UnityEngine;

namespace IGT.Systems
{
    public class BlowbackBFX : InstantBattleFX
    {
        public override string Name { get; } = "Blowback";
        public int Strength { get; }
        private Vector2Int Direction;

        public BlowbackBFX(int strength, Vector2Int direction) {
            Strength = strength;
            Direction = direction;
        }
        
        public override IEnumerator Inflict(Unit unit) {
            if (Strength < 1) { yield break; }
            
            Vector2Int startCell = unit.UnitInfo.Vector2CellLocation();
            Vector2Int previousCell = startCell;

            for (int i = 1; i <= Strength; i++)
            {
                Vector2Int nextCell = startCell + Direction * i;

                // Stop the unit from trying to traverse null tile locations
                if (!TileLocator.SelectableTiles.TryGetValue(nextCell, out var tile))
                {
                    TilemapCreator.UnitLocator.Remove(startCell);
                    yield return UnitMovement.Move(unit, previousCell);
                    TilemapCreator.UnitLocator.Add(previousCell, unit);

                    int damage =
                        DamageCalculator.DealFixedHPDamage((int)(5 * (1 + 0.20 * (Strength - 1))), unit.UnitInfo);
                    SoundFXManager.PlaySoundFXClip("SwordHit", 0.45f);
                    yield return DamageDisplay.DisplayUnitDamage(unit, damage);

                    yield break;
                }

                if (TilemapCreator.UnitLocator.TryGetValue(nextCell, out var target)) {
                    // Remove the Location the Unit is currently at in UnitLocator
                    TilemapCreator.UnitLocator.Remove(startCell);

                    // Updates the location as the Unit moves
                    yield return UnitMovement.Move(unit, previousCell);

                    int damageA =
                        DamageCalculator.DealFixedHPDamage((int)(5 * (1 + 0.10 * (Strength - 1))), unit.UnitInfo);
                    int damageB = DamageCalculator.DealFixedHPDamage((int)(5 * (1 + 0.10 * (Strength - 1))),
                        target.UnitInfo);
                    SoundFXManager.PlaySoundFXClip("SwordHit", 0.45f);
                    yield return DamageDisplay.DisplayUnitDamage(unit, damageA);
                    yield return DamageDisplay.DisplayUnitDamage(target, damageB);
                    yield return new BlowbackBFX((Strength - i) / 2, Direction).Inflict(target);

                    var endLocation = TilemapCreator.UnitLocator.TryGetValue(nextCell, out var stillThere)
                        ? previousCell
                        : nextCell;

                    if (unit.UnitInfo.Vector2CellLocation() != endLocation) {
                        yield return UnitMovement.Move(unit, endLocation);
                    }

                    // Adds the location of the tile the Unit ended at in UnitLocator
                    TilemapCreator.UnitLocator.Add(endLocation, unit);
                    break;
                }

                previousCell = nextCell;

                if (i != Strength) continue;

                // Remove the Location the Unit is currently at in UnitLocator
                TilemapCreator.UnitLocator.Remove(startCell);

                // Updates the location as the Unit moves
                yield return UnitMovement.Move(unit, previousCell);

                // Adds the location of the tile the Unit ended at in UnitLocator
                TilemapCreator.UnitLocator.Add(previousCell, unit);
            }
        }
    }
}
