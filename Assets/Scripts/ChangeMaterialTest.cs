using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeMaterialTest : MonoBehaviour
{
    private bool isTileBlue = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !isTileBlue)
        {
            Debug.Log("test");
            isTileBlue = true;
        }
        else if(Input.GetKeyDown(KeyCode.Return) && isTileBlue)
        {
            Debug.Log("test2");
            isTileBlue = false;
        }
    }
}
