using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextError : MonoBehaviour
{
    private void Update()
    {
        print(this.gameObject.GetComponent<Text>().text);
        
    }
}
