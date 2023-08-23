using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickPhoneNumberTrigger : ClickInfoTrigger
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (!DataManager.Inst.SaveData.isProfilerInstall || !DataManager.Inst.SaveData.isProfilerDocument)
        {
            return;
        }

        string InformaitonText = ResourceManager.Inst.GetProfilerInfoData(infoDataIDList[0]).infomationText;
        EventManager.TriggerEvent(ECallEvent.AddAutoCompleteCallBtn, new object[1] { InformaitonText });
    }


}
