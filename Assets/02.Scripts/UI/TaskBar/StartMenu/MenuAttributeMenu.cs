using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuAttributeMenu : MenuAttributePanel, IPointerClickHandler
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        EventManager.TriggerEvent(EWindowEvent.ExpendMenu);
    }
}
