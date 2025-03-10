using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UnitSelector : MonoBehaviour
{
    public static int PANEL_WIDTH = 250;
    public static int PANEL_HEIGHT = 150;

    private static Canvas Canvas;
    public static GameObject Menu { get; private set; }
    public static List<MenuSlot> MenuSlots { get; private set; }

    private void Update()
    {
        if(Menu == null || !Menu.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.Z))
        {
            // get the next unit to the right
            SelectorUnitIcons.NextUnit();
        }
        else if (Input.GetKeyDown(KeyCode.C))
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

        // Initialize Unit Icons
        // Create SelectorIcons GameObject

    }

    public static void PlaceUnit(Vector2Int tileCell)
    {
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
