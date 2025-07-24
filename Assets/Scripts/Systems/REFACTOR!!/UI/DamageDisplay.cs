using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class DamageDisplay : MonoBehaviour
{
    public static IEnumerator DisplayUnitDamage(Unit unit, int damage)
    {
        yield return new WaitUntil(() => !LeanTween.isTweening(Camera.main.transform.parent.gameObject));
        TMP_Text displayText =  new GameObject("Display Text", typeof(RectTransform)).AddComponent<TextMeshProUGUI>();
        displayText.transform.SetParent(CanvasUI.ReferenceCanvas());

        var canvasRect = CanvasUI.ReferenceCanvas().GetComponent<RectTransform>();
        
        displayText.text = damage.ToString();
        displayText.fontSize = 100;
        displayText.fontStyle = FontStyles.Bold;
        
        // Convert world position to viewport position (0-1 range)
        Vector2 viewportPosition = Camera.main.WorldToViewportPoint(unit.GameObj.transform.position);

        // Convert viewport position to canvas local position
        Vector2 textPosition = new Vector2(
            (viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * (0.48f + 0.01f * (displayText.text.Length - 1))),
            (viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f) + 50f // Move down slightly
        );
        
        displayText.GetComponent<RectTransform>().anchoredPosition = textPosition;

        yield return new WaitForSeconds(1.0f);
        Destroy(displayText);
    }
}
