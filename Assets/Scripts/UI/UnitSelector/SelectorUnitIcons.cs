using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SelectorUnitIcons : MonoBehaviour
{
    private static int BACKGROUND_WIDTH = Mathf.RoundToInt(UnitSelector.PANEL_WIDTH * 0.3f);
    private static int BACKGROUND_HEIGHT = Mathf.RoundToInt(UnitSelector.PANEL_HEIGHT * 0.7f);
    private static Image unitImage;


    //private static Dictionary<int, Vector2Int> activeUnits = new Dictionary<int, Vector2Int>();
    private static Dictionary<int, Unit> activeUnits;
    private static int currentIdx;

    void Awake()
    {
        InstantiateUnitPanel();
    }

    private void InstantiateUnitPanel()
    {
        activeUnits = new Dictionary<int, Unit>();
        currentIdx = 0;



        // Create background panel for unit icons
        GameObject background = new GameObject("Background", typeof(RectTransform));
        background.transform.SetParent(this.transform);

        RectTransform backgroundRectTransform = background.GetComponent<RectTransform>();
        backgroundRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, BACKGROUND_WIDTH);
        backgroundRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, BACKGROUND_HEIGHT);

        background.AddComponent<Image>();
        background.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/UnitSelector/Unit_Icon_Background");

        // Get sprite of the 0th unit in the list
        // Create and store a new GameObject for the unit icon
        GameObject icon = new GameObject("Icon", typeof(RectTransform));
        icon.transform.SetParent(this.transform);

        RectTransform unitIconRectTransform = icon.GetComponent<RectTransform>();
        unitIconRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, BACKGROUND_WIDTH * 0.6f);
        unitIconRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, BACKGROUND_HEIGHT * 0.6f);

        unitImage = icon.AddComponent<Image>();
        unitImage.sprite = PartyManager.unitList[currentIdx].unitInfo.sprite;
    }

    public static void NextUnit()
    {
        // increment currentIdx
        // if currentIdx is greater than the number of units, set currentIdx to 0

        // get unit from PartyManager.cs at currentIdx
        // set unitImage to new unit's icon

        // if currentIdx is in activeUnits, shade unitIcon

        currentIdx++;
        if (currentIdx >= PartyManager.unitList.Count)
            currentIdx = 0;

        Debug.Log($"Current Idx: {currentIdx}");

        unitImage.sprite = PartyManager.unitList[currentIdx].unitInfo.sprite;

        if (activeUnits.ContainsKey(currentIdx))
            ShadeUnitIcon();
        else
            UnshadeUnitIcon();
    }

    public static void PreviousUnit()
    {
        // decrement currentIdx
        // if currentIdx is less than 0, set currentIdx to the number of units - 1

        // get unit from PartyManager.cs at currentIdx
        // set unitIcon to new unit's icon

        currentIdx--;
        if (currentIdx < 0)
            currentIdx = PartyManager.unitList.Count - 1;

        Debug.Log($"Current Idx: {currentIdx}");

        unitImage.sprite = PartyManager.unitList[currentIdx].unitInfo.sprite;

        if (activeUnits.ContainsKey(currentIdx))
            ShadeUnitIcon();
        else
            UnshadeUnitIcon();
    }



    public static void isUnitActive()
    {
        // if currentIdx is in activeUnits, return true
        // else return false
    }




    public static void GetUnit(Vector2Int cellLocation)
    {
        // if currentIdx is in activeUnits, 

        // get the unit from PartyManager.cs at currentIdx
        // Add currentIdx to activeUnits, set to value cellLocation
        // shade unitIcon
    }


    public static void ActivateUnit()
    {
        activeUnits.Add(currentIdx, PartyManager.unitList[currentIdx]);
        ShadeUnitIcon();
    }

    public static void DeactivateUnit()
    {
        activeUnits.Remove(currentIdx);
        UnshadeUnitIcon();
    }

    public static void ShadeUnitIcon()
    {
        // shade unitIcon at currentIdx
        if (unitImage == null) return;

        unitImage.color = Color.HSVToRGB(0, 0, 0.7f);
    }

    public static void UnshadeUnitIcon()
    {
        // unshade unitIcon at currentIdx
        if (unitImage == null) return;
        unitImage.color = Color.HSVToRGB(0, 0, 1);
    }
}
