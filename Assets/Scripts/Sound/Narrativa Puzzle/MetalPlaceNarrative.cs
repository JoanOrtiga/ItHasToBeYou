using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetalPlaceNarrative : MonoBehaviour
{
    public BalanceController balanceController;

    public TextBox placeMatirialCorrect;
    public TextBox placeMatirialWrong;
    public TextBox placeMatirialWithOutPuzzle;

    private bool playSoundOne =false;
    private bool playSoundTwo = false;
    private bool playSoundThree = false;
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
            print("NO TRY THE PUZZLE");
            playSoundOne = true;
            placeMatirialWithOutPuzzle.StartTextPuzzle();
            
        }
        else if (balanceController.haveTryPuzzle == true && playSoundThree == false && placeMaterial.correctMaterial == false && placeMaterial.hasBeenPlaced)
        {
            playSoundThree = true;
            placeMatirialWrong.StartTextPuzzle();
            print("WRONG MATIRIAL");
            placeMatirialWrong.textDone = false;
        }
        else if(balanceController.haveTryPuzzle == true && playSoundTwo == false && placeMaterial.correctMaterial && placeMaterial.hasBeenPlaced)
        {
            print("CORRECT MATIRIAL");
            playSoundTwo = true;
            placeMatirialCorrect.StartTextPuzzle();
        }
        

        if (placeMaterial.hasBeenPlaced == false)
        {
            playSoundThree = false;
        }
    }
}
