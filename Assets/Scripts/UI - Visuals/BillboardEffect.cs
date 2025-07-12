using UnityEngine;

public class BillboardEffect : MonoBehaviour
{
    private Camera MainCamera;
    
    // Start is called before the first frame update
    void Start() { MainCamera = Camera.main; }

    // Update is called once per frame
    void Update() {
        if (MainCamera) {
            Vector3 cameraPosition = MainCamera.transform.position;
            cameraPosition.y = gameObject.transform.position.y;
            transform.LookAt(cameraPosition);
        }
    }
}
