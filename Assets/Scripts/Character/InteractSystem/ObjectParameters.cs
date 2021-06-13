using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectParameters : MonoBehaviour
{
    public string pickUpPath;
    public string dropPath;

    public bool hasBeenPlaced;
    public GameObject popUp;

    [HideInInspector] public bool isStartPlace = true;

    public Vector3 startPos { get; private set; }
    [HideInInspector] public Quaternion startRot;
    

     [Header("Animation")]
    [SerializeField] private bool changeAnim;
    [SerializeField] private string animParam;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private PickUp pickUp;
    private void Awake()
    {
        startPos = transform.position;
        startRot = transform.rotation;
    }

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
        pickUp = FindObjectOfType<PickUp>();
    }

    public void ReLocate()
    {
        transform.position = startPos;
        transform.rotation = startRot;
    }
    public void DisablePopUp(bool Disable)
    {
        popUp.gameObject.SetActive(!Disable);

        if(changeAnim)
            playerController.AnimatorSetBool(animParam, Disable);
    }
}
