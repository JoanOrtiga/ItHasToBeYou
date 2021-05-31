using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public string pathSoundFmod;

    public bool isTrigger, isInteraction, isPickUp, isLook, isCompletePuzzle, isPlaceObject, isTriggerWithAPreCondition, lookCloseObject;
    private GameObject textBox;
    public string text;
    public float textDuration;

   [HideInInspector] public bool textDone;
    public TextBox preTrigger;

    TextBoxController player;
    private bool activeTrigger;
    private void Awake()
    {
        textBox = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        player = FindObjectOfType<TextBoxController>();
        if (isTriggerWithAPreCondition)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && textDone == false && isTrigger)
        {
            textDone = true;
            player.textBoxActive++;
            StartCoroutine(TextBoxStart());
         
        }

        if (isTriggerWithAPreCondition && other.CompareTag("Player") && textDone == false  )
        {
            textDone = true;
            player.textBoxActive++;
            
            StartCoroutine(TextBoxStart());
        }
    }

    public void StartText()
    {
        if (textDone == false && !isCompletePuzzle)
        {
            player.textBoxActive++;
            StopCoroutine(TextBoxStart());
            StartCoroutine(TextBoxStart());
        }

        if (textDone == false && lookCloseObject)
        {
            player.textBoxActive++;
            StopCoroutine(TextBoxStart());
            StartCoroutine(TextBoxStart());
        }

    }


    public void StartTextPuzzle()
    {
        if (textDone == false && isCompletePuzzle)
        {
            player.textBoxActive++;
            StopCoroutine(TextBoxStart());
            StartCoroutine(TextBoxStart());
        }
    }
    IEnumerator TextBoxStart()
    {
        if (pathSoundFmod != "")
        {
            FMODUnity.RuntimeManager.PlayOneShot(pathSoundFmod, player.transform.position);
        }

        if (isTrigger ||isTriggerWithAPreCondition)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
       
        print("TEXT BOX");
        textDone = true;
        textBox.gameObject.SetActive(true);
        textBox.gameObject.GetComponent<Text>().text = text;
   
        print(textBox.gameObject.activeSelf);
        
        yield return new WaitForSeconds(textDuration);

        if (player.textBoxActive - 1 == 0)
        {
            textBox.gameObject.SetActive(false);
        }
        player.textBoxActive--;

        if (isTrigger || isLook || isTriggerWithAPreCondition)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator TextBoxPuzzle()
    {


        print("TEXT BOX PUZZLE");
        textDone = true;
        textBox.gameObject.SetActive(true);
        textBox.gameObject.GetComponent<Text>().text = text;

        yield return new WaitForSeconds(textDuration);

        if (player.textBoxActive - 1 == 0)
        { 
            textBox.gameObject.SetActive(false);
        }
        player.textBoxActive--;

        if (isTrigger || isLook)
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (isTriggerWithAPreCondition)
        {
            if (preTrigger.textDone)
            {
                gameObject.GetComponent<BoxCollider>().enabled = true;
            }
        }
    }
}
