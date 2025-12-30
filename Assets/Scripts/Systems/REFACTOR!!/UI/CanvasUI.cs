using Unity.VisualScripting;
using UnityEngine;

public class CanvasUI : MonoBehaviour
{
    private static Transform Canvas;
    private static InfoBar InfoBar;
    private static UnitInfoDisplay UnitDisplay;
    private static UnitInfoDisplay TargetUnitDisplay;

    public static void Clear()
    {
        Canvas = null;
        InfoBar = null;
        UnitDisplay = null;
        TargetUnitDisplay = null;
    }

    public static void RegisterCleanup()
    {
        MemoryManager.AddListeners(Clear);
        UnitMenu.RegisterCleanup();
        //UnitSelector.RegisterCleanup();
        TurnCycle.RegisterCleanup();
        PauseMenu.RegisterCleanup();
        GameOver.RegisterCleanup();
    }

    
    private void Start()
    {
        Canvas = transform;
        
        UnitMenu.Initialize(this.gameObject);
        //UnitSelector.Initialize(this.gameObject);      
        TurnCycle.Initialize(this.gameObject);
        PauseMenu.Initialize(this.gameObject);
        GameOver.Initialize(this.gameObject);

        UnitDisplay = new GameObject("UnitDisplay", typeof(RectTransform)).AddComponent<UnitInfoDisplay>();
        UnitDisplay.transform.SetParent(transform, false);
        UnitDisplay.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
        UnitDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, 100);
        UnitDisplay.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        UnitDisplay.GetComponent<RectTransform>().anchorMax = new Vector2(0, 0);
        UnitDisplay.Initialize();
        HideTurnUnitInfoDisplay();
        
        TargetUnitDisplay = new GameObject("TargetUnitDisplay", typeof(RectTransform)).AddComponent<UnitInfoDisplay>();
        TargetUnitDisplay.transform.SetParent(transform, false);
        TargetUnitDisplay.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
        TargetUnitDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, 100);
        TargetUnitDisplay.GetComponent<RectTransform>().anchorMin = new Vector2(1, 0);
        TargetUnitDisplay.GetComponent<RectTransform>().anchorMax = new Vector2(1, 0);
        TargetUnitDisplay.InitializeTarget();
        HideTargetUnitInfoDisplay();

        InfoBar = this.AddComponent<InfoBar>();
        InfoBar.Initialize();
    }


    public static void ShowTurnUnitInfoDisplay(UnitInfo unitInfo) {
        UnitDisplay.gameObject.SetActive(true);
        UnitDisplay.DisplayUnitInfo(unitInfo);
    }
    
    public static void ShowTargetUnitInfoDisplay(UnitInfo unitInfo) {
        TargetUnitDisplay.gameObject.SetActive(true);
        TargetUnitDisplay.DisplayUnitInfo(unitInfo);
    }
    
    public static void HideTurnUnitInfoDisplay() { UnitDisplay.gameObject.SetActive(false); }
    public static void HideTargetUnitInfoDisplay() { TargetUnitDisplay.gameObject.SetActive(false); }

    public static Transform ReferenceCanvas() { return Canvas; }
}
