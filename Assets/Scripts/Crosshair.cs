using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    private Transform player;

    [SerializeField] private Material observe;
    [SerializeField] private Material normal;

    public float normalDistance;
    public float closeDistance;
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<CameraController>().gameObject.transform.GetChild(0);
    }

   
    void Update()
    {
        Debug.DrawLine(player.position, gameObject.transform.position);
        float distace = Vector3.Distance(player.transform.position, gameObject.transform.position);

        if (distace < closeDistance)
        {
            gameObject.GetComponent<MeshRenderer>().material = observe;
        }
        else if (distace < normalDistance)
        {
            gameObject.GetComponent<MeshRenderer>().material = normal;
        }
        transform.LookAt(player);
    }
}
