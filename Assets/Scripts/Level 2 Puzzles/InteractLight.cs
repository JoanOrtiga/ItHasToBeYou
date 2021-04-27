using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractLight : MonoBehaviour, IInteractable
{
    private void Start()
    {
        this.transform.GetChild(0).gameObject.SetActive(false);
    }
    public void Interact()
    {
        this.transform.GetChild(0).gameObject.SetActive(true);
    }
}
