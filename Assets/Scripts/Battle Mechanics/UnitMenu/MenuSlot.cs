using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuSlot : MonoBehaviour
{
    private const float SLOT_SCALE = 0.5f;
    private const float SLOT_MARGIN = 48f;
    
    public string Name { get; private set; }
    public UnitAction Action { get; private set; }

    public void DefineSlot(UnitAction unitAction)
    {
        Name = unitAction.Name;
        Image image = gameObject.AddComponent<Image>();
        image.sprite = unitAction.SlotImage();
    }

    public void PositionSlot(int slotNumber)
    {
        // Position the UI element
        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.localScale = new Vector3(SLOT_SCALE, SLOT_SCALE, SLOT_SCALE);
        rectTransform.anchoredPosition = new Vector2(slotNumber * SLOT_MARGIN, 0);
    }
}
