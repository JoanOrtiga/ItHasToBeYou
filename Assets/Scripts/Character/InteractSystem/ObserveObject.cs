using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserveObject : MonoBehaviour
{
    [HideInInspector]
    public bool isObserving = false;
    public bool hasText;
    public string text;

    public GameObject popUp;

    private void Start()
    {
       // popUp = gameObject.transform.GetChild(1).gameObject;
    }
    public void DisablePopUp(bool Disable)
    {
        if (Disable)
        {
            popUp.gameObject.SetActive(false);
        }
        else
        {
            popUp.gameObject.SetActive(true);
        }
    }
}
