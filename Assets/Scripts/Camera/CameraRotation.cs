using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    private Transform focusPoint; // The point the camera will rotate around
    private const float ROTATION_ANGLE = 45f; // Degrees per key press
    private const float ROTATION_SPEED = 0.5f; // Smoothness of rotation
    private const float MOVE_SPEED = 0.1f; // Smoothness of movement


    void Update()
    {
        //transform.position = focusPoint.position;
        if (Input.GetKeyDown(KeyCode.Q))
            RotateCamera(-ROTATION_ANGLE);
        
        if (Input.GetKeyDown(KeyCode.E))
            RotateCamera(ROTATION_ANGLE);
        
    }

    void MoveCamera()
    {
        LeanTween.move(gameObject, focusPoint.position, MOVE_SPEED);
    }

    void RotateCamera(float angle)
    {
        if (!LeanTween.isTweening(gameObject))
            LeanTween.rotateAround(gameObject, Vector3.up, angle, ROTATION_SPEED);

        // Rotate around the focus point on the Y-axis
        //Transform newTransform = transform;
        //newTransform.RotateAround(focusPoint.position, Vector3.up, angle);

        //targetRotation = transform.rotation;
        //transform.rotation = Quaternion.Lerp(transform.rotation, newTransform.rotation, Time.deltaTime * rotationSpeed);
        //LeanTween.move(gameObject, newTransform.position, 3f);
        //LeanTween.rotate(gameObject, newTransform.rotation.eulerAngles, 3f);
        //LeanTween.clerp()
    }

    public void SetFocusPoint(Transform newFocusPoint)
    {
        focusPoint = newFocusPoint;
        MoveCamera();
    }
}
