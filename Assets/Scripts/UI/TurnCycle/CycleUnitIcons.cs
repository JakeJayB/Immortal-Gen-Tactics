using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CycleUnitIcons : MonoBehaviour
{
    private static Dictionary<Unit, Image> iconDict;

    private static GameObject instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this.gameObject;
        }
        else
        {
            Destroy(this);
        }

        iconDict = new Dictionary<Unit, Image>();

        RectTransform rectTransform = instance.GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, TurnCycle.PANEL_WIDTH);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, TurnCycle.PANEL_HEIGHT);
    }

    public static void InitializeUnits(List<Unit> units)
    {
        float posOffset = TurnCycle.PANEL_WIDTH / (units.Count + 1);

        for(int i = 0; i < units.Count; i++)
        {
            Unit currUnit = units[i];
            Image image = new GameObject("Unit" + (i + 1), typeof(RectTransform)).AddComponent<Image>();
            image.transform.SetParent(instance.transform, false);
            image.sprite = currUnit.unitInfo.sprite;

            image.rectTransform.anchorMin = new Vector2(0, 0.5f);
            image.rectTransform.anchorMax = new Vector2(0, 0.5f);
            image.rectTransform.anchoredPosition = new Vector2((i + 1) * posOffset, 0);

            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 40);
            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 40);
            iconDict[currUnit] = image;
        }
        
    }

    public static void RearrangeUnits(List<Unit> units)
    {
        float posOffset = TurnCycle.PANEL_WIDTH / (units.Count + 1);

        for (int i = 0; i < units.Count; i++)
        {
            Unit currUnit = units[i];
            Image image = iconDict[currUnit];
            image.rectTransform.anchoredPosition = new Vector2((i + 1) * posOffset, 0);
        }
    }

    public static void RemoveUnit(Unit unit)
    {
        iconDict.Remove(unit);
    }
}
