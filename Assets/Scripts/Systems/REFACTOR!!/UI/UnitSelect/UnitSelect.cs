using System.Collections;
using System.Collections.Generic;
using UnityEditor.Graphs;
using UnityEngine;

namespace IGT.Systems
{
    public class UnitSelect : MonoBehaviour
    {
        private const int UNIT_SELECT_SIZE = 15;
        private const int UNIT_SELECT_ROW_COUNT = 3;
        private const float START_TRANSITION_OFFSET = 1200f;

        private static GameObject gameObj;
        private int index;
        [SerializeField] private UnitSelectSlot[] UnitSelectSlots = new UnitSelectSlot[UNIT_SELECT_SIZE];
        [SerializeField] private Vector2[] SlotPositions = new Vector2[UNIT_SELECT_SIZE];
        private static bool isActivated = false;
        private bool canInteract = false;

        private void Awake()
        {
            gameObj = gameObject;
        }

        // Start is called before the first frame update
        void Start()
        {
            InitializeUnitSelect();
            SetGameObjInactive();
        }

        // Update is called once per frame
        void Update()
        {
            if (isActivated) {
                isActivated = false;
                HighlightActiveUnits();
                StartCoroutine(PerformEntryTransition());
                return;
            }

            if (canInteract)
            {
                if (!Input.anyKeyDown) { return; }

                MoveUp();
                MoveDown();
                MoveLeft();
                MoveRight();
                SelectUnitSlot();
                ExitUnitSelect();
            }
        }

        private void InitializeUnitSelect()
        {
            RecordUnitSlotPositions();
            AssignUnitSlots();
            index = 7;
            UnitSelectSlots[index].HighlightSlot();
        }

        private void RecordUnitSlotPositions()
        {
            for (int i = 0; i < UNIT_SELECT_SIZE; i++)
            {
                SlotPositions[i] = UnitSelectSlots[i].GetComponent<RectTransform>().anchoredPosition;
            }
        }

        private void AssignUnitSlots()
        {
            for (int i = 0; i < PartyManager.unitList.Count; i++)
            {
                UnitSelectSlots[i].ReferenceUnit(PartyManager.unitList[i]);
                UnitSelectSlots[i].UpdatePortrait();
            }
        }

        private IEnumerator PerformEntryTransition()
        {
            float slotDelay = 0.025f;

            for (int col = 0; col < 5; col++)
            {
                for (int row = 0; row < 3; row++)
                {
                    int slotNum = (row * 5) + col;
                    RectTransform slot = UnitSelectSlots[slotNum].GetComponent<RectTransform>();
                    Vector2 target = SlotPositions[slotNum];

                    if (col == 4 && row == 2)
                    {
                        yield return MoveSlot(slot, target);
                    }
                    else
                    {
                        StartCoroutine(MoveSlot(slot, target));
                        yield return new WaitForSeconds(slotDelay);
                    }          
                }
            }
            canInteract = true;
        }

        private void OffsetSlots()
        {
            foreach (UnitSelectSlot slot in UnitSelectSlots){
                slot.GetComponent<RectTransform>().anchoredPosition += new Vector2(START_TRANSITION_OFFSET, 0);
            }
        }

        private void HighlightActiveUnits()
        {
            foreach (UnitSelectSlot slot in UnitSelectSlots)
            {
                if (TilemapCreator.UnitLocator.ContainsValue(slot.ReferencedUnit()))
                {
                    slot.FlagUnitIsActive();
                }
                else
                {
                    slot.FlagUnitIsInactive();
                }

                slot.UpdateBackground();
            }
        }

        IEnumerator MoveSlot(RectTransform slot, Vector2 end)
        {
            float moveDuration = 0.25f;
            Vector2 start = slot.anchoredPosition;
            float elapsed = 0f;

            while (elapsed < moveDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / moveDuration;

                // Smooth easing
                t = Mathf.SmoothStep(0f, 1f, t);

                slot.anchoredPosition = Vector2.Lerp(start, end, t);
                yield return null;
            }

            slot.anchoredPosition = end;
        }

        private void MoveUp()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                UnitSelectSlots[index].RemoveHighlight();

                int projectedIndex = index - (UNIT_SELECT_SIZE / UNIT_SELECT_ROW_COUNT);
                index = projectedIndex < 0
                    ? UNIT_SELECT_SIZE + projectedIndex
                    : projectedIndex;

                UnitSelectSlots[index].HighlightSlot();
            }
        }

        private void MoveDown()
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                UnitSelectSlots[index].RemoveHighlight();

                int projectedIndex = index + (UNIT_SELECT_SIZE / UNIT_SELECT_ROW_COUNT);
                index = projectedIndex > UNIT_SELECT_SIZE - 1
                    ? projectedIndex - UNIT_SELECT_SIZE
                    : projectedIndex;

                UnitSelectSlots[index].HighlightSlot();
            }
        }

        private void MoveLeft()
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                UnitSelectSlots[index].RemoveHighlight();

                int leftBound = index - (index % (UNIT_SELECT_SIZE / UNIT_SELECT_ROW_COUNT));
                int projectedIndex = index - 1;
                index = projectedIndex < leftBound
                    ? projectedIndex + (UNIT_SELECT_SIZE / UNIT_SELECT_ROW_COUNT)
                    : projectedIndex;

                UnitSelectSlots[index].HighlightSlot();
            }
        }

        private void MoveRight()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                UnitSelectSlots[index].RemoveHighlight();

                int rightBound = index - (index % (UNIT_SELECT_SIZE / UNIT_SELECT_ROW_COUNT)) + 4;
                int projectedIndex = index + 1;
                index = projectedIndex > rightBound
                    ? projectedIndex - (UNIT_SELECT_SIZE / UNIT_SELECT_ROW_COUNT)
                    : projectedIndex;

                UnitSelectSlots[index].HighlightSlot();
            }
        }

        // TODO: Don't allow units to be placed on occupied tiles
        // TODO: Don't allow active units to be selected again
        // TODO: Don't allow empty slots to be passed as selectable units
        private void SelectUnitSlot()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (UnitSelectSlots[index].ReferencedUnit() == null) { return; }
                if (UnitSelectSlots[index].unitIsActive) { return; }

                //UnitSelectSlots[index].RemoveHighlight();
                FormationManager.PlaceUnitOnTile(UnitSelectSlots[index].ReferencedUnit());
                //UnitSelectSlots[index].FlagUnitIsActive();
                //UnitSelectSlots[index].UpdateBackground();
                MapCursor.SetGameObjActive();
                SetGameObjInactive();
            }
        }

        private void ExitUnitSelect()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                MapCursor.SetGameObjActive();
                SetGameObjInactive();
            }
        }

        public static void SetGameObjActive() { 
            gameObj.SetActive(true);
            isActivated = true;
        }
        public void SetGameObjInactive() {
            OffsetSlots();
            canInteract = false;
            gameObj.SetActive(false); 
        }
    }
}
