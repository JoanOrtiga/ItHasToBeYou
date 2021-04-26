using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractBooks : MonoBehaviour , IInteractable
{
    [SerializeField] private bool bookCounts;
    private bool activated = false;

    public UnityEvent activating;
    public UnityEvent deActivating;
    public void Interact()
    {
        if (!activated)
        {
            transform.Rotate(new Vector3(-20f, 0, 0));
            activated = true;
            
            
            if(bookCounts)
                activating.Invoke();
            
            return;
        }
        
        transform.Rotate(new Vector3(+20f, 0, 0));
        activated = false;
        
        if(bookCounts)
            deActivating.Invoke();
    }
}
