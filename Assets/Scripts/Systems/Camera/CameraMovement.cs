using PixelCamera;
using UnityEditor;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    private static GameObject MainCamera;
    private const float ROTATION_ANGLE = 90f;
    private const float ROTATION_SPEED = 0.7f;
    private const float MOVE_SPEED = 0.2f;
    private const float ZOOM_SPEED = 0.4f;
    private bool isZoomedOut = true;

    private void Awake() {
        MainCamera = gameObject;
    }

    void Update() {
        if (MapCursor.GetState() == ControlState.Inactive) return;
        if (Input.GetKeyDown(KeyCode.Q)) RotateCamera(ROTATION_ANGLE);
        else if (Input.GetKeyDown(KeyCode.E)) RotateCamera(-ROTATION_ANGLE);
        else if(Input.GetKeyDown(KeyCode.W)) ToggleZoom();
    }

    private static void Clear() { MainCamera = null; }
    public static void RegisterCleanup() { MemoryManager.AddListeners(Clear); }
    
    private void RotateCamera(float angle) {
        if (LeanTween.isTweening(gameObject)) return;
        Transform hoverTile = TileLocator.SelectableTiles[MapCursor.hoverCell].TileObj.transform;
        LeanTween.rotateAround(gameObject, Vector3.up, angle, ROTATION_SPEED)
                .setEase(LeanTweenType.easeInOutQuad)
                .setOnComplete(() => { CheckAndMove(hoverTile); });
    }


    public void ToggleZoom() {
        if (LeanTween.isTweening(gameObject)) return;

        float zoomMin = 2f;
        float zoomMax = 2.5f;

        var cam = Camera.main.GetComponent<PixelCameraManager>();
        float startSize = cam.GameCameraZoom;
        float targetSize = isZoomedOut ? zoomMin : zoomMax;

        LeanTween.value(gameObject, startSize, targetSize, ZOOM_SPEED)
                 .setOnUpdate(size => cam.GameCameraZoom = size)
                 .setEase(LeanTweenType.easeInOutQuad)
                 .setOnComplete(() => isZoomedOut = !isZoomedOut);
    }


    public static void CheckAndMove(Transform point) {
        if (LeanTween.isTweening(MainCamera)) return;

        // viewport bounds
        const float minX = 0.3f, maxX = 0.7f;
        const float minY = 0.4f, maxY = 0.7f;

        Camera cam = Camera.main;
        Vector3 vp = cam.WorldToViewportPoint(point.position);

        // clamp to your bounds
        float cx = Mathf.Clamp(vp.x, minX, maxX);
        float cy = Mathf.Clamp(vp.y, minY, maxY);

        // if the point is outside, we'll move the camera
        if (vp.x != cx || vp.y != cy) {
            // build a viewport position that lies on the boundary
            Vector3 clampedVP = new Vector3(cx, cy, vp.z);

            // convert that viewport�]boundary back into world space
            Vector3 worldAtBoundary = cam.ViewportToWorldPoint(clampedVP);

            // figure out how far to shift the camera so that our point
            // will land exactly on that boundary spot
            Vector3 offset = point.position - worldAtBoundary;
            Vector3 targetCamPos = MainCamera.transform.position + offset;

            LeanTween.move(MainCamera, targetCamPos, MOVE_SPEED)
                    .setEase(LeanTweenType.easeInOutQuad);
        }
    }

    public static void SetFocusPoint(Transform focusPoint) {
        LeanTween.move(MainCamera, focusPoint.position, MOVE_SPEED).setEase(LeanTweenType.easeInOutQuad);
    }


}
