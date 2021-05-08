using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{

    public bool isTrigger, isInteraction, isPickUp, isLook, isCompletePuzzle, isPlaceObject;
    public GameObject textBox;
    public string text;
    public float textDuration;

    private bool textDone;

    TextBoxController player;

    private void Awake()
    {
        player = FindObjectOfType<TextBoxController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && textDone == false && isTrigger)
        {
            textDone = true;
            player.textBoxActive++;
            StartCoroutine(TextBoxStart());
         
        }
    }

    public void StartText()
    {
        if (textDone == false)
        {
            player.textBoxActive++;
            StopCoroutine(TextBoxStart());
            StartCoroutine(TextBoxStart());
        }

    }

    IEnumerator TextBoxStart()
    {

        if (isTrigger)
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
        }

        print("TEXT BOX");
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
}
