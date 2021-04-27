using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleDoor1 : MonoBehaviour
{
    private int booksCounter = 0;
    [SerializeField] private int booksToOpen = 3;

    private bool opening = false;

    [SerializeField] private Transform endTransform;

    [SerializeField] private float speed = 50f;

    private void Update()
    {
        if (opening)
        {
            Open();
        }
    }

    public void AddBook()
    {
        booksCounter++;

        print(booksCounter);
        
        if (booksCounter >= booksToOpen)
        {
            opening = true;
        }
    }
    
    public void SubstractBook()
    {
        booksCounter--;
    }

    private void Open()
    {
        transform.position = Vector3.Lerp(transform.position, endTransform.position, speed * Time.deltaTime);
    }
}
