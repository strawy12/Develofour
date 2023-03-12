using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscordArea : MonoBehaviour, IPointerDownHandler
{
    public Action OnAttributePanelOff;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            OnAttributePanelOff?.Invoke();
        }
    }
}
