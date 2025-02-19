using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private Transform focusPoint; // The point the camera will rotate around
    private const float ROTATION_ANGLE = 45f; // Degrees per key press
    private const float ROTATION_SPEED = 0.4f; // Smoothness of rotation
    private const float MOVE_SPEED = 0.1f; // Smoothness of movement


    void Update()
    {
        //transform.position = focusPoint.position;
        if (Input.GetKeyDown(KeyCode.Q))
            RotateCamera(ROTATION_ANGLE);
        
        if (Input.GetKeyDown(KeyCode.E))
            RotateCamera(-ROTATION_ANGLE);
        
    }

    void MoveCamera()
    {
        LeanTween.move(this.gameObject, focusPoint.position, MOVE_SPEED);
    }

    void RotateCamera(float angle)
    {
        if (!LeanTween.isTweening(this.gameObject))
            //LeanTween.rotateAround(gameObject, Vector3.up, angle, ROTATION_SPEED);
            //LeanTween.rotateAround(gameObject, Vector3.up, angle, ROTATION_SPEED).setEaseInOutQuart();
            LeanTween.rotateAround(gameObject, Vector3.up, angle, ROTATION_SPEED).setEaseInOutCubic();

    }

    public void SetFocusPoint(Transform newFocusPoint)
    {
        focusPoint = newFocusPoint;
        MoveCamera();
    }
}
