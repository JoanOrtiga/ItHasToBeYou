using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxTrigger : MonoBehaviour
{
    public GameObject textBox;
    public string text;
    public float textDuration;

    private bool textTrigged;

    TextBoxController player;

    private void Awake()
    {
        player = FindObjectOfType<TextBoxController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && textTrigged == false)
        {
            textTrigged = true;
            player.textBoxActive++;
            StartCoroutine(TextBox());
         
        }
    }
    IEnumerator TextBox()
    {
        print("TEXT BOX");
        textBox.gameObject.SetActive(true);
        textBox.gameObject.GetComponent<Text>().text = text;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        yield return new WaitForSeconds(textDuration);

        if (player.textBoxActive - 1 == 0)
        {
            textBox.gameObject.SetActive(false);
        }
        player.textBoxActive--;
    }
}
