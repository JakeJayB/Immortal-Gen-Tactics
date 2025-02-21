using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UnitMenu : MonoBehaviour
{
    private Canvas Canvas;
    public GameObject Menu { get; private set; }
    public List<MenuSlot> MenuSlots { get; private set; }
    public UnitMenuTextbox Textbox { get; private set; }

    // FOR TESTING DisplayUnitMenu()
    private List<UnitAction> UnitActions = new List<UnitAction>()
    {
        new Move(),
        new Attack(),
        new Item(),
        new Wait()
    };

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
    
        MenuSlots = new List<MenuSlot>();
        Textbox = new GameObject("UnitMenuTextbox", typeof(RectTransform)).AddComponent<UnitMenuTextbox>();
        Textbox.transform.SetParent(Menu.transform, false);
        Textbox.Text.transform.SetParent(Menu.transform, false);

        DisplayUnitMenu();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            var vector3 = Menu.transform.localPosition;
            vector3.y = (Menu.transform.localPosition.y + 1);
            Menu.transform.localPosition = vector3;
        }
        
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            var vector3 = Menu.transform.localPosition;
            vector3.y = (Menu.transform.localPosition.y - 1);
            Menu.transform.localPosition = vector3;
        }
    }

    private void DisplayUnitMenu()
    {
        for (int i = 0; i < 4; i++)
        {
            MenuSlot slot = new GameObject("Slot: " + UnitActions[i].Name, typeof(RectTransform)).AddComponent<MenuSlot>();
            slot.transform.SetParent(Menu.transform, false);
        
            // TODO: Makes sure to get actions from actual unit instead of the testing list of UnitActions
            slot.DefineSlot(UnitActions[i]);
            slot.PositionSlot(i);
        
            MenuSlots.Add(slot);
        }

        Image TextboxImage = Textbox.AddComponent<Image>();
        TextboxImage.sprite = Textbox.TextboxImage;
        TextboxImage.SetNativeSize();
        
        RectTransform rectTransform = Textbox.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(73, -40);
        
        RectTransform textRectTransform = Textbox.Text.GetComponent<RectTransform>();
        textRectTransform.anchoredPosition = new Vector2(73, -40);
    }
}
