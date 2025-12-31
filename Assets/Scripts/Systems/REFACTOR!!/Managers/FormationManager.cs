using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

namespace IGT.Systems
{
    public class FormationManager : MonoBehaviour
    {
        private const int FORMATION_MAX_LIMIT = 4;
        private static Unit[] Formation = new Unit[FORMATION_MAX_LIMIT];

        // Start is called before the first frame update
        void Start()
        {
            Formation = new Unit[FORMATION_MAX_LIMIT];
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public static void PlaceUnitOnTile(Unit unit)
        {
            Vector2Int tileCell = MapCursor.hoverCell;
            Vector3Int newLocation = TileLocator.SelectableTiles[tileCell].TileInfo.CellLocation + Vector3Int.up;

            // adding new unit to the selected tileCell
            if (unit.GameObj)
            {
                // Placing Unit to new location
                unit.UnitInfo.CellLocation = newLocation;
                SpriteRenderer spriteRenderer = unit.GameObj.GetComponent<SpriteRenderer>();
                UnitRenderer unitRenderer = new UnitRenderer(unit, spriteRenderer);
                UnitTransform.PositionUnit(unitRenderer, newLocation);
                TilemapCreator.UnitLocator.Add(unit.UnitInfo.Vector2CellLocation(), unit);
                unit.GameObj.SetActive(true);
                AddUnitToFormation(unit);

                SoundFXManager.PlaySoundFXClip("Select", 0.2f);

            }
            else
            {
                SoundFXManager.PlaySoundFXClip("Deselect", 0.4f);
            }
        }

        public static void RemoveUnitOnTile()
        {
            Vector2Int tileCell = MapCursor.hoverCell;

            if (TilemapCreator.UnitLocator.TryGetValue(tileCell, out var unit))
            {
                unit.GameObj.SetActive(false);
                TilemapCreator.UnitLocator.Remove(tileCell);
                RemoveUnitFromFormation(unit);
            }
        }

        private static void AddUnitToFormation(Unit unit)
        {
            for (int i = 0; i < Formation.Length; i++) {
                if (Formation[i] == null)
                {
                    Formation[i] = unit;
                    return;
                }
            }

            Debug.LogError("Unit could not be added to the Formation.");
        }

        private static void RemoveUnitFromFormation(Unit unit)
        {
            for (int i = 0; i < Formation.Length; i++)
            {
                if (Formation[i] == unit)
                {
                    Formation[i] = null;
                    return;
                }
            }

            Debug.LogError("Unit does not exist in the Formation.");
        }

        public static bool FormationHasAtLeastOneUnit()
        {
            for (int i = 0; i < Formation.Length; i++)
            {
                if (Formation[i] != null) return true;
            }

            return false;
        }
    }
}
