using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    private static int PANEL_WIDTH;
    private static int PANEL_HEIGHT;

    public static GameObject Menu { get; private set; }

    private void Update()
    {
         
        if(Menu == null || !Menu.activeSelf) return;

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Menu.transform.childCount == 0)
                ShowPauseMenu();
            else
                HidePauseMenu();
        }
    }

    public static void Clear()
    {
        Menu = null;
    }

    public static void RegisterCleanup()
    {
        MemoryManager.AddListeners(Clear);
    }

    public static void Initialize(GameObject canvas)
    {
        if (Menu != null) return;

        PANEL_WIDTH = 400;
        PANEL_HEIGHT = 200;

        Menu = new GameObject("PauseMenu", typeof(RectTransform));
        Menu.transform.SetParent(canvas.transform, false);
        RectTransform rectTransform = Menu.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, PANEL_WIDTH);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, PANEL_HEIGHT);
        Menu.AddComponent<PauseMenu>();
    }


    private static void ShowPauseMenu()
    {
        if(Menu.transform.childCount != 0) return;

        // 1) pause everything
        Time.timeScale = 0f;

        // 2) full-screen dark panel
        var panel = new GameObject("Panel", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(Menu.transform, false);
        var rect = panel.GetComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, PANEL_WIDTH);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, PANEL_HEIGHT);
        rect.offsetMin = rect.offsetMax = Vector2.zero;
        var img = panel.GetComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0.7f);

        // 3) gPausedh title
        var title = CreateText("Title", panel.transform, "PAUSED");
        title.rectTransform.anchorMin = title.rectTransform.anchorMax = new Vector2(0.5f, 1f);
        title.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 140);
        title.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 35);
        title.rectTransform.anchoredPosition = new Vector2(0, -30f);

        // 4) PLAY button
        CreateButton(
            "PlayButton",
            panel.transform,
            "PLAY",
            new Vector2(0, -90f),
            new Vector2(0.5f, 1f),
            () => HidePauseMenu()
        );

        // 5) MAIN MENU button
        CreateButton(
            "MainMenuButton",
            panel.transform,
            "MAIN MENU",
            new Vector2(0, -140f),
            new Vector2(0.5f, 1f),
            () => {
                Time.timeScale = 1f;
                MemoryManager.InvokeCleanup();
                SceneManager.LoadScene(0);
            }
        );

    }

    private static void HidePauseMenu()
    {
        if (Menu.transform.childCount == 0) return;

        foreach (Transform child in Menu.transform)
            Destroy(child.gameObject);
        
        Time.timeScale = 1f;
    }

    // helper to make a Button+Image+Text
    private static void CreateButton(string name, Transform parent, string label, Vector2 position, Vector2 anchor, UnityEngine.Events.UnityAction onClick)
    {
        // button root
        var btnGO = new GameObject(name, typeof(RectTransform), typeof(Image), typeof(Button));
        btnGO.transform.SetParent(parent, false);
        var rt = btnGO.GetComponent<RectTransform>();
        rt.anchorMin = rt.anchorMax = anchor;
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 120);
        rt.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 35);
        rt.anchoredPosition = position;

        // background
        var img = btnGO.GetComponent<Image>();
        img.color = Color.gray;

        // button behavior
        var btn = btnGO.GetComponent<Button>();
        btn.targetGraphic = img;
        btn.onClick.AddListener(onClick);

        // text child
        var txtGO = new GameObject("Text", typeof(RectTransform), typeof(TextMeshProUGUI));
        txtGO.transform.SetParent(btnGO.transform, false);
        var txtRT = txtGO.GetComponent<RectTransform>();
        txtRT.anchorMin = Vector2.zero;
        txtRT.anchorMax = Vector2.one;
        txtRT.offsetMin = txtRT.offsetMax = Vector2.zero;

        var txt = txtGO.GetComponent<TMP_Text>();
        txt.text = label;
        txt.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Oswald Bold SDF");
        txt.alignment = TextAlignmentOptions.Center;
        txt.color = Color.white;
        txt.enableAutoSizing = true;
        txt.fontSizeMin = 18;
        txt.fontSizeMax = 72;
    }

    // helper to make a single Text element
    private static TMP_Text CreateText(string name, Transform parent, string content)
    {
        var gameObj = new GameObject(name, typeof(RectTransform), typeof(TextMeshProUGUI));
        gameObj.transform.SetParent(parent, false);
        TMP_Text text = gameObj.GetComponent<TextMeshProUGUI>();
        text.text = content;
        text.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Oswald Bold SDF");
        text.alignment = TextAlignmentOptions.Center;
        text.color = Color.white;
        text.enableAutoSizing = true;
        text.fontSizeMin = 18;
        text.fontSizeMax = 72;
        return text;
    }
}
