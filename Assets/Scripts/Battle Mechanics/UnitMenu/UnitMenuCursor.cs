using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMenuCursor : MonoBehaviour
{
    public Image Cursor { get; private set; }
    private Sprite CursorImage;
    private RectTransform CursorRectTransform;

    private List<MenuSlot> MenuSlots;
    public int slotIndex;
    
    // Start is called before the first frame update
    void Start()
    {
        CursorRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            slotIndex = slotIndex - 1 < 0 ? MenuSlots.Count - 1 : (slotIndex - 1) % MenuSlots.Count;
            PositionCursor(MenuSlots[slotIndex]);
            UnitMenuTextbox.UpdateText(MenuSlots[slotIndex].Name);
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            slotIndex = (slotIndex + 1) % MenuSlots.Count;
            PositionCursor(MenuSlots[slotIndex]);
            UnitMenuTextbox.UpdateText(MenuSlots[slotIndex].Name);
        }
    }

    public void InstantiateCursor(List<MenuSlot> menuSlots)
    {
        Cursor = gameObject.AddComponent<Image>();
        CursorImage = Resources.Load<Sprite>("Sprites/UnitMenu/igt_unit_menu_cursor");
        Cursor.sprite = CursorImage;

        MenuSlots = menuSlots;
        slotIndex = 0;

        CursorRectTransform = GetComponent<RectTransform>();
        
        PositionCursor(MenuSlots[slotIndex]);
        UnitMenuTextbox.UpdateText(MenuSlots[slotIndex].Name);
    }

    private void PositionCursor(MenuSlot currentSlot)
    {
        CursorRectTransform.localScale = currentSlot.SlotRectTransform.localScale;
        CursorRectTransform.anchoredPosition = currentSlot.SlotRectTransform.anchoredPosition;
    }
    
}
