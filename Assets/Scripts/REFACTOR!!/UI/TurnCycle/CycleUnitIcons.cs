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

    public static void Clear()
    {
        iconDict = null;
        instance = null;
    }

    public static void RegisterCleanup()
    {
        MemoryManager.AddListeners(Clear);
    }


    public static void InitializeUnits(List<Unit> units)
    {
        float posOffset = TurnCycle.PANEL_WIDTH / (units.Count + 1);

        for(int i = 0; i < units.Count; i++)
        {
            Unit currUnit = units[i];
            Image image = new GameObject("Unit" + (i + 1), typeof(RectTransform)).AddComponent<Image>();
            image.transform.SetParent(instance.transform, false);
            image.sprite = currUnit.UnitRenderer.Sprite();

            image.rectTransform.anchorMin = new Vector2(0, 0.5f);
            image.rectTransform.anchorMax = new Vector2(0, 0.5f);
            image.rectTransform.anchoredPosition = new Vector2((i + 1) * posOffset, 0);

            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 30);
            image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);
            iconDict[currUnit] = image;
        }
        
    }

    public static void RearrangeUnits(List<Unit> units)
    {
        if(iconDict.Count == 0)
        {
            InitializeUnits(units);
            return;
        }

        float posOffset = TurnCycle.PANEL_WIDTH / (units.Count + 1);

        for (int i = 0; i < units.Count; i++)
        {
            Unit currUnit = units[i];
            Image image = iconDict[currUnit];
            image.rectTransform.anchoredPosition = new Vector2((i + 1) * posOffset, 0);
        }
    }

    public static void AddUnit(Unit unit)
    {
        if (iconDict.ContainsKey(unit))
        {
            Debug.LogError("CycleUnitIcons: Unit already found in icon dictionary");
            return;
        }
        
        float posOffset = TurnCycle.PANEL_WIDTH / (iconDict.Count + 1);
        
        Unit currUnit = unit;
        Image image = new GameObject("Unit" + (iconDict.Count + 1), typeof(RectTransform)).AddComponent<Image>();
        image.transform.SetParent(instance.transform, false);
        image.sprite = currUnit.UnitRenderer.Sprite();

        image.rectTransform.anchorMin = new Vector2(0, 0.5f);
        image.rectTransform.anchorMax = new Vector2(0, 0.5f);
        image.rectTransform.anchoredPosition = new Vector2((iconDict.Count + 1) * posOffset, 0);

        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 30);
        image.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 30);
        iconDict[currUnit] = image;
    }
    
    public static void RemoveUnit(Unit unit)
    {
        if (!iconDict.TryGetValue(unit, out var image))
        {
            Debug.LogError("CycleUnitIcons: Unit not found in icon dictionary");
            return;
        }

        Destroy(image.gameObject);
        iconDict.Remove(unit);
    }
}
