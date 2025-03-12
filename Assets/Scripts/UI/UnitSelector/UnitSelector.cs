using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public static int PANEL_WIDTH;
    public static int PANEL_HEIGHT;

    private static Canvas Canvas;
    public static GameObject Menu { get; private set; }
    public static List<MenuSlot> MenuSlots { get; private set; }

    private void Update()
    {
        if(Menu == null || !Menu.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.C))
        {
            // get the next unit to the right
            SelectorUnitIcons.NextUnit();
        }
        else if (Input.GetKeyDown(KeyCode.Z))
        {
            // get the previous unit to the left
            SelectorUnitIcons.PreviousUnit();
        }
    }

    public static void Initialize(GameObject canvas)
    {

        PANEL_WIDTH = Mathf.RoundToInt(Display.main.systemWidth * 0.3f);
        PANEL_HEIGHT = Mathf.RoundToInt(Display.main.systemHeight * 0.3f);

        Canvas = canvas.GetComponent<Canvas>();
        Menu = new GameObject("UnitSelector", typeof(RectTransform));
        Menu.AddComponent<UnitSelector>();
        Menu.transform.SetParent(canvas.transform, false);

        RectTransform rectTransform = Menu.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0, 0);
        rectTransform.anchorMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0, 0);

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, PANEL_WIDTH);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, PANEL_HEIGHT);

        Menu.SetActive(false);

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









        // old pseudocode
        // if currentIdx from SelectorUnitIcon is in activeIdx
            // get unit based on currentIdx

            // if another unit is already in tileCell
                // swap unit locations. update other unit's activeIdx value
            // else
                // move unit to tileCell
        // else
            // get unitInfo from SelectorUnitIcon
            // Create unit object based on unitInfo
            // if another unit is already in tileCell
                // Find and destroy unit based on tileCell. unshade unit icon
                // move unit to tileCell
            // else
                // move unit to tileCell
    }

}
