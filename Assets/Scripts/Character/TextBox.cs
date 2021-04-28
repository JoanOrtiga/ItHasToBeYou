using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextBox : MonoBehaviour
{
    public GameObject textBox;
    public string text;
    public float textDuration;

    private bool textTrigged;
    private float time;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            textBox.gameObject.transform.parent.gameObject.SetActive(true);
            textBox.GetComponent<Text>().text = text;
            textTrigged = true;
            time = 0;
        }
    }

    private void Update()
    {
        time += Time.deltaTime;
        if (textTrigged && time >= textDuration)
        {
            textBox.gameObject.transform.parent.gameObject.SetActive(false);
            Destroy(this.gameObject);
        }

    }
}
