using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HighlightBtn : HighlightUI, IPointerClickHandler
{
    public Action OnClick;

    public void Awake()
    {
        OnClick += ImmediatelyStop;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick?.Invoke();
    }
}
