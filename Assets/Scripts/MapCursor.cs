using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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
            CheckInput();
        }
    }

    private void CheckInput()
    {
        if (!Input.anyKeyDown) return;

        KeyCode GetPressedKey()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) return KeyCode.UpArrow;
            if (Input.GetKeyDown(KeyCode.DownArrow)) return KeyCode.DownArrow;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) return KeyCode.LeftArrow;
            if (Input.GetKeyDown(KeyCode.RightArrow)) return KeyCode.RightArrow;
            if (Input.GetKeyDown(KeyCode.Space)) return KeyCode.Space;

            return KeyCode.None; // Default case when no valid key is pressed
        }

        KeyCode keyPressed = GetPressedKey();
        switch (keyPressed)
        {
            case KeyCode.Space:
                Debug.Log("MapCursor: Space pressed");
                DeactivateMove();
                break;
            case KeyCode.UpArrow:
                Debug.Log("MapCursor: Up Arrow pressed");
                break;
            case KeyCode.DownArrow:
                Debug.Log("MapCursor: Down Arrow pressed");
                break;
            case KeyCode.LeftArrow:
                Debug.Log("MapCursor: Left Arrow pressed");
                break;
            case KeyCode.RightArrow:
                Debug.Log("MapCursor: Right Arrow pressed");
                break;
            default:
                break;
        }
        
    }


    public void ActivateMove(Vector3Int cell)
    {
        /* This function is called from TilemapCreator */
        SetHoverCell(cell);
        this.canMove = true;
    }

    private void DeactivateMove()
    {
        canMove = false;
        RemoveTileOutline();
        turnSystem.ContinueLoop();
    }

    private void SetHoverCell(Vector3Int cell)
    {
        RemoveTileOutline();
        this.hoverCell = cell;
        AddTileOutline();
    }

    private void AddTileOutline()
    {

        // adds outline effect on tile when mapcursor is on it.
        GameObject tileObj = TilemapCreator.TileLocator[this.hoverCell].GameObj;
        if (tileObj.GetComponent<Outline>() == null)
        { 
            Outline outline = tileObj.AddComponent<Outline>();
            outline.enabled = true;
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 10f;
        }
        else
            tileObj.GetComponent<Outline>().enabled = true;
        
    }

    private void RemoveTileOutline()
    {
        if (this.hoverCell == null) return;
        
        // deactivates outline effect on tile
        GameObject tileObj = TilemapCreator.TileLocator[this.hoverCell].GameObj;
        if (tileObj.GetComponent<Outline>() != null)
            tileObj.GetComponent<Outline>().enabled = false;
        
    }
}
