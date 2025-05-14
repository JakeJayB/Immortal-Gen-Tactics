using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
    public enum GameResult
    {
        Win,
        Lose
    }


public class GameOver : MonoBehaviour
{

    private static int PANEL_WIDTH;
    private static int PANEL_HEIGHT;

    public static GameObject Menu { get; private set; }

    public static void Initialize(GameObject canvas)
    {
        if(Menu != null) return;

        PANEL_WIDTH = 400;
        PANEL_HEIGHT = 200;

        Menu = new GameObject("GameOver", typeof(RectTransform), typeof(GameOver));
        Menu.transform.SetParent(canvas.transform, false);
        RectTransform rectTransform = Menu.GetComponent<RectTransform>();
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, PANEL_WIDTH);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, PANEL_HEIGHT);
    }

    public static void Clear()
    {
        Menu = null;
    }

    public static void RegisterCleanup()
    {
        MemoryManager.AddListeners(Clear);
    }

    public static async void ShowMenu(GameResult result)
    {
        // 1) pause everything
        Time.timeScale = 0f;

        // 2) full-screen dark panel
        var panel = new GameObject("Panel", typeof(RectTransform), typeof(Image));
        panel.transform.SetParent(Menu.transform, false);
        var panelRect = panel.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, PANEL_WIDTH);
        panelRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, PANEL_HEIGHT);
        panelRect.offsetMin = panelRect.offsetMax = Vector2.zero;
        var img = panel.GetComponent<Image>();
        img.color = new Color(0f, 0f, 0f, 0.7f);

        // 3) gYou Won/You Losth title
        var title = new GameObject("ResultText", typeof(RectTransform), typeof(TextMeshProUGUI));
        title.transform.SetParent(panel.transform, false);

        var titleRect = title.GetComponent<RectTransform>();
        titleRect.anchorMin = titleRect.anchorMax = new Vector2(0.5f, 1f);
        titleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 140);
        titleRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 35);
        titleRect.anchoredPosition = new Vector2(0, -30f);


        var titleTxt = title.GetComponent<TMP_Text>();
        titleTxt.text = result == GameResult.Win ? "You Won!" : "You Lost..";
        titleTxt.font = Resources.Load<TMP_FontAsset>("Fonts & Materials/Oswald Bold SDF");
        titleTxt.alignment = TextAlignmentOptions.Center;
        titleTxt.color = Color.white;
        titleTxt.enableAutoSizing = true;
        titleTxt.fontSizeMin = 18;
        titleTxt.fontSizeMax = 72;

        // 4) MAIN MENU button
        CreateButton(
            "MainMenuButton",
            panel.transform,
            "MAIN MENU",
            new Vector2(0, -90f),
            new Vector2(0.5f, 1f),
            () => {
                Time.timeScale = 1f;
                MemoryManager.InvokeCleanup();
                SceneManager.LoadScene(0);
            }
        );
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
}
