using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ClickText : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private EDecisionEvent decisionEvent;

    private bool isComplete = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isComplete)
        {
            Debug.Log("Click");
            isComplete = true;
            EventManager.TriggerEvent(decisionEvent);
        }
    }
}
