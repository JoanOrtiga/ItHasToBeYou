using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    public GameObject pauseMenu;

    private void Awake()
    {
        pauseMenu.SetActive(false);
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && isPaused == false)
        {
            isPaused = true;
            pauseMenu.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else if (Input.GetKeyDown(KeyCode.P) && isPaused)
        {
            isPaused = false;
            pauseMenu.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;

        }

    

    }

    public void continueGame()
    {
        Time.timeScale = 1;
    }

    public void goToMainMenu()
    {
        Time.timeScale = 1;
    }

}
