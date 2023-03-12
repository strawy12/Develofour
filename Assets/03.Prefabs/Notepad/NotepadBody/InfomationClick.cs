using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InfomationClick : MonoBehaviour, IPointerClickHandler
{
    public EProfileCategory category;
    public string information;

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.TriggerEvent(
             EProfileEvent.FindInfoText,
             new object[2] { category, information });
    }
}
