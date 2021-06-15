using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseHover : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    private Text text;
    [SerializeField] private Color finalColor = Color.yellow;
    private Color initialColor;
    
    private void Awake()
    {
        text = GetComponent<Text>();
        initialColor = text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.color = finalColor;
    }

    public void ResetColor()
    {
        text.color = initialColor;
    }
 
    public void OnPointerExit(PointerEventData eventData)
    {
        text.color = initialColor;
    }
}
