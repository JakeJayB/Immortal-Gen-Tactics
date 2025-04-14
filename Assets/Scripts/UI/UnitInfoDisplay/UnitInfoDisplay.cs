using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoDisplay : MonoBehaviour
{
    private Image Display;
    private Sprite DisplaySprite;
    private Slider HPSlider;
    private Slider MPSlider;
    private TMP_Text HP;
    private TMP_Text HPValues;
    private TMP_Text MP;
    private TMP_Text MPValues;
    
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Initialize()
    {
        Display = new GameObject("Display").AddComponent<Image>();
        Display.transform.SetParent(transform, false);
        DisplaySprite = Resources.Load<Sprite>("Sprites/UnitInfo/UnitInfoDisplay");
        Display.sprite = DisplaySprite;
        Display.SetNativeSize();

        HPSlider = new GameObject("HP Slider", typeof(RectTransform)).AddComponent<Slider>();
        HPSlider.transform.SetParent(transform, false);
        HPSlider.transition = Selectable.Transition.None;
        HPSlider.direction = Slider.Direction.LeftToRight;
        HPSlider.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        
        var HPBar = new GameObject("HP Bar").AddComponent<Image>();
        HPBar.transform.SetParent(HPSlider.transform, false);
        var HPBarSprite = Resources.Load<Sprite>("Sprites/UnitInfo/IGT_HPBar");
        HPBar.sprite = HPBarSprite;
        HPBar.type = Image.Type.Filled;
        HPBar.fillMethod = Image.FillMethod.Horizontal;
        HPBar.fillOrigin = 0;
        HPBar.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 35);

        HPSlider.fillRect = HPBar.GetComponent<RectTransform>();
        
        MPSlider = new GameObject("MP Slider", typeof(RectTransform)).AddComponent<Slider>();
        MPSlider.transform.SetParent(transform, false);
        MPSlider.transition = Selectable.Transition.None;
        MPSlider.direction = Slider.Direction.LeftToRight;
        MPSlider.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        
        var MPBar = new GameObject("MP Bar").AddComponent<Image>();
        MPBar.transform.SetParent(MPSlider.transform, false);
        var MPBarSprite = Resources.Load<Sprite>("Sprites/UnitInfo/IGT_MPBar");
        MPBar.sprite = MPBarSprite;
        MPBar.type = Image.Type.Filled;
        MPBar.fillMethod = Image.FillMethod.Horizontal;
        MPBar.fillOrigin = 0;
        MPBar.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 35);
        
        MPSlider.fillRect = MPBar.GetComponent<RectTransform>();

        HPSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(-107, -8);
        MPSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(201, -8);
        
        // Add the TMP_Text component
        HP = new GameObject("HP", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        HPValues = new GameObject("HP Values", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        MP = new GameObject("MP", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        MPValues = new GameObject("MP Values", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        
        // Set the TMP_Text components inside the UnitInfoDisplay GameObject
        HP.transform.SetParent(transform, false);
        HPValues.transform.SetParent(transform, false);
        MP.transform.SetParent(transform, false);
        MPValues.transform.SetParent(transform, false);
        
        // Configure text properties
        HP.SetText("HP");
        HP.fontSize = 30;
        HP.alignment = TextAlignmentOptions.Left;
        HP.color = Color.white;
        
        HPValues.SetText("100/100");
        HPValues.fontSize = 30;
        HPValues.alignment = TextAlignmentOptions.Right;
        HPValues.color = Color.white;
        
        MP.SetText("MP");
        MP.fontSize = 30;
        MP.alignment = TextAlignmentOptions.Left;
        MP.color = Color.white;
        
        MPValues.SetText("30/30");
        MPValues.fontSize = 30;
        MPValues.alignment = TextAlignmentOptions.Right;
        MPValues.color = Color.white;
        
        // Position TMP_Text Components
        HP.GetComponent<RectTransform>().anchoredPosition = new Vector2(-140, 25);
        HPValues.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, 25);
        MP.GetComponent<RectTransform>().anchoredPosition = new Vector2(165, 25);
        MPValues.GetComponent<RectTransform>().anchoredPosition = new Vector2(235, 25);
    }

    public void InitializeTarget()
    {
        Display = new GameObject("Display").AddComponent<Image>();
        Display.transform.SetParent(transform, false);
        DisplaySprite = Resources.Load<Sprite>("Sprites/UnitInfo/UnitInfoDisplay");
        Display.sprite = DisplaySprite;
        var DisplayScale = Display.GetComponent<RectTransform>().localScale;
        Display.GetComponent<RectTransform>().localScale = new Vector3(DisplayScale.x * -1, DisplayScale.y * 1, DisplayScale.z * 1);
        Display.SetNativeSize();

        HPSlider = new GameObject("HP Slider", typeof(RectTransform)).AddComponent<Slider>();
        HPSlider.transform.SetParent(transform, false);
        HPSlider.transition = Selectable.Transition.None;
        HPSlider.direction = Slider.Direction.LeftToRight;
        HPSlider.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        
        var HPBar = new GameObject("HP Bar").AddComponent<Image>();
        HPBar.transform.SetParent(HPSlider.transform, false);
        var HPBarSprite = Resources.Load<Sprite>("Sprites/UnitInfo/IGT_HPBar");
        HPBar.sprite = HPBarSprite;
        HPBar.type = Image.Type.Filled;
        HPBar.fillMethod = Image.FillMethod.Horizontal;
        HPBar.fillOrigin = 0;
        HPBar.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 35);

        HPSlider.fillRect = HPBar.GetComponent<RectTransform>();
        
        MPSlider = new GameObject("MP Slider", typeof(RectTransform)).AddComponent<Slider>();
        MPSlider.transform.SetParent(transform, false);
        MPSlider.transition = Selectable.Transition.None;
        MPSlider.direction = Slider.Direction.LeftToRight;
        MPSlider.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        
        var MPBar = new GameObject("MP Bar").AddComponent<Image>();
        MPBar.transform.SetParent(MPSlider.transform, false);
        var MPBarSprite = Resources.Load<Sprite>("Sprites/UnitInfo/IGT_MPBar");
        MPBar.sprite = MPBarSprite;
        MPBar.type = Image.Type.Filled;
        MPBar.fillMethod = Image.FillMethod.Horizontal;
        MPBar.fillOrigin = 0;
        MPBar.GetComponent<RectTransform>().sizeDelta = new Vector2(300, 35);
        
        MPSlider.fillRect = MPBar.GetComponent<RectTransform>();

        HPSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(-201, -8);
        MPSlider.GetComponent<RectTransform>().anchoredPosition = new Vector2(107, -8);
        
        // Add the TMP_Text component
        HP = new GameObject("HP", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        HPValues = new GameObject("HP Values", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        MP = new GameObject("MP", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        MPValues = new GameObject("MP Values", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        
        // Set the TMP_Text components inside the UnitInfoDisplay GameObject
        HP.transform.SetParent(transform, false);
        HPValues.transform.SetParent(transform, false);
        MP.transform.SetParent(transform, false);
        MPValues.transform.SetParent(transform, false);
        
        // Configure text properties
        HP.SetText("HP");
        HP.fontSize = 30;
        HP.alignment = TextAlignmentOptions.Left;
        HP.color = Color.white;
        
        HPValues.SetText("100/100");
        HPValues.fontSize = 30;
        HPValues.alignment = TextAlignmentOptions.Right;
        HPValues.color = Color.white;
        
        MP.SetText("MP");
        MP.fontSize = 30;
        MP.alignment = TextAlignmentOptions.Left;
        MP.color = Color.white;
        
        MPValues.SetText("30/30");
        MPValues.fontSize = 30;
        MPValues.alignment = TextAlignmentOptions.Right;
        MPValues.color = Color.white;
        
        // Position TMP_Text Components
        HP.GetComponent<RectTransform>().anchoredPosition = new Vector2(-232, 25);
        HPValues.GetComponent<RectTransform>().anchoredPosition = new Vector2(-162, 25);
        MP.GetComponent<RectTransform>().anchoredPosition = new Vector2(73, 25);
        MPValues.GetComponent<RectTransform>().anchoredPosition = new Vector2(143, 25);
    }
    
    public void DisplayUnitInfo(UnitInfo unitInfo)
    {
        HPValues.text = unitInfo.currentHP + "/" + unitInfo.finalHP;
        UpdateHPSlider((float)unitInfo.currentHP / unitInfo.finalHP);
        
        MPValues.text = unitInfo.currentMP + "/" + unitInfo.finalMP;
        UpdateMPSlider((float)unitInfo.currentMP / unitInfo.finalMP);
    }

    
    
    private void UpdateHPSlider(float healthPercent) { HPSlider.value = healthPercent; }
    private void UpdateMPSlider(float magicPercent) { MPSlider.value = magicPercent; }
}
