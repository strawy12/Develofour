using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscordArea : MonoBehaviour, IPointerClickHandler
{
    public Action OnAttributePanelOff;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            OnAttributePanelOff?.Invoke();
        }
    }
}
