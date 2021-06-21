using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LockBookShelf : MonoBehaviour
{
    [SerializeField] private int booksToOpen = 3;
    private int booksCounter = 0;

    [SerializeField] private InteractBooks[] interactBooks;

    public UnityEvent solved;

    public void AddBook()
    {
        booksCounter++;

        if (booksCounter >= booksToOpen)
        {
            LockBooks();
        }
    }

    public void SubstractBook()
    {
        booksCounter--;

        if (booksCounter >= booksToOpen)
        {
            LockBooks();
        }
    }


    private void LockBooks()
    {
        foreach (var book in interactBooks)
        {
            book.gameObject.layer = LayerMask.NameToLayer("Default");
            book.enabled = false;
            book.tag = "Untagged";
        }

        solved.Invoke();

        //AQUI VA EL SO DE BOOKSHELF COMPLETED

        FMODUnity.RuntimeManager.PlayOneShot("event:/INGAME/Puzzle 1/BookComplete");
    }
}