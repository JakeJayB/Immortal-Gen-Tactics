using UnityEngine;
using UnityEngine.UIElements;

public class CameraMovement : MonoBehaviour
{
    private const float ROTATION_ANGLE = 90f; // Degrees per key press
    private const float ROTATION_SPEED = 0.7f; // Smoothness of rotation
    private const float STEEPEN_SPEED = 0.4f; // Smoothness of steepening
    private const float MOVE_SPEED = 0.2f; // Smoothness of movement


    private static GameObject MainCamera;

    private void Awake()
    {
        MainCamera = this.gameObject;
    }

    void Update()
    {
        if (MapCursor.GetState() == MapCursor.ControlState.Inactive) return;

        if (Input.GetKeyDown(KeyCode.Q))
            RotateCamera(ROTATION_ANGLE);
        else if (Input.GetKeyDown(KeyCode.E))
            RotateCamera(-ROTATION_ANGLE);
        else if(Input.GetKeyDown(KeyCode.W))
            SteepenCamera();
    }

    void RotateCamera(float angle)
    {
        if (!LeanTween.isTweening(gameObject))
        {
            LeanTween.rotateAround(gameObject, Vector3.up, angle, ROTATION_SPEED).setEase(LeanTweenType.easeInOutQuad).setOnComplete(() =>
            {
                CheckAndMove(TilemapCreator.TileLocator[MapCursor.hoverCell].TileObj.transform);
            });

        }
    }

    void SteepenCamera()
    {
        if (!LeanTween.isTweening(gameObject))
        {
            int currentAngle = Mathf.RoundToInt(transform.rotation.eulerAngles.x);
            if(currentAngle > 30)
            {
                LeanTween.rotateAroundLocal(gameObject, Vector3.right, -5f, STEEPEN_SPEED).setEase(LeanTweenType.easeInOutQuad);

            }
            else if(currentAngle <= 30)
            {
                LeanTween.rotateAroundLocal(gameObject, Vector3.right, 5f, STEEPEN_SPEED).setEase(LeanTweenType.easeInOutQuad);
            }

        }
    }

    public static void SetFocusPoint(Transform focusPoint)
    {
        LeanTween.move(MainCamera, focusPoint.position, MOVE_SPEED).setEase(LeanTweenType.easeInOutQuad);
    }

    public static void CheckAndMove(Transform point)
    {
        if (LeanTween.isTweening(MainCamera)) return;

        float minX = 0.3f;
        float maxX = 0.7f;
        float minY = 0.4f;
        float maxY = 0.7f;

        Vector3 screenPos = Camera.main.WorldToViewportPoint(point.position);

        Vector3 rightDir = MainCamera.transform.right;
        Vector3 upDir = MainCamera.transform.up;
        Vector3 moveDirection = Vector3.zero;
        float moveAmount = 0f;

        // Check horizontal movement
        if (screenPos.x < minX)
        {
            moveDirection -= rightDir;
            moveAmount += Mathf.Abs(minX - screenPos.x); 
        }
        else if (screenPos.x > maxX)
        {
            moveDirection += rightDir;
            moveAmount += Mathf.Abs(screenPos.x - maxX);
        }

        // Check vertical movement
        if (screenPos.y < minY)
        {
            moveDirection -= upDir;
            moveAmount += Mathf.Abs(minY - screenPos.y);
        }
        else if (screenPos.y > maxY)
        {
            moveDirection += upDir;
            moveAmount += Mathf.Abs(screenPos.y - maxY);
        }

        if (moveDirection != Vector3.zero)
        {
            Vector3 offset = Camera.main.ViewportToScreenPoint(moveDirection * moveAmount);
            Vector3 targetPosition = MainCamera.transform.position + (moveDirection * moveAmount * Camera.main.orthographicSize * 2f);
            LeanTween.move(MainCamera, targetPosition, MOVE_SPEED).setEase(LeanTweenType.easeInOutQuad);
        }
    }

}
