using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuAttributeMenu : MenuAttributePanel, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.TriggerEvent(EEvent.ExpendMenu);
    }

}
