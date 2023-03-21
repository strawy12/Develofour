using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GetInformationTrigger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public EProfileCategory category;
    public string information;
    public List<string> needStringList;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(needStringList.Count == 0)
        {
            EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, information, null });
        }
        else
        {
            EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, information, needStringList });
        }

        OnPointerEnter(eventData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        CursorChangeSystem.ECursorState state = CursorChangeSystem.ECursorState.Default;

        if (DataManager.Inst.IsProfileInfoData(category, information))
        {
            state = CursorChangeSystem.ECursorState.FoundInfo;
        }
        else
        {
            state = CursorChangeSystem.ECursorState.FindInfo;
        }

        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { state });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { CursorChangeSystem.ECursorState.Default });

    }
}