using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UnitMenuTextbox : MonoBehaviour
{
    private Image Textbox;
    public static TMP_Text Text { get; private set; }
    public Sprite TextboxImage;
    
    // Start is called before the first frame update
    void Awake()
    {
        InstantiateTextbox();
        DefineText();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InstantiateTextbox()
    {
        // Create a new GameObject for the text
        Textbox = new GameObject("Textbox").AddComponent<Image>();
        Textbox.transform.SetParent(transform, false);
        TextboxImage = Resources.Load<Sprite>("Sprites/UnitMenu/igt_unit_menu_textbox");
        
        Textbox.sprite = TextboxImage;
        Textbox.SetNativeSize();
        
        RectTransform rectTransform = Textbox.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = new Vector2(73, -40);
    }
    
    public void DefineText()
    {
        // Add the TMP_Text component
        Text = new GameObject("Text", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        Text.transform.SetParent(transform, false);
        
        RectTransform textRectTransform = Text.GetComponent<RectTransform>();
        textRectTransform.anchoredPosition = new Vector2(73, -40);

        // Configure text properties
        Text.SetText("Move");
        Text.fontSize = 14;
        Text.alignment = TextAlignmentOptions.Center;
        Text.color = Color.white;
    }

    public static void UpdateText(string slotName)
    {
        Text.SetText(slotName);
    }
}
