using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UnitMenu : MonoBehaviour
{
    private Canvas Canvas;
    public static GameObject Menu { get; private set; }
    public static List<MenuSlot> MenuSlots { get; private set; }
    public static UnitMenuTextbox Textbox { get; private set; }
    public static UnitMenuCursor Cursor { get; private set; }

    private void Awake()
    {
        // Create a new Canvas GameObject
        GameObject canvasObject = new GameObject("Canvas");
        Canvas = canvasObject.AddComponent<Canvas>();
    
        // Set proper render mode
        Canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Change to Overlay to avoid 3D interference

        // Add essential UI components
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        // Create the menu as a UI object
        Menu = new GameObject("UnitMenu", typeof(RectTransform));
        Menu.transform.SetParent(Canvas.transform, false);
    }


    // Update is called once per frame
    void Update()
    {
        // if menu  is active and S is pressed, hide the menu and activate the map cursor
        if (Input.GetKeyDown(KeyCode.S) && Menu.gameObject.activeSelf)
        {
            HideMenu();
            MapCursor.ActiveState();
        }
    }

    public static void DisplayUnitMenu(List<UnitAction> actions)
    {
        // if Unit Menu has already been made
        if (Menu.transform.childCount != 0) return;

        MenuSlots = new List<MenuSlot>();
        
        for (int i = 0; i < actions.Count; i++)
        {
            MenuSlot slot = new GameObject("Slot: " + actions[i].Name, typeof(RectTransform)).AddComponent<MenuSlot>();
            slot.transform.SetParent(Menu.transform, false);
        
            // TODO: Makes sure to get actions from actual unit instead of the testing list of UnitActions
            slot.DefineSlot(actions[i]);
            slot.PositionSlot(i);
        
            MenuSlots.Add(slot);
        }
        
        Textbox = new GameObject("UnitMenuTextbox", typeof(RectTransform)).AddComponent<UnitMenuTextbox>();
        Textbox.transform.SetParent(Menu.transform, false);

        Cursor = new GameObject("Cursor", typeof(RectTransform)).AddComponent<UnitMenuCursor>();
        Cursor.transform.SetParent(Menu.transform, false);
        Cursor.InstantiateCursor(MenuSlots);
    }
    
    public static void ShowMenu() { Menu.SetActive(true); }
    public static void HideMenu() { Menu.SetActive(false); }
}
