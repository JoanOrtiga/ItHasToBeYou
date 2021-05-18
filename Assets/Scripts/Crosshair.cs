using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Transform player;
    private float distance;
    private Animator crosshairAnimator;
    [SerializeField] public AnimationCurve animation;



    void Start()
    {
        crosshairAnimator = GetComponent<Animator>();
        player = FindObjectOfType<CameraController>().gameObject.transform.GetChild(0);
        
    }  

    void Update()
    {
        distance = Vector3.Distance(player.transform.position, gameObject.transform.position);
    
        crosshairAnimator.SetFloat("Distance", animation.Evaluate(distance));   
        transform.LookAt(2 * transform.position - player.position);
        
    }
}
