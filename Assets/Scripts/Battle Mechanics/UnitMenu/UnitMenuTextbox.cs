using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitMenuTextbox : MonoBehaviour
{
    public TMP_Text Text { get; private set; }
    public Sprite TextboxImage;
    
    // Start is called before the first frame update
    void Awake()
    {
        Text = new GameObject("Text", typeof(RectTransform)).AddComponent<TMP_Text>();
        Text.text = "Move";
        TextboxImage = Resources.Load<Sprite>("Sprites/UnitMenu/igt_unit_menu_textbox");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
