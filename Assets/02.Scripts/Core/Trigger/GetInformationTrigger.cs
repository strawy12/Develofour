using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GetInformationTrigger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public EProfileCategory category;
    public string information;
    public List<ProfileInfoTextDataSO> needInformaitonList;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!DataManager.Inst.IsProfileInfoData(category, information))
        {
            if (needInformaitonList.Count == 0)
            {
                EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, information, null });
            }
            else
            {
                EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, information, needInformaitonList });
            }
        }
        OnPointerEnter(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        Define.ChangeInfoCursor(needInformaitonList, category, information);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { CursorChangeSystem.ECursorState.Default });

    }
}
