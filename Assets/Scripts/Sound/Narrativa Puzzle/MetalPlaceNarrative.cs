using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalPlaceNarrative : MonoBehaviour
{
    public BalanceController balanceController;

    public TextBox placeMatirialCorrect;
    public TextBox placeMatirialWithOutPuzzle;

    private bool playSoundOne =false;
    private bool playSoundTwo = false;
    PlaceMaterial placeMaterial;
    void Start()
    {
        placeMaterial = gameObject.GetComponent<PlaceMaterial>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (balanceController.haveTryPuzzle == false && playSoundOne == false && placeMaterial.hasBeenPlaced)
        {
            print("SOUNDS PLACE 1");
            playSoundOne = true;
            placeMatirialWithOutPuzzle.StartText();
            
        }
        else if(balanceController.haveTryPuzzle == true && playSoundTwo == false && placeMaterial.hasBeenPlaced)
        {
            print("SOUNDS PLACE 2");

            playSoundTwo = true;
            placeMatirialCorrect.StartText();
        }
    }
}
