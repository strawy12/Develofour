using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GetInformationTrigger : MonoBehaviour, IPointerClickHandler
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
    }
}
