using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickPhoneNumberTrigger : ClickInfoTrigger
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        EventManager.TriggerEvent(ECallEvent.AddAutoCompleteCallBtn, new object[1] { infomaitionDataList[0].infomationText });
    }


}
