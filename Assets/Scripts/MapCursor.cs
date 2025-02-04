using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCursor : MonoBehaviour
{

    [SerializeField]
    private TurnSystem turnSystem;
    public Vector3Int hoverCell;
    public bool canMove;

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            MoveCursor();
        }
    }

    private void MoveCursor()
    {
        // TODO: Move Camera with map cursor
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("MapCursor: Space key pressed");
            DeactivateMove();
        }
    }

    private void DeactivateMove()
    {
        canMove = false;
        turnSystem.ContinueLoop();
    }


    public void ActivateMove(Unit unit)
    {
        /* This function is called from TilemapCreator */
        SetHoverCell(unit);
        this.canMove = true;
    }

    private void SetHoverCell(Unit unit)
    {
        this.hoverCell = unit.CellLocation;
    }
}
