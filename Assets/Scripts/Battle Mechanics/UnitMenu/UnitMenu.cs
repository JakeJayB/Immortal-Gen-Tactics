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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void DisplayUnitMenu(List<UnitAction> actions)
    {
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
}
