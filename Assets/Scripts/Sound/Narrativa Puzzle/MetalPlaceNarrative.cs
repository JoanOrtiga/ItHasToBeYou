using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalPlaceNarrative : MonoBehaviour
{
    public BalanceController balanceController;

    public TextBox placeMatirialCorrect;
    public TextBox placeMatirialWithOutPuzzle;

    private bool playSoundOne;
    private bool playSoundTwo;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (balanceController.haveTryPuzzle == false && playSoundOne == false)
        {
            if (placeMatirialWithOutPuzzle != null)
            {
                playSoundOne = true;
                placeMatirialWithOutPuzzle.StartTextPuzzle();
            }
        }
        else if(balanceController.haveTryPuzzle == true && playSoundTwo == false)
        {
            playSoundOne = true;
            placeMatirialCorrect.StartTextPuzzle();
        }
    }
}
