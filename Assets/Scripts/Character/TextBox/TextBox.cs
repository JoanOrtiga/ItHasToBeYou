using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    [Header("Text One")]
    public string textOne;
    public string pathSoundFmodOne;

    public bool isTrigger, isInteraction, isPickUp, isLook, isCompletePuzzle, isPlaceObject, isTriggerWithAPreCondition, lookCloseObject;
    private GameObject textBox;

    public float textDuration;

    public bool textDone;
    public TextBox preTrigger;

    TextBoxController player;
    private bool activeTrigger;


    VoiceSoundManager voiceSoundManager;

    private void Awake()
    {
        textBox = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        player = FindObjectOfType<TextBoxController>();
        if (isTriggerWithAPreCondition)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;

        }
        if (pathSoundFmodOne != "")
        {
            voiceSoundManager = FindObjectOfType<VoiceSoundManager>();

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

        if (isTriggerWithAPreCondition && other.CompareTag("Player") && textDone == false)
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


    }


    public void StartTextGetClose()
    {
        if (textDone == false)
        {

            player.textBoxActive++;
            textDone = true;
            StopCoroutine(TextCloseFace());
            StartCoroutine(TextCloseFace());

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
        if (pathSoundFmodOne != "")
        {
            voiceSoundManager.Sound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);


            voiceSoundManager.Sound = FMODUnity.RuntimeManager.CreateInstance(pathSoundFmodOne);
            voiceSoundManager.Sound.start();
        }

        if (isTrigger || isTriggerWithAPreCondition)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }


        if (!isPickUp && !lookCloseObject)
        {

            textDone = true;
        }

        textDone = true;
        textBox.gameObject.SetActive(true);
        textBox.gameObject.GetComponent<Text>().text = textOne;


        yield return new WaitForSeconds(textDuration);

        if (player.textBoxActive - 1 == 0)
        {
            textBox.gameObject.SetActive(false);
        }
        player.textBoxActive--;

        if (isTrigger || isLook || isTriggerWithAPreCondition)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }
    }


    IEnumerator TextCloseFace()
    {

        if (pathSoundFmodOne != "")
        {
            //FMODUnity.RuntimeManager.PlayOneShot(pathSoundFmodTwo, player.transform.position);
        }

        textBox.gameObject.SetActive(true);


        yield return new WaitForSeconds(textDuration);

        if (player.textBoxActive - 1 == 0)
        {
            textBox.gameObject.SetActive(false);
        }
        player.textBoxActive--;
    }


    IEnumerator TextBoxPuzzle()
    {


        textDone = true;
        textBox.gameObject.SetActive(true);
        
        

        yield return new WaitForSeconds(textDuration);

        if (player.textBoxActive - 1 == 0)
        {
            textBox.gameObject.SetActive(false);
        }
        player.textBoxActive--;

        if (isTrigger || isLook)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
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
