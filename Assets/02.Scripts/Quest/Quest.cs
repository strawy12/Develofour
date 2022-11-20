using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quest : MonoBehaviour
{
    [SerializeField]
    private EQuestEvent currentEvent; 

    [SerializeField]
    private List<Decision> decisionList;


    private void Start()
    {
        foreach (var decision in decisionList)
        {
            decision.OnChangedValue += CheckClearQuest;
        }
    }

    private void CheckClearQuest()
    {
        foreach (var decision in decisionList)
        {
            if (!decision.CheckDecision())
            {
                return;
            }
        }

        QuestClear();
    }

    private void QuestClear()
    {
        //EventManager.TriggerEvent(EEvent.HateBtnClicked);
        EventManager.TriggerEvent(EEvent.Quest_LoginGoogle);

        //EventManager.TriggerEvent(currentEvent);
        Destroy(gameObject);
    }
}
