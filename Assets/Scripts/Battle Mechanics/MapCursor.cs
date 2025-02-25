using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class MapCursor : MonoBehaviour
{
    [SerializeField] private TurnSystem turnSystem;
    [SerializeField] private CameraRotation cameraRotation;
    public static Vector2Int currentUnit; // Current unit's cell location
    public Vector2Int hoverCell;
    public bool canMove;

    private enum ControlState
    {
        Active,
        Action,
        Inactive
    }

    private static ControlState CursorControlState;

    private void Awake() { CursorControlState = ControlState.Inactive; }

    // Update is called once per frame
    void Update()
    {
        if (CursorControlState == ControlState.Active) { ActiveControls(); }
        if (CursorControlState == ControlState.Action) { ActionControls(); }
    }

    private void ActiveControls()
    {
        if (!Input.anyKeyDown) return;

        KeyCode GetPressedKey()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow)) return KeyCode.UpArrow;
            if (Input.GetKeyDown(KeyCode.DownArrow)) return KeyCode.DownArrow;
            if (Input.GetKeyDown(KeyCode.LeftArrow)) return KeyCode.LeftArrow;
            if (Input.GetKeyDown(KeyCode.RightArrow)) return KeyCode.RightArrow;
            if (Input.GetKeyDown(KeyCode.A)) return KeyCode.A;
            if (Input.GetKeyDown(KeyCode.Space)) return KeyCode.Space;

            return KeyCode.None; // Default case when no valid key is pressed
        }


        KeyCode keyPressed = GetPressedKey();
        switch (keyPressed)
        {
            case KeyCode.A:
                if (TilemapCreator.UnitLocator[hoverCell])
                {
                    CursorControlState = ControlState.Inactive;
                    currentUnit = hoverCell;
                    UnitMenu.ShowMenu();
                    UnitMenu.DisplayUnitMenu(TilemapCreator.UnitLocator[hoverCell].unitInfo.ActionSet.GetAllActions());
                }
                break;
            case KeyCode.Space:
                DeactivateMove();
                break;
            case KeyCode.LeftArrow:
                MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.left));
                break;
            case KeyCode.RightArrow:
                MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.right));
                break;
            case KeyCode.UpArrow:
                MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.up));
                break;
            case KeyCode.DownArrow:
                MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.down));
                break;
            default:
                break;
        }

    }

    private void ActionControls()
    {
        // Movement Controls
        MoveCursorUp();
        MoveCursorDown();
        MoveCursorLeft();
        MoveCursorRight();

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (TilemapCreator.TileLocator[hoverCell].IsSelectable())
            {
                Debug.Log("Moving here!");
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            ActionUtility.HideSelectableTilesForAction(TilemapCreator.UnitLocator[currentUnit]);
            UnitMenu.ShowMenu();
        }
    }

    private void MoveCursorUp() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.up));
        }
    }
    
    private void MoveCursorDown() {
        if (Input.GetKeyDown(KeyCode.DownArrow)) {
            MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.down));
        }
    }
    
    private void MoveCursorLeft() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.left));
        }
    }
    
    private void MoveCursorRight() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.right));
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
        Vector2Int cell2D = new Vector2Int(cell.x, cell.z);
        SetHoverCell(cell2D);
        currentUnit = cell2D;
        // this.canMove = true;
        CursorControlState = ControlState.Active;

        //send currentUnit unit object to UI Manager
        //UIManager.SetLeftPanel(TilemapCreator.UnitLocator[currentUnit]);
    }

    private void DeactivateMove()
    {
        RemoveTileOutline();
        canMove = false;
        hoverCell = Vector2Int.zero;
        currentUnit = Vector2Int.zero;
        turnSystem.ContinueLoop();
    }

    private void SetHoverCell(Vector2Int cell)
    {
        RemoveTileOutline();
        hoverCell = cell;
        AddTileOutline();
        cameraRotation.SetFocusPoint(TilemapCreator.TileLocator[hoverCell].TileObj.transform);

        // send hoverCell unit object to UI Manager
        //UIManager.SetRightPanel(hoverCell == currentUnit ? null : TilemapCreator.UnitLocator[hoverCell]);
    }

    private void AddTileOutline()
    {

        // adds outline effect on tile when mapcursor hovers it.
        GameObject tileObj = TilemapCreator.TileLocator[this.hoverCell].TileObj;
        if (tileObj.GetComponent<Outline>() == null)
        { 
            Outline outline = tileObj.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 2f;
            outline.enabled = true;
        }
        else
            tileObj.GetComponent<Outline>().enabled = true;
    }

    private void RemoveTileOutline()
    {
        if (this.hoverCell == null) return;
        
        // deactivates outline effect on tile
        GameObject tileObj = TilemapCreator.TileLocator[this.hoverCell].TileObj;
        if (tileObj.GetComponent<Outline>() != null)
            tileObj.GetComponent<Outline>().enabled = false;
    }

    public static void ActiveState() { CursorControlState = ControlState.Active; }
    public static void ActionState() { CursorControlState = ControlState.Action; }
    public static void InactiveState() { CursorControlState = ControlState.Inactive; }
}
