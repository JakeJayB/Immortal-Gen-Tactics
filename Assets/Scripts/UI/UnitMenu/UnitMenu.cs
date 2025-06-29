using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UnitMenu : MonoBehaviour
{
    // Size of the UnitMenu that Doesn't Need Slot Positioning Adjustments
    public const int STANDARD_MENU_SIZE = 4;
    
    private static Canvas Canvas;
    public static GameObject Menu { get; private set; }
    public static List<MenuSlot> MenuSlots { get; private set; }
    public static UnitMenuTextbox Textbox { get; private set; }
    public static UnitMenuCursor Cursor { get; private set; }
    public static Camera MainCamera;
    public static UnitAction SubMenu;
    public static bool InSubMenu = false;
    public static bool InReactionMode = false;



    // Update is called once per frame
    void Update()
    {
        // if(!Menu) return;

        // TODO: Place this control in the MapCursor so we don't have this option available during Reactions
        // if menu is active and S is pressed, hide the menu and activate the map cursor
        if (Input.GetKeyDown(KeyCode.S) && Menu.activeSelf)
        {
            if (InSubMenu) {
                InSubMenu = false;
                SubMenu = null;
                DisplayUnitMenu(TilemapCreator.UnitLocator[MapCursor.currentUnit]);
            }
            else {
                HideMenu();
                InfoBar.HideInfo();
                if (ChainSystem.ReactionInProgress)
                {
                    MapCursor.InactiveState();
                    ChainSystem.ReactionInProgress = false;
                }
                else
                {
                    MapCursor.ActiveState();
                }
                    
            }
            SoundFXManager.PlaySoundFXClip("Deselect", 0.4f);
        }
    }

    public static void Clear()
    {
        Menu = null;
        MenuSlots = null;
        Textbox = null;
        Cursor = null;
        MainCamera = null;
        Canvas = null;
        SubMenu = null;
        InSubMenu = false;
        InReactionMode = false;
    }

    public static void RegisterCleanup()
    {
        MemoryManager.AddListeners(Clear);
        UnitMenuTextbox.RegisterCleanup();
    }
    

    public static void Initialize(GameObject canvas)
    {
        if (Menu != null) return;

        Canvas = canvas.GetComponent<Canvas>();
        Menu = new GameObject("UnitMenu", typeof(RectTransform));
        Menu.AddComponent<UnitMenu>();
        Menu.transform.SetParent(canvas.transform, false);
        Menu.SetActive(false);
        Menu.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        
        Textbox = new GameObject("UnitMenuTextbox", typeof(RectTransform)).AddComponent<UnitMenuTextbox>();
        Textbox.transform.SetParent(Menu.transform, false);

        Cursor = new GameObject("Cursor", typeof(RectTransform)).AddComponent<UnitMenuCursor>();
        Cursor.transform.SetParent(Menu.transform, false);
        Cursor.InstantiateCursor(MenuSlots);
    }


    public static void DisplayUnitMenu(Unit unit)
    {
        ClearUnitSlots();
        MenuSlots = new List<MenuSlot>();
        var actions = ChainSystem.ReactionInProgress ? unit.unitInfo.ActionSet.GetAllReactions() : unit.unitInfo.ActionSet.GetAllTurnActions();
        
        for (int i = 0; i < actions.Count; i++)
        {
            MenuSlot slot = new GameObject("Slot: " + actions[i].Name, typeof(RectTransform)).AddComponent<MenuSlot>();
            slot.transform.SetParent(Menu.transform, false);
        
            // TODO: Makes sure to get actions from actual unit instead of the testing list of UnitActions
            slot.DefineSlot(actions[i], unit.unitInfo);
            slot.PositionSlot(i, actions.Count);
        
            MenuSlots.Add(slot);
        }
        
        Cursor.ResetCursor(MenuSlots);
    }
    
    public static void DisplayUnitSubMenu(Unit unit, List<UnitAction> actions)
    {
        ClearUnitSlots();
        MenuSlots = new List<MenuSlot>();
        
        for (int i = 0; i < actions.Count; i++)
        {
            MenuSlot slot = new GameObject("Slot: " + actions[i].Name, typeof(RectTransform)).AddComponent<MenuSlot>();
            slot.transform.SetParent(Menu.transform, false);
        
            // TODO: Makes sure to get actions from actual unit instead of the testing list of UnitActions
            slot.DefineSlot(actions[i], unit.unitInfo);
            slot.PositionSlot(i, actions.Count);
        
            MenuSlots.Add(slot);
        }
        
        Cursor.ResetCursor(MenuSlots);
    }

    private static void ClearUnitSlots()
    {
        if (MenuSlots == null) { return; }

        foreach (var slot in MenuSlots.ToList())
        {
            MenuSlots.Remove(slot);
            Destroy(slot.gameObject);
        }
    }

    public static IEnumerator ShowMenu(Unit unit)
    {
        CameraMovement.SetFocusPoint(TilemapCreator.TileLocator[unit.unitInfo.Vector2CellLocation()].TileObj.transform);
        yield return new WaitUntil(() => !LeanTween.isTweening(Camera.main.transform.parent.gameObject));

        DisplayUnitMenu(unit);
        
        var canvasRect = Canvas.GetComponent<RectTransform>();

        // Convert world position to viewport position (0-1 range)
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(unit.transform.position);

        // Convert viewport position to canvas local position
        Vector2 menuPosition = new Vector2(
            (viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.565f),
            (viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f) - 40f // Move down slightly
        );

        Menu.GetComponent<RectTransform>().anchoredPosition = menuPosition;

        SubMenu = null;
        InSubMenu = false;
        Menu.SetActive(true);
        
        // Display Info Bar for Unit Menu
        InfoBar.DisplayInfo(ChainSystem.ReactionInProgress ? InfoTabType.Reaction : InfoTabType.Action);
    }

    public static void HideMenu() { Menu.SetActive(false); }
}
