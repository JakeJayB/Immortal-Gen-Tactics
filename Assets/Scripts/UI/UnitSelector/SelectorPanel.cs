using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectorPanel : MonoBehaviour
{
    private static Image Panel;

    void Awake()
    {
        InstantiatePanel();
    }

    private void InstantiatePanel()
    {
        Panel = this.gameObject.AddComponent<Image>();
        Sprite PanelSprite = Resources.Load<Sprite>("Sprites/UnitSelector/Unit_Selector_Panel");

        Panel.sprite = PanelSprite;

        RectTransform rectTransform = this.gameObject.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, UnitSelector.PANEL_WIDTH);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, UnitSelector.PANEL_HEIGHT);

    }
}
