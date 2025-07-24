using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoDisplay : MonoBehaviour
{
    private Image Display;
    private Sprite DisplaySprite;
    private TMP_Text Name;
    private Image ProfileImage;
    private Slider HPSlider;
    private Slider MPSlider;
    private TMP_Text HP;
    private TMP_Text HPValues;
    private TMP_Text MP;
    private TMP_Text MPValues;
    private TMP_Text Actions;
    private Image[] ActionPoints;
    

    public void Initialize()
    {
        Display = new GameObject("Display").AddComponent<Image>();
        Display.transform.SetParent(transform, false);
        DisplaySprite = Resources.Load<Sprite>("Sprites/UnitInfo/UnitInfoDisplay");
        Display.sprite = DisplaySprite;
        Display.SetNativeSize();

        ProfileImage = new GameObject("Profile Image", typeof(RectTransform)).AddComponent<Image>();
        ProfileImage.transform.SetParent(transform, false);
        ProfileImage.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 56);
        ProfileImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(-375, 92);

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
        Name = new GameObject("Name", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        Name.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 50);
        HP = new GameObject("HP", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        HPValues = new GameObject("HP Values", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        MP = new GameObject("MP", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        MPValues = new GameObject("MP Values", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        Actions = new GameObject("Actions", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        
        // Set the TMP_Text components inside the UnitInfoDisplay GameObject
        Name.transform.SetParent(transform, false);
        HP.transform.SetParent(transform, false);
        HPValues.transform.SetParent(transform, false);
        MP.transform.SetParent(transform, false);
        MPValues.transform.SetParent(transform, false);
        Actions.transform.SetParent(transform, false);
        
        // Configure text properties
        Name.SetText("Name");
        Name.fontSize = 30;
        Name.fontStyle = FontStyles.Bold;
        Name.alignment = TextAlignmentOptions.Right;
        Name.color = Color.white;
        
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
        
        Actions.SetText("Actions");
        Actions.fontSize = 30;
        Actions.fontStyle = FontStyles.Bold;
        Actions.alignment = TextAlignmentOptions.Right;
        Actions.color = Color.white;
        
        // Position TMP_Text Components
        Name.GetComponent<RectTransform>().anchoredPosition = new Vector2(-65, 90);
        HP.GetComponent<RectTransform>().anchoredPosition = new Vector2(-140, 25);
        HPValues.GetComponent<RectTransform>().anchoredPosition = new Vector2(-70, 25);
        MP.GetComponent<RectTransform>().anchoredPosition = new Vector2(165, 25);
        MPValues.GetComponent<RectTransform>().anchoredPosition = new Vector2(235, 25);
        Actions.GetComponent<RectTransform>().anchoredPosition = new Vector2(-450, -90);
        
        // Establish Action Points
        ActionPoints = new []
        {
            new GameObject("AP 0", typeof(RectTransform)).AddComponent<Image>(),
            new GameObject("AP 1", typeof(RectTransform)).AddComponent<Image>(),
            new GameObject("AP 2", typeof(RectTransform)).AddComponent<Image>(),
            new GameObject("AP 3", typeof(RectTransform)).AddComponent<Image>(),
            new GameObject("AP 4", typeof(RectTransform)).AddComponent<Image>(),
            new GameObject("AP 5", typeof(RectTransform)).AddComponent<Image>(),
            new GameObject("AP 6", typeof(RectTransform)).AddComponent<Image>()
        };

        for (int i = 0; i < ActionPoints.Length; i++)
        {
            ActionPoints[i].transform.SetParent(transform, false);
            ActionPoints[i].sprite = Resources.Load<Sprite>("Sprites/UnitInfo/ActionPoint");
            ActionPoints[i].GetComponent<RectTransform>().sizeDelta = new Vector2(35, 35);
            ActionPoints[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(315 - (75 * i), -90);
        }
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

        ProfileImage = new GameObject("Profile Image", typeof(RectTransform)).AddComponent<Image>();
        ProfileImage.transform.SetParent(transform, false);
        ProfileImage.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 56);
        var ProfileScale = ProfileImage.GetComponent<RectTransform>().localScale;
        ProfileImage.GetComponent<RectTransform>().localScale = new Vector3(ProfileScale.x * -1, ProfileScale.y * 1, ProfileScale.z * 1);
        ProfileImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(375, 92);
        
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
        Name = new GameObject("Name", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        Name.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 50);
        HP = new GameObject("HP", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        HPValues = new GameObject("HP Values", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        MP = new GameObject("MP", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        MPValues = new GameObject("MP Values", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        Actions = new GameObject("Actions", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        
        
        // Set the TMP_Text components inside the UnitInfoDisplay GameObject
        Name.transform.SetParent(transform, false);
        HP.transform.SetParent(transform, false);
        HPValues.transform.SetParent(transform, false);
        MP.transform.SetParent(transform, false);
        MPValues.transform.SetParent(transform, false);
        Actions.transform.SetParent(transform, false);
        
        // Configure text properties
        Name.SetText("Name");
        Name.fontSize = 30;
        Name.fontStyle = FontStyles.Bold;
        Name.alignment = TextAlignmentOptions.Left;
        Name.color = Color.white;
        
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
        
        Actions.SetText("Actions");
        Actions.fontSize = 30;
        Actions.fontStyle = FontStyles.Bold;
        Actions.alignment = TextAlignmentOptions.Right;
        Actions.color = Color.white;
        
        // Position TMP_Text Components
        Name.GetComponent<RectTransform>().anchoredPosition = new Vector2(65, 90);
        HP.GetComponent<RectTransform>().anchoredPosition = new Vector2(-232, 25);
        HPValues.GetComponent<RectTransform>().anchoredPosition = new Vector2(-162, 25);
        MP.GetComponent<RectTransform>().anchoredPosition = new Vector2(73, 25);
        MPValues.GetComponent<RectTransform>().anchoredPosition = new Vector2(143, 25);
        Actions.GetComponent<RectTransform>().anchoredPosition = new Vector2(360, -90);
        
        // Establish Action Points
        ActionPoints = new []
        {
            new GameObject("AP 0", typeof(RectTransform)).AddComponent<Image>(),
            new GameObject("AP 1", typeof(RectTransform)).AddComponent<Image>(),
            new GameObject("AP 2", typeof(RectTransform)).AddComponent<Image>(),
            new GameObject("AP 3", typeof(RectTransform)).AddComponent<Image>(),
            new GameObject("AP 4", typeof(RectTransform)).AddComponent<Image>(),
            new GameObject("AP 5", typeof(RectTransform)).AddComponent<Image>(),
            new GameObject("AP 6", typeof(RectTransform)).AddComponent<Image>()
        };

        for (int i = 0; i < ActionPoints.Length; i++)
        {
            ActionPoints[i].transform.SetParent(transform, false);
            ActionPoints[i].sprite = Resources.Load<Sprite>("Sprites/UnitInfo/ActionPoint");
            ActionPoints[i].GetComponent<RectTransform>().sizeDelta = new Vector2(35, 35);
            ActionPoints[i].GetComponent<RectTransform>().anchoredPosition = new Vector2(-315 + (75 * i), -90);
        }
    }
    
    public void DisplayUnitInfo(UnitInfo unitInfo)
    {
        Name.text = unitInfo.unit.GameObj.name;
        
        ProfileImage.sprite = unitInfo.UnitAffiliation == UnitAffiliation.Player
            ? Resources.Load<Sprite>("Sprites/Units/Test_Player/PlayerProfile")
            : Resources.Load<Sprite>("Sprites/Units/Test_Enemy/EnemyProfile");
        
        HPValues.text = unitInfo.currentHP + "/" + unitInfo.FinalHP;
        UpdateHPSlider(unitInfo.FinalHP > 0 ? (float)unitInfo.currentHP / unitInfo.FinalHP : 0);
        
        MPValues.text = unitInfo.currentMP + "/" + unitInfo.FinalMP;
        UpdateMPSlider(unitInfo.FinalMP > 0 ? (float)unitInfo.currentMP / unitInfo.FinalMP : 0);

        for (int i = 0; i < ActionPoints.Length; i++)
        {
            if (i + 1 <= unitInfo.currentAP) {
                ActionPoints[i].gameObject.SetActive(true);
                ActionPoints[i].sprite = Resources.Load<Sprite>("Sprites/UnitInfo/ActionPoint");
            }
            else if (i + 1 <= unitInfo.FinalAP) {
                ActionPoints[i].gameObject.SetActive(true);
                ActionPoints[i].sprite = Resources.Load<Sprite>("Sprites/UnitInfo/UsedActionPoint");
            }
            else
            {
                ActionPoints[i].gameObject.SetActive(false);
            }
        }
    }

    
    
    private void UpdateHPSlider(float healthPercent) { HPSlider.value = healthPercent; }
    private void UpdateMPSlider(float magicPercent) { MPSlider.value = magicPercent; }
}
