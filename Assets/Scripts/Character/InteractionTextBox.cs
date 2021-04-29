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
    public void StartText()
    {
        if (doneText == false)
        {
            StartCoroutine("TextBox");
        }
        
    }
    IEnumerator TextBox()
    {
        textBox.gameObject.SetActive(true);
        textBox.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = text;
        yield return new WaitForSeconds(textDuration);
        textBox.gameObject.SetActive(false);
        doneText = true;
        

    }

}
