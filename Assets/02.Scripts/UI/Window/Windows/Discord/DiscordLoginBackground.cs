using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscordLoginBackground : MonoBehaviour, IPointerDownHandler
{
    public Action OnIDPWPanelOff;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnIDPWPanelOff?.Invoke();
        }
    }
}
