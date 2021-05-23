using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject options;
    int index;

    private void Start()
    {
        index = SceneManager.GetActiveScene().buildIndex;
    }

    public void StartGame()
    {
        //GameObject.FindObjectOfType<LoadNewScene>().LoadNextScene("LoadScreen");
        SceneManager.LoadScene(index + 1);
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
