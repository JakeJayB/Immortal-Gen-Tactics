using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapCursor : MonoBehaviour
{
    private static GameObject gameObj;
    private static CameraMovement cameraMovement;
    public static Vector2Int currentUnit; 
    public static Vector2Int hoverCell;
    
    private enum ControlState
    {
        Start,
        Active,
        Action,
        Inactive
    }

    private static ControlState CursorControlState;

    private void Awake() { 
        CursorControlState = ControlState.Inactive;
        gameObj = gameObject;
        cameraMovement = Camera.main.transform.parent.GetComponent<CameraMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if(CursorControlState == ControlState.Start) { StartControls(); }
        if (CursorControlState == ControlState.Active) { ActiveControls(); }
        if (CursorControlState == ControlState.Action) { ActionControls(); }
    }
    
    private void StartControls()
    {
        if (!Input.anyKeyDown) return;

        // Movement Controls
        MoveCursorUp();
        MoveCursorDown();
        MoveCursorLeft();
        MoveCursorRight();


        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (TilemapCreator.TileLocator[hoverCell].IsSelectable())
                 UnitSelector.PlaceUnit(hoverCell);
        }
        else if (Input.GetKeyDown(KeyCode.Backspace))
        {
            UnitSelector.ResetUnitSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            UnitSelector.DestroyMenu();
            SelectedStartPositions();
        }
    }


    private void ActiveControls()
    {
        if (!Input.anyKeyDown) return;

        // Movement Controls
        MoveCursorUp();
        MoveCursorDown();
        MoveCursorLeft();
        MoveCursorRight();


        if(Input.GetKeyDown(KeyCode.A))
        {
            if (TilemapCreator.UnitLocator[hoverCell])
            {
                InactiveState();
                currentUnit = hoverCell;
                StartCoroutine(UnitMenu.ShowMenu(TilemapCreator.UnitLocator[currentUnit]));
                UnitMenu.DisplayUnitMenu(TilemapCreator.UnitLocator[hoverCell].unitInfo.ActionSet.GetAllActions());
            }
        }
    }

    private void ActionControls()
    {
        if (!Input.anyKeyDown) return;


        // Movement Controls
        MoveCursorUp();
        MoveCursorDown();
        MoveCursorLeft();
        MoveCursorRight();

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (TilemapCreator.TileLocator[hoverCell].IsSelectable())
            {
                if (ChainSystem.UnitIsReacting())
                {
                    // Call IEnumerator as a normal function, ignoring 'yield return' instructions
                    ChainSystem.AddAction(hoverCell);
                }
                StartCoroutine(ConfirmAction());

            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            InactiveState();
            CameraMovement.SetFocusPoint(TilemapCreator.TileLocator[currentUnit].TileObj.transform);
            SetHoverCell(currentUnit);
            ChainSystem.ReleasePotentialChain();
            ActionUtility.HideSelectableTilesForAction(TilemapCreator.UnitLocator[currentUnit]);

            StartCoroutine(UnitMenu.ShowMenu(TilemapCreator.UnitLocator[currentUnit]));
        }
    }

    private static IEnumerator ConfirmAction()
    {
        InactiveState();
        ActionUtility.HideSelectableTilesForAction(TilemapCreator.UnitLocator[currentUnit]);
        yield return ChainSystem.AddAction(hoverCell);
        yield return ChainSystem.ExecuteChain();
        
        
        // Set 'currentUnit' back to the current unit in the turn system
        currentUnit = TurnSystem.CurrentUnit.unitInfo.Vector2CellLocation();

        if (TurnSystem.CurrentUnit.unitInfo.currentAP > 0)
        {
            CameraMovement.SetFocusPoint(TilemapCreator.TileLocator[currentUnit].TileObj.transform);
            MoveCursor(currentUnit);
            yield return UnitMenu.ShowMenu(TilemapCreator.UnitLocator[currentUnit]);
        }
        else
        { 
            EndMove();
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
        float cameraYRotation = cameraMovement.transform.eulerAngles.y;

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


    private static void MoveCursor(Vector2Int cell)
    {
        if (TilemapCreator.TileLocator.ContainsKey(cell))
        {
            SetHoverCell(cell);
            SoundFXManager.PlaySoundFXClip("MapCursor", Camera.main.transform, 0.75f);

        }
    }

    public static void StartMove(Vector3Int cell)
    {
        Vector2Int cell2D = new Vector2Int(cell.x, cell.z);
        SetHoverCell(cell2D);
        currentUnit = cell2D;
        CursorControlState = ControlState.Active;

        //send currentUnit unit object to UI Manager
        //UIManager.SetLeftPanel(TilemapCreator.UnitLocator[currentUnit]);
    }

    public static void EndMove()
    {
        RemoveTileOutline();
        hoverCell = Vector2Int.zero;
        currentUnit = Vector2Int.zero;
        TurnSystem.ContinueLoop();
    }

    private static void SetHoverCell(Vector2Int cell)
    {
        RemoveTileOutline();
        hoverCell = cell;
        AddTileOutline();
        CameraMovement.CheckAndMove(TilemapCreator.TileLocator[hoverCell].TileObj.transform);

        // send hoverCell unit object to UI Manager
        //UIManager.SetRightPanel(hoverCell == currentUnit ? null : TilemapCreator.UnitLocator[hoverCell]);
    }

    private static void AddTileOutline()
    {

        // adds outline effect on tile when mapcursor hovers it.
        GameObject tileObj = TilemapCreator.TileLocator[hoverCell].TileObj;
        if (!tileObj.GetComponent<Outline>())
        { 
            Outline outline = tileObj.AddComponent<Outline>();
            outline.OutlineMode = Outline.Mode.OutlineAll;
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 2f;
            outline.enabled = true;
        }
        else
        {
            tileObj.GetComponent<Outline>().enabled = true;
        }
    }

    private static void RemoveTileOutline()
    {
        if (hoverCell == null) return;
        
        // deactivates outline effect on tile
        GameObject tileObj = TilemapCreator.TileLocator[hoverCell].TileObj;
        if (tileObj.GetComponent<Outline>())
            tileObj.GetComponent<Outline>().enabled = false;
    }


    public static void SelectStartPositions()
    {
        Transform startTransform = null;
        foreach (Tile tile in TilemapCreator.TileLocator.Values)
        {
            if (!tile.TileInfo.IsStartArea) continue;
            tile.OverlayObj.ActivateOverlayTile(OverlayMaterial.START);
            startTransform = tile.TileObj.transform;
        }
        SetHoverCell(new Vector2Int((int)startTransform.position.x, (int)startTransform.position.z));
        StartState();
    }

    private static void SelectedStartPositions()
    {
        foreach (Tile tile in TilemapCreator.TileLocator.Values)
        {
            if (tile.TileInfo.IsStartArea)
                tile.OverlayObj.DeactivateOverlayTile();
        }
        InactiveState();
        TurnSystem.StartLoop();
    }

    public static void StartState() { CursorControlState = ControlState.Start; }
    public static void ActiveState() { CursorControlState = ControlState.Active; }
    public static void ActionState() { CursorControlState = ControlState.Action; }
    public static void InactiveState() { CursorControlState = ControlState.Inactive; }
    
    public static void SetGameObjActive() { gameObj.SetActive(true); }
    public static void SetGameObjInactive() { gameObj.SetActive(false); }
}
