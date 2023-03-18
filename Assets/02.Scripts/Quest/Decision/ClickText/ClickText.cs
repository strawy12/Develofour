using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class ClickText : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private EDecisionEvent decisionEvent;
    [SerializeField]
    private List<EProfileCategory> categories = new List<EProfileCategory>();
    [SerializeField]
    private string infoName;

    private bool isComplete = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isComplete)
        {
            Debug.Log("Click");
            isComplete = true;
            EventManager.TriggerEvent(decisionEvent);
            foreach (var category in categories)
            {
                EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, infoName , null});
            }
        }
    }
}
