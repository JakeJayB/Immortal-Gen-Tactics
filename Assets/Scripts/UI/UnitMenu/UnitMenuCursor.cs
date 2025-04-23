using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMenuCursor : MonoBehaviour
{
    public Image Cursor { get; private set; }
    private Sprite CursorImage;
    private RectTransform CursorRectTransform;

    private static List<MenuSlot> MenuSlots;
    public static int slotIndex;
    
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
            SoundFXManager.PlaySoundFXClip("UnitMenuCursor", 1f);
            PositionCursor(MenuSlots[slotIndex]);
            UnitMenuTextbox.UpdateText(MenuSlots[slotIndex].Name);
        }
        
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            slotIndex = (slotIndex + 1) % MenuSlots.Count;
            SoundFXManager.PlaySoundFXClip("UnitMenuCursor", 1f);
            PositionCursor(MenuSlots[slotIndex]);
            UnitMenuTextbox.UpdateText(MenuSlots[slotIndex].Name);
        }

        if (Input.GetKeyDown(KeyCode.A) && MenuSlots[slotIndex].Selectable)
        {
            if (ChainSystem.ReactionInProgress) { }
            MenuSlots[slotIndex].Action.ActivateAction(TilemapCreator.UnitLocator[MapCursor.currentUnit]);
            SoundFXManager.PlaySoundFXClip("Select", 0.2f);
        }
    }

    public void InstantiateCursor(List<MenuSlot> menuSlots)
    {
        Cursor = gameObject.AddComponent<Image>();
        CursorImage = Resources.Load<Sprite>("Sprites/UnitMenu/igt_unit_menu_cursor");
        Cursor.sprite = CursorImage;
        
        ResetCursor(menuSlots);
    }

    private void PositionCursor(MenuSlot currentSlot)
    {
        CursorRectTransform.localScale = currentSlot.SlotRectTransform.localScale;
        CursorRectTransform.anchoredPosition = currentSlot.SlotRectTransform.anchoredPosition;
    }

    public void ResetCursor(List<MenuSlot> menuSlots)
    {
        // Get the New Menu Slots and Reset the Index
        MenuSlots = menuSlots;
        slotIndex = 0;

        CursorRectTransform = GetComponent<RectTransform>();

        // Reset Cursor Back to the First Slot and Update the Textbox
        if(menuSlots != null)
        {
            PositionCursor(MenuSlots[slotIndex]);
            UnitMenuTextbox.UpdateText(MenuSlots[slotIndex].Name);
        }

        // Set the Cursor to be above all slots
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }

    public static UnitAction GetSelectedAction() { return MenuSlots[slotIndex].Action; }
}
