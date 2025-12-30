using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IGT.Systems
{
    public class UnitSelect : MonoBehaviour
    {
        private const int UNIT_SELECT_SIZE = 15;
        private const int UNIT_SELECT_ROW_COUNT = 3;

        private int index;
        [SerializeField] private UnitSelectSlot[] UnitSelectSlots = new UnitSelectSlot[UNIT_SELECT_SIZE];

        // Start is called before the first frame update
        void Start()
        {
            InitializeUnitSelect();
        }

        // Update is called once per frame
        void Update()
        {
            MoveUp();
            MoveDown();
            MoveLeft();
            MoveRight();
        }

        private void InitializeUnitSelect()
        {
            AssignUnitSlots();
            index = 7;
            UnitSelectSlots[index].HighlightSlot();
        }

        private void AssignUnitSlots()
        {
            for (int i = 0; i < PartyManager.unitList.Count; i++)
            {
                UnitSelectSlots[i].ReferenceUnit(PartyManager.unitList[i]);
                UnitSelectSlots[i].UpdatePortrait();
            }
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
    }
}
