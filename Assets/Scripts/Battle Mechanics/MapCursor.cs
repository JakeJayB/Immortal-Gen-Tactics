using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class MapCursor : MonoBehaviour
{


    [SerializeField] private TurnSystem turnSystem;
    [SerializeField] private CameraRotation cameraRotation;
    public Vector2Int hoverCell;
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
            case KeyCode.LeftArrow:
                MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.left));
                Debug.Log("MapCursor: Left Arrow pressed");
                break;
            case KeyCode.RightArrow:
                MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.right));
                Debug.Log("MapCursor: Right Arrow pressed");
                break;
            case KeyCode.UpArrow:
                MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.up));
                Debug.Log("MapCursor: Up Arrow pressed");
                break;
            case KeyCode.DownArrow:
                MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.down));
                Debug.Log("MapCursor: Down Arrow pressed");
                break;
            default:
                break;
        }

    }

    // Adjusts movement direction based on the camera's Y rotation
    private Vector2Int GetRelativeDirection(Vector2Int inputDirection)
    {
        float cameraYRotation = cameraRotation.transform.eulerAngles.y;

        // Get the rotation step interval (8 intervals since camera rotates 45 degrees)
        int rotationStep = Mathf.RoundToInt(cameraYRotation / 45f) % 8; //

        // Map rotation steps to directional changes
        Dictionary<int, Vector2Int> directionMap = new Dictionary<int, Vector2Int>
    {
        { 0, inputDirection }, // 0 degrees: no change
        { 1, new Vector2Int(inputDirection.y, -inputDirection.x) }, // 45 degrees
        { 2, new Vector2Int(inputDirection.y, -inputDirection.x) }, // 90 degrees: rotate 90 degrees clockwise
        { 3, new Vector2Int(-inputDirection.x, -inputDirection.y) }, // 135 degrees
        { 4, -inputDirection }, // 180 degrees: invert direction
        { 5, new Vector2Int(-inputDirection.y, inputDirection.x) }, // 225 degrees
        { 6, new Vector2Int(-inputDirection.y, inputDirection.x) }, // 270 degrees: rotate 90 degrees counter-clockwise
        { 7, inputDirection }, // 315 degrees
    };

        return directionMap[rotationStep];
    }


    private void MoveCursor(Vector2Int cell)
    {

        if(TilemapCreator.TileLocator.ContainsKey(cell))
            SetHoverCell(cell);
    }

    public void ActivateMove(Vector3Int cell)
    {
        /* This function is called from TilemapCreator */
        SetHoverCell(new Vector2Int(cell.x, cell.z));
        this.canMove = true;
    }

    private void DeactivateMove()
    {
        canMove = false;
        RemoveTileOutline();
        turnSystem.ContinueLoop();
    }

    private void SetHoverCell(Vector2Int cell)
    {
        RemoveTileOutline();
        this.hoverCell = cell;
        AddTileOutline();
        cameraRotation.SetFocusPoint(TilemapCreator.TileLocator[this.hoverCell].GameObj.transform);
    }

    private void AddTileOutline()
    {

        // adds outline effect on tile when mapcursor hovers it.
        GameObject tileObj = TilemapCreator.TileLocator[this.hoverCell].GameObj;
        if (tileObj.GetComponent<Outline>() == null)
        { 
            Outline outline = tileObj.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 10f;
            outline.enabled = true;
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
