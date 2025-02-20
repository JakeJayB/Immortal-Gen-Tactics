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
    private Sprite slotImage;
    public GameObject Menu { get; private set; }
    public List<GameObject> MenuSlots { get; private set; }

    private const float SLOT_SCALE = 0.5f;
    private const float SLOT_MARGIN = 48f;

    private List<UnitAction> UnitActions = new List<UnitAction>()
    {
        new Move(),
        new Wait(),
        new Move(),
        new Wait()
    };

    private void Awake()
    {
        // Create a new Canvas GameObject
        GameObject canvasObject = new GameObject("Canvas");
        Canvas = canvasObject.AddComponent<Canvas>();
        slotImage = Resources.Load<Sprite>("Sprites/UnitMenu/igt_unit_menu_slot");
    
        // Set proper render mode
        Canvas.renderMode = RenderMode.ScreenSpaceOverlay; // Change to Overlay to avoid 3D interference

        // Add essential UI components
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();

        // Create the menu as a UI object
        Menu = new GameObject("UnitMenu", typeof(RectTransform));
        Menu.transform.SetParent(Canvas.transform, false);
    
        MenuSlots = new List<GameObject>();

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
            GameObject slot = new GameObject("Slot " + i, typeof(RectTransform));
            slot.transform.SetParent(Menu.transform, false); // Ensure proper positioning in UI
        
            // Add Image component instead of SpriteRenderer
            Image image = slot.AddComponent<Image>();
            image.sprite = UnitActions[i].SlotImage(); // Set the sprite as a UI element

            // Positioning the UI element
            RectTransform rectTransform = slot.GetComponent<RectTransform>();
            rectTransform.localScale = new Vector3(SLOT_SCALE, SLOT_SCALE, SLOT_SCALE);
            rectTransform.anchoredPosition = new Vector2(i * SLOT_MARGIN, 0);
        
            MenuSlots.Add(slot);
        }
    }
}
