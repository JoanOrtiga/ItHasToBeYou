using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasTutorial : MonoBehaviour
{
    public GameObject drop;

    [Header("Level 1")]
    public GameObject puzzle11;

    [Header("Level 2")]
    public GameObject puzzle21;
    public GameObject puzzle22;

    [Header("Level 3")]
    public GameObject puzzle31;
    public GameObject puzzle32;
    public GameObject puzzle33;


    public void TutorialPuzzle11(bool changeState)
    {
        puzzle11.SetActive(changeState ? true : false);
    }

    public void TutorialPuzzle21(bool changeState)
    {
        puzzle21.SetActive(changeState ? true : false);
    }

    public void TutorialPuzzle22(bool changeState)
    {
        puzzle22.SetActive(changeState ? true : false);
    }

   
    
}
