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
        if (CheckDecisions())
        {
            Destroy(gameObject);
            return;
        }

        foreach (var decision in decisionList)
        {
            decision.OnChangedValue += CheckClearQuest;
        }

    }

    private void CheckClearQuest()
    {
        if (CheckDecisions())
        {
            QuestClear();
        }
    }

    private bool CheckDecisions()
    {
        foreach (var decision in decisionList)
        {
            if (!decision.CheckDecision())
            {
                return false;
            }
        }

        return true;
    }

    private void QuestClear()
    {
        EventManager.TriggerEvent(currentEvent);
        Destroy(gameObject);
    }
}
