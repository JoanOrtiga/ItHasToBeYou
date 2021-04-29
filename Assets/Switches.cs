using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switches : MonoBehaviour
{
    [SerializeField] private Transform top;
    [SerializeField] private Transform mid;
    [SerializeField] private Transform bot;

    [SerializeField] private float speed = 1.0f;
    
    private enum SwitchesPosition
    {
        bot, mid, top
    }

    [SerializeField] private SwitchesPosition currentPosition;

    public UnityEvent goingUp;
    public UnityEvent goingDown;

    public bool Solved()
    {
        return currentPosition == SwitchesPosition.mid;
    }
    
    public void GoUp(bool button)
    {
        if (currentPosition != SwitchesPosition.top)
        {
            currentPosition = (SwitchesPosition)((int)currentPosition + 1);
            if(button)
                    goingUp.Invoke();
        }

    }

    public void GoDown(bool button)
    {
        if (currentPosition != SwitchesPosition.bot)
        {
            currentPosition = (SwitchesPosition)((int)currentPosition - 1);
            if(button)
                goingDown.Invoke();
        }
    }

    private void Update()
    {
        switch (currentPosition)
        {
            case SwitchesPosition.top:
                transform.position = Vector3.Lerp(transform.position, top.position, speed * Time.deltaTime);
                break;
            case SwitchesPosition.mid:
                transform.position = Vector3.Lerp(transform.position, mid.position, speed * Time.deltaTime);
                break;
            case SwitchesPosition.bot:
                transform.position = Vector3.Lerp(transform.position, bot.position, speed * Time.deltaTime);
                break;
        }
    }
}
