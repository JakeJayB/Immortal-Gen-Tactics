using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCursor : MonoBehaviour
{
    private static GameObject gameObj;
    private static CameraMovement cameraMovement;
    public static Vector2Int currentUnit; 
    public static Vector2Int hoverCell;
    
    public enum ControlState
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
        if (CursorControlState == ControlState.Start) { StartControls(); }
        if (CursorControlState == ControlState.Active) { ActiveControls(); }
        if (CursorControlState == ControlState.Action) { ActionControls(); }
    }


    public static void Clear()
    {
        gameObj = null;
        cameraMovement = null;
        hoverCell = Vector2Int.zero;
        currentUnit = Vector2Int.zero;
        CursorControlState = ControlState.Inactive;
    }

    public static void RegisterCleanup() =>
    MemoryManager.AddListeners(Clear);


    private void StartControls()
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
                 UnitSelector.PlaceUnit(hoverCell);
            else
                SoundFXManager.PlaySoundFXClip("Deselect", 0.4f);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            UnitSelector.ResetUnitSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && UnitSelector.IsThereActiveUnit())
        {
            UnitSelector.DestroyMenu();
            SelectedStartPositions();
            RemoveTileOutline();
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


        if (Input.GetKeyDown(KeyCode.A))
        {
            if (TilemapCreator.UnitLocator.TryGetValue(hoverCell, out Unit unit) && unit == TurnSystem.CurrentUnit)
            {
                InactiveState();
                //CameraMovement.SetFocusPoint(TilemapCreator.TileLocator[unit.unitInfo.Vector2CellLocation()].TileObj.transform);
                currentUnit = hoverCell;
                StartCoroutine(UnitMenu.ShowMenu(unit));
                UnitMenu.DisplayUnitMenu(unit);
                InfoBar.DisplayInfo(InfoTabType.Action);
                SoundFXManager.PlaySoundFXClip("Select", 0.2f);

            }
            else
            {
                SoundFXManager.PlaySoundFXClip("Deselect", 0.4f);
            }
        }
        else if(Input.GetKeyDown(KeyCode.S)) 
        {
            Unit unit = TilemapCreator.UnitLocator[currentUnit];

            InactiveState();
            SetHoverCell(currentUnit);
            CameraMovement.SetFocusPoint(TilemapCreator.TileLocator[currentUnit].TileObj.transform);
            CanvasUI.ShowTurnUnitInfoDisplay(unit.unitInfo);
            CanvasUI.HideTargetUnitInfoDisplay();
            StartCoroutine(UnitMenu.ShowMenu(unit));
            SoundFXManager.PlaySoundFXClip("Deselect", 0.4f);
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
                InfoBar.HideInfo();
                if (ChainSystem.UnitIsReacting())
                {
                    // Call IEnumerator as a normal function, ignoring 'yield return' instructions
                    ChainSystem.AddAction(hoverCell);
                }
                SoundFXManager.PlaySoundFXClip("Select", 0.2f);
                StartCoroutine(ConfirmAction());

            }
            else
            {
                SoundFXManager.PlaySoundFXClip("Deselect", 0.4f);
            }
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            InactiveState();
            if (ChainSystem.UnitIsReacting())
                CanvasUI.ShowTargetUnitInfoDisplay(ChainSystem.ReactingUnit.unitInfo);
            else
                CanvasUI.HideTargetUnitInfoDisplay();

            SetHoverCell(currentUnit);
            ChainSystem.ReleasePotentialChain();
            ActionUtility.HideAllSelectableTiles();
            SoundFXManager.PlaySoundFXClip("Deselect", 0.4f);


            StartCoroutine(UnitMenu.ShowMenu(TilemapCreator.UnitLocator[currentUnit]));
        }
    }

    private static IEnumerator ConfirmAction()
    {
        InactiveState();
        ActionUtility.HideAllSelectableTiles();
        yield return ChainSystem.AddAction(hoverCell);
        yield return ChainSystem.ExecuteChain();
        
        // Set 'currentUnit' back to the current unit in the turn system
        currentUnit = TurnSystem.CurrentUnit.unitInfo.Vector2CellLocation();
        //CameraMovement.SetFocusPoint(TilemapCreator.TileLocator[currentUnit].TileObj.transform);
        MoveCursor(currentUnit);
        CanvasUI.HideTargetUnitInfoDisplay();
        yield return UnitMenu.ShowMenu(TilemapCreator.UnitLocator[currentUnit]);
        InfoBar.DisplayInfo(InfoTabType.Action);
        
    }

    private void MoveCursorUp() {
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.up));
            ShowUnitInfoFromTile();
        }
    }
    
    private void MoveCursorDown() {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.down));
            ShowUnitInfoFromTile();
        }
    }
    
    private void MoveCursorLeft() {
        if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.left));
            ShowUnitInfoFromTile();
        }
    }
    
    private void MoveCursorRight() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) {
            MoveCursor(hoverCell + GetRelativeDirection(Vector2Int.right));
            ShowUnitInfoFromTile();
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
            SoundFXManager.PlaySoundFXClip("MapCursor", 0.75f);

        }
    }

    public static void StartMove(Vector2Int cell2D)
    {
        SetHoverCell(cell2D);
        currentUnit = cell2D;
        CanvasUI.ShowTurnUnitInfoDisplay(TilemapCreator.UnitLocator[cell2D].unitInfo);
        CursorControlState = ControlState.Active;
    }

    public static void EndMove()
    {
        RemoveTileOutline();
        CanvasUI.HideTurnUnitInfoDisplay();
        CanvasUI.HideTargetUnitInfoDisplay();
        InfoBar.HideInfo();
        hoverCell = Vector2Int.zero;
        currentUnit = Vector2Int.zero;
        TurnSystem.ContinueLoop();
    }

    public static void SetHoverCell(Vector2Int cell)
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

    public static void ShowUnitInfoFromTile()
    {
        if (TilemapCreator.UnitLocator.TryGetValue(hoverCell, out var foundUnit)) {
            switch (CursorControlState)
            {
                case ControlState.Start:
                    if(foundUnit.unitInfo.UnitAffiliation == UnitAffiliation.Enemy)
                        CanvasUI.ShowTargetUnitInfoDisplay(foundUnit.unitInfo);
                    break;
                case ControlState.Active:
                    if (foundUnit == TurnSystem.CurrentUnit)
                    {
                        CanvasUI.ShowTurnUnitInfoDisplay(foundUnit.unitInfo);
                        CanvasUI.HideTargetUnitInfoDisplay();
                    }
                    else
                        CanvasUI.ShowTargetUnitInfoDisplay(foundUnit.unitInfo);

                    break;
                case ControlState.Action:
                    CanvasUI.ShowTargetUnitInfoDisplay(foundUnit.unitInfo); 
                    break;
                default:
                    Debug.LogError("Control State not valid...");
                    break;
            }
        }
        else
        {
            if (CursorControlState == ControlState.Action)
            {
                CanvasUI.HideTargetUnitInfoDisplay();
            } 
            else
            {
                CanvasUI.HideTurnUnitInfoDisplay();
                CanvasUI.HideTargetUnitInfoDisplay();
            }
        }
    }


    public static ControlState GetState() { return CursorControlState; }
    public static void StartState() { CursorControlState = ControlState.Start; }
    public static void ActiveState() { CursorControlState = ControlState.Active; }
    public static void ActionState() { CursorControlState = ControlState.Action; }
    public static void InactiveState() { CursorControlState = ControlState.Inactive; }    
    public static void SetGameObjActive() { gameObj.SetActive(true); }
    public static void SetGameObjInactive() { RemoveTileOutline();  gameObj.SetActive(false); }
}
