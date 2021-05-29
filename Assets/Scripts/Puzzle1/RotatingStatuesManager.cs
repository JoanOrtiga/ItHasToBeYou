using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingStatuesManager : MonoBehaviour , IPuzzleSolver
{
    [SerializeField] private RotateStatues[] statues;

    private GameObject player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    public bool Solved()
    {
        foreach (var statue in statues)
        {
            if (!statue.Solved())
                return false;
        }

        FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 1/Objetos/Estanteria se Abre", player.transform.position);
        CamaraShake.ShakeOnce(3, 3);
        return true;
    }
}
