using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused;
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Escape) && isPaused )
        //{
        //    isPaused = false;
        //    Time.timeScale = 1;
        //}

        //if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        //{
        //    isPaused= true;
        //    Time.timeScale = 0;
        //}

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
