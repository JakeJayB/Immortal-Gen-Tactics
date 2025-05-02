using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void OnPlayButton()
    {
        
        int level = UnityEngine.Random.Range(1, 4);
        Debug.Log("Start Button Pressed");
        Debug.Log($"Loading Level {level}");
        SceneManager.LoadScene(level);
        
    }

    public void OnExitButton()
    {
        Debug.Log("Exit Button Pressed");
        Application.Quit();
    }

}
