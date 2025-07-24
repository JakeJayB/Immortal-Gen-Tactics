using UnityEngine;

public class FollowAudioListener : MonoBehaviour {
    private void Start() {
        if (Camera.main) return;
        Debug.LogError("[FollowAudioListener]: Main Camera Doesn't Exist!");
        Destroy(this);
    }
    
    void Update() =>  gameObject.transform.position = Camera.main.transform.position;       
}
