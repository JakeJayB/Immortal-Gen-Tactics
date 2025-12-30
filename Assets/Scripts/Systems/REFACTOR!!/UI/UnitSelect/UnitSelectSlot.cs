using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace IGT.Systems
{
    public class UnitSelectSlot : MonoBehaviour
    {
        // Slot Components
        [SerializeField] private Image slotFrame;
        [SerializeField] private Image slotBackground;
        [SerializeField] private Image slotPortrait;
        [SerializeField] private Unit referencedUnit;
        public bool isHighlighted { get; private set; }
        public bool unitIsActive { get; private set; } = false;

        // Slot Highlight Colors
        [SerializeField] private Color HighlightColor_1 = Color.yellow;
        [SerializeField] private Color HighlightColor_2 = Color.red;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        // TODO: Update function to set the sprite of the slotPortrait to the unit referenced
        public void UpdatePortrait() { slotPortrait.gameObject.SetActive(true); }

        public void UpdateBackground() { slotBackground.color = HighlightColor_2; }

        public void ReferenceUnit(Unit unit) { referencedUnit = unit; }

        public void FlagUnitIsActive() => unitIsActive = true;

        public Unit ReferencedUnit() => referencedUnit;

        // TODO: Change the function to pulse between "HighlightColor_1" and "HighlightColor_2"
        public void HighlightSlot() { slotFrame.color = HighlightColor_1; }

        public void RemoveHighlight() { slotFrame.color = Color.white; }

    }
}
