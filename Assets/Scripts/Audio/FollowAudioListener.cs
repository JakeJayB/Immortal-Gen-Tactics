using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAudioListener : MonoBehaviour
{
    void Update() =>  gameObject.transform.position = Camera.main.transform.position;       
    
}
