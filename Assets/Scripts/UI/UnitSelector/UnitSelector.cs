using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

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
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            // get the previous unit to the left
        }
    }

    public static void Initialize(GameObject canvas)
    {
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
        Menu.SetActive(true);
        var panel = new GameObject("Panel", typeof(RectTransform)).AddComponent<SelectorPanel>();
        panel.transform.SetParent(Menu.transform, false);
    }
}
