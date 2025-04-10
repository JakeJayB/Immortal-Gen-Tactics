using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor;
using UnityEngine;

public class TurnCycle : MonoBehaviour
{
    public static int PANEL_WIDTH;
    public static int PANEL_HEIGHT;

    private static GameObject Menu;
    private static CycleUnitIcons unitIcons;


    public static void Initialize(GameObject canvas)
    {
        PANEL_WIDTH = 800;
        PANEL_HEIGHT = 80;

        Menu = new GameObject("TurnCycle", typeof(RectTransform));
        Menu.AddComponent<TurnCycle>();
        Menu.transform.SetParent(canvas.transform, false);

        RectTransform rectTransform = Menu.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0);
        rectTransform.anchorMax = new Vector2(0.5f, 0);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        rectTransform.anchoredPosition = new Vector2(0, 40);

        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, PANEL_WIDTH);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, PANEL_HEIGHT);
    }

    private static void DisplayTurnCycle()
    {
        var panel = new GameObject("Panel", typeof(RectTransform)).AddComponent<CyclePanel>();
        panel.transform.SetParent(Menu.transform, false);

        unitIcons = new GameObject("Units", typeof(RectTransform)).AddComponent<CycleUnitIcons>();
        unitIcons.transform.SetParent(Menu.transform, false);
    }

    public static void InitializeCycle(List<Unit> unitList)
    {
        if (Menu.transform.childCount <= 0) DisplayTurnCycle();
        CycleUnitIcons.InitializeUnits(unitList);
    }

    public static void CycleUnits(List<Unit> unitList) => CycleUnitIcons.RearrangeUnits(unitList);
    
    public static void RemoveUnit(Unit unit) => CycleUnitIcons.RemoveUnit(unit);
}
