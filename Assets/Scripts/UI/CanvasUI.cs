using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class CanvasUI : MonoBehaviour
{
    private static UnitInfoDisplay UnitDisplay;
    private static UnitInfoDisplay TargetUnitDisplay;
    
    private void Start()
    {
        UnitMenu.Initialize(this.gameObject);
        UnitSelector.Initialize(this.gameObject);      
        TurnCycle.Initialize(this.gameObject);

        UnitDisplay = new GameObject("UnitDisplay", typeof(RectTransform)).AddComponent<UnitInfoDisplay>();
        UnitDisplay.transform.SetParent(transform, false);
        UnitDisplay.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
        UnitDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(-200, -150);
        UnitDisplay.Initialize();
        HideTurnUnitInfoDisplay();
        
        TargetUnitDisplay = new GameObject("TargetUnitDisplay", typeof(RectTransform)).AddComponent<UnitInfoDisplay>();
        TargetUnitDisplay.transform.SetParent(transform, false);
        TargetUnitDisplay.GetComponent<RectTransform>().localScale = new Vector3(0.4f, 0.4f, 0.4f);
        TargetUnitDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2(200, -150);
        TargetUnitDisplay.InitializeTarget();
        HideTargetUnitInfoDisplay();
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
}
