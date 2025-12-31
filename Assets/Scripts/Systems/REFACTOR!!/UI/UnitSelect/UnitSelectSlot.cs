using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IGT.Systems
{
    public class UnitSelectSlot : MonoBehaviour
    {
        private const float HIGHLIGHT_PULSE_SPEED = 6.0f;

        // Slot Components
        [SerializeField] private Image slotFrame;
        [SerializeField] private Image slotBackground;
        [SerializeField] private Image slotPortrait;
        [SerializeField] private Unit referencedUnit;
        public bool isHighlighted { get; private set; } = false;
        public bool unitIsActive { get; private set; } = false;

        // Slot Highlight Colors
        [SerializeField] private Color DefaultBackgroundColor;
        [SerializeField] private Color HighlightedBackgroundColor;
        [SerializeField] private Color ActiveBackgroundColor;
        [SerializeField] private Color HighlightColor_1 = Color.yellow;
        [SerializeField] private Color HighlightColor_2 = Color.red;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if (isHighlighted)
            {
                float t = (Mathf.Sin(Time.time * HIGHLIGHT_PULSE_SPEED) + 1f) * 0.5f;
                Color pulsedColor1 = Color.Lerp(HighlightColor_1, HighlightColor_2, t);
                Color pulsedColor2 = Color.Lerp(unitIsActive ? ActiveBackgroundColor : DefaultBackgroundColor, HighlightedBackgroundColor, t);
                slotFrame.color = (pulsedColor1);
                slotBackground.color = (pulsedColor2);
            }
        }

        // TODO: Update function to set the sprite of the slotPortrait to the unit referenced
        public void UpdatePortrait() { slotPortrait.gameObject.SetActive(true); }

        public void UpdateBackground() => slotBackground.color = unitIsActive ? ActiveBackgroundColor : DefaultBackgroundColor;

        public void ReferenceUnit(Unit unit) { referencedUnit = unit; }

        public void FlagUnitIsActive() => unitIsActive = true;

        public void FlagUnitIsInactive() => unitIsActive = false;

        public Unit ReferencedUnit() => referencedUnit;

        public void HighlightSlot() { 
            isHighlighted = true;
            slotBackground.color = HighlightedBackgroundColor;
        }

        public void RemoveHighlight() {
            isHighlighted = false;
            UpdateBackground();
            slotFrame.color = Color.white; 
        }

    }
}
