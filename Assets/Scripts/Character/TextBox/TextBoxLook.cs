using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBoxLook : MonoBehaviour
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

    public void textBoxStart()
    {

        if (doneText == false)
        {
            doneText = true;
            player.textBoxActive++;
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
        Destroy(this.gameObject);
        player.textBoxActive--;

    }

}
