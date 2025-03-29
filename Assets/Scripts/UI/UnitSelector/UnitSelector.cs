using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public static int PANEL_WIDTH;
    public static int PANEL_HEIGHT;

    public static GameObject Menu { get; private set; }
    private static Unit unitSelected;


    private void Update()
    {
        if(Menu == null || !Menu.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            // get the next unit to the right
            SelectorUnitIcons.NextUnit();
            SoundFXManager.PlaySoundFXClip("Next", 0.5f);
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            // get the previous unit to the left
            SelectorUnitIcons.PreviousUnit();
            SoundFXManager.PlaySoundFXClip("Next", 0.5f);
        }
    }

    public static void Initialize(GameObject canvas)
    {

        PANEL_WIDTH = Mathf.RoundToInt(Display.main.systemWidth * 0.3f);
        PANEL_HEIGHT = Mathf.RoundToInt(Display.main.systemHeight * 0.3f);

        Menu = new GameObject("UnitSelector", typeof(RectTransform));
        Menu.AddComponent<UnitSelector>();
        Menu.transform.SetParent(canvas.transform, false);

        RectTransform rectTransform = Menu.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0, 0);

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, PANEL_WIDTH);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, PANEL_HEIGHT);

        DisplayUnitSelector();
    }

    public static void DisplayUnitSelector()
    {
        if (Menu.transform.childCount != 0) return;

        Menu.SetActive(true);
        var panel = new GameObject("Panel", typeof(RectTransform)).AddComponent<SelectorPanel>();
        panel.transform.SetParent(Menu.transform, false);

        var unitIcon = new GameObject("UnitIcon", typeof(RectTransform)).AddComponent<SelectorUnitIcons>();
        unitIcon.transform.SetParent(Menu.transform, false);

    }

    public static void DestroyMenu()
    {
        if (Menu != null)
        {
            Destroy(Menu);
        }
    }

    public static void ResetUnitSelected()
    {
        if(unitSelected != null)
        {
            unitSelected = null;
            SoundFXManager.PlaySoundFXClip("Deselect", 0.4f);
        }
    }


    public static void PlaceUnit(Vector2Int tileCell)
    {
        // New pseudocode
        // Conditions:
            // 1. if a unit is on the selected tileCell
                // a. get unit from tileCell
                // b. if unitSelected is not null
                    // i. if unitSelected is the same as the unit on the selected tileCell
                        // 1. Delete unit from tileCell
                        // 2. signal to SelectorUnitIcons that the unit is no longer active
                        // 3. set unitSelected to null
                    // ii. else
                        // 1. swap unit locations
                        // 2. set unitSelected to null
                // c. else if unitSelected is null
                    // i. set unitSelected to unit on the selected tileCell
            // 2. if no unit is on the selected tileCell
                // a. if unitSelected is not null
                    // i. place already existing unit on the selected tileCell
                    // ii. set unitSelected to null
                // b. else if unitSelected is null
                    // i. get unit from SelectorUnitIcons based on currentIdx
                    // ii. place unit on the selected tileCell
                    // iii. signal to SelectorUnitIcons that the unit is now active


        if(TilemapCreator.UnitLocator.TryGetValue(tileCell, out var unit))
        {
            if (unitSelected != null) // if a unit was previously selected
            {
                if(unitSelected == unit) // if the previously selected unit is the same as the unit on tileCell
                {
                    // Deleting/deactivating unit
                    TilemapCreator.UnitLocator.Remove(tileCell);
                    SelectorUnitIcons.DeactivateUnit(unitSelected);
                    SoundFXManager.PlaySoundFXClip("Deselect", 0.4f);
                    unitSelected = null;
                }
                else // if the previously selected unit is different from the unit on tileCell
                {
                    // get unit locations
                    Vector3Int unit1Location = unitSelected.unitInfo.CellLocation;
                    Vector3Int unit2Location = unit.unitInfo.CellLocation;

                    // Remove old locations from UnitLocator
                    TilemapCreator.UnitLocator.Remove(unitSelected.unitInfo.Vector2CellLocation()); // unitSelected
                    TilemapCreator.UnitLocator.Remove(unit.unitInfo.Vector2CellLocation()); // unit

                    // Swap locations
                    Vector3Int tempCell = unit1Location;
                    unitSelected.unitInfo.CellLocation = unit2Location;
                    unit.unitInfo.CellLocation = tempCell;

                    unitSelected.unitRenderer.PositionUnit(unit2Location);
                    unit.unitRenderer.PositionUnit(tempCell);

                    // Add new locations to UnitLocator
                    TilemapCreator.UnitLocator.Add(unitSelected.unitInfo.Vector2CellLocation(), unitSelected);
                    TilemapCreator.UnitLocator.Add(unit.unitInfo.Vector2CellLocation(), unit);

                    SoundFXManager.PlaySoundFXClip("Select", 0.2f);
                    unitSelected = null;
                }
            }
            else // if no unit was previously selected
            {
                // set unitSelected to the unit on tileCell
                unitSelected = unit;
                SoundFXManager.PlaySoundFXClip("Select", 0.2f);

            }
        }
        else // if no unit is on the selected tileCell
        {
            if(unitSelected != null) // if a unit was previously selected
            {
                // Set current and new locations
                Vector3Int currLocation = unitSelected.unitInfo.CellLocation;
                Vector3Int newLocation = TilemapCreator.TileLocator[tileCell].TileInfo.CellLocation + Vector3Int.up;

                // Placing Unit to new location
                TilemapCreator.UnitLocator.Remove(new Vector2Int(currLocation.x, currLocation.z));
                unitSelected.unitInfo.CellLocation = newLocation;
                unitSelected.unitRenderer.PositionUnit(newLocation);
                TilemapCreator.UnitLocator.Add(unitSelected.unitInfo.Vector2CellLocation(), unitSelected);
                SoundFXManager.PlaySoundFXClip("Select", 0.2f);

                unitSelected = null;
            }
            else // if no unit was previously selected
            {
                Vector3Int newLocation = TilemapCreator.TileLocator[tileCell].TileInfo.CellLocation + Vector3Int.up;

                // adding new unit to the selected tileCell
                unit = SelectorUnitIcons.GetUnit(tileCell);
                if(unit != null)
                {
                    // Placing Unit to new location
                    unit.unitInfo.CellLocation = newLocation;
                    unit.unitRenderer.PositionUnit(newLocation);
                    TilemapCreator.UnitLocator.Add(unit.unitInfo.Vector2CellLocation(), unit);

                    // signal to SelectorUnitIcons that the new unit is now active
                    SelectorUnitIcons.ActivateCurrentUnit();
                    SoundFXManager.PlaySoundFXClip("Select", 0.2f);

                }
                else
                {
                    SoundFXManager.PlaySoundFXClip("Deselect", 0.4f);
                }
            }

        }

    }

}
