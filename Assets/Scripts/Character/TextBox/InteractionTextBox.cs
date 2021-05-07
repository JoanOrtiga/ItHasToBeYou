using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractionTextBox : MonoBehaviour
{
    public GameObject textBox;
    public string text;
    public float textDuration;
    private bool doneText = false;

    TextBoxController player;

    private void Awake()
    {
        player = FindObjectOfType<TextBoxController>();
    }
    public void StartText()
    {
        if (doneText == false)
        {
            player.textBoxActive++;
            StopCoroutine(TextBox());
            StartCoroutine(TextBox());
        }
        
    }
    IEnumerator TextBox()
    {
        print("TEXT BOX");
        doneText = true;
        textBox.SetActive(true);
        textBox.gameObject.GetComponent<Text>().text = text;
        yield return new WaitForSeconds(textDuration);

        if (player.textBoxActive - 1 == 0)
        {
            textBox.SetActive(false);
        }
        player.textBoxActive --;

    }

}
