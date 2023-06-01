using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuAttributeSetting : MenuAttributePanel, IPointerClickHandler
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
       // EventManager.TriggerEvent(EEvent.ClickSettingBtn);
    }

    
}
