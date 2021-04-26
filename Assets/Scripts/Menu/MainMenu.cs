using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public GameObject options;
    
    public void StartGame()
    {
        GameObject.FindObjectOfType<LoadNewScene>().LoadNextScene("LoadScreen");
    }

    public void ShowOptions()
    {
        options.SetActive(true);
    }

    public void HideOptions()
    {
        options.SetActive(false);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
