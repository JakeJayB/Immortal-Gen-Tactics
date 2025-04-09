using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CyclePanel : MonoBehaviour
{
    private static Image Panel;
    void Awake()
    {
        InstantiatePanel();
    }

    private void InstantiatePanel()
    {
        Panel = this.gameObject.AddComponent<Image>();
        Sprite PanelSprite = Resources.Load<Sprite>("Sprites/TurnCycle/Turn_Cycle_Panel3");

        Panel.sprite = PanelSprite;

        RectTransform rectTransform = this.gameObject.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, TurnCycle.PANEL_WIDTH);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TurnCycle.PANEL_HEIGHT);

    }
}
