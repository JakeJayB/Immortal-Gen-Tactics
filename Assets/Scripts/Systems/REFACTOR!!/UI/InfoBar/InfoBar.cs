using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum InfoTabType { Action, Reaction }

public class InfoBar : MonoBehaviour
{
    // InfoBar Object Properties
    private static GameObject InfoBarObj;
    public Image InfoBarImage;
    public Sprite InfoBarSprite;
    public TMP_Text InfoBarText;
    
    // InfoTab Object Properties
    private static Image InfoTabImage;
    private static readonly Dictionary<InfoTabType, string> InfoTabSprites = new Dictionary<InfoTabType, string>()
    {
        { InfoTabType.Action, "Sprites/InfoBar/ActionInfoTab"},
        { InfoTabType.Reaction, "Sprites/InfoBar/ReactionInfoTab"}
    };

    public void Initialize()
    {
        InfoBarObj = new GameObject("InfoBar", typeof(RectTransform));
        InfoBarImage = InfoBarObj.AddComponent<Image>();
        InfoBarImage.transform.SetParent(transform, false);
        InfoBarSprite = Resources.Load<Sprite>("Sprites/InfoBar/InfoBar");
        InfoBarImage.sprite = InfoBarSprite;
        InfoBarImage.SetNativeSize();
        
        InfoBarObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
        InfoBarObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
        InfoBarObj.GetComponent<RectTransform>().sizeDelta = new Vector2(800, 30);
        InfoBarObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -25);

        var tabObj = new GameObject("Tab", typeof(RectTransform));
        InfoTabImage = tabObj.AddComponent<Image>();
        InfoTabImage.transform.SetParent(InfoBarObj.transform, false);
        InfoTabImage.sprite = null;
        
        tabObj.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 1);
        tabObj.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 1);
        tabObj.GetComponent<RectTransform>().anchoredPosition = new Vector2(-320, -25);
        tabObj.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        
        InfoBarObj.SetActive(false);
    }

    public static void DisplayInfo(InfoTabType infoType) {
        InfoTabImage.sprite = Resources.Load<Sprite>(InfoTabSprites[infoType]);
        InfoTabImage.SetNativeSize();
        InfoBarObj.SetActive(true);
    }
    
    public static void HideInfo() { InfoBarObj.SetActive(false); }
}
