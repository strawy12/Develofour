using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quest : MonoBehaviour
{
    [SerializeField]
    private QuestDataSO questData;

    public List<Decision> decisionList;

    private void Start()
    {
        for (int i = 0; i < decisionList.Count; i++)
        {
            decisionList[i].Init();
            decisionList[i].isClear = questData.decisionClearList[i]; 
        }

        if (CheckDecisions() || questData.isClear)
        {
            questData.ChangeSuccessRate(100);
            questData.isClear = true;
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
        EventManager.TriggerEvent(questData.questEvent);
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
       for(int i = 0; i< decisionList.Count; i++)
        {
            questData.decisionClearList[i] = decisionList[i].CheckDecision();

        }
    }
    public void OnApplicationQuit()
    {
        for (int i = 0; i < decisionList.Count; i++)
        {
            questData.decisionClearList[i] = decisionList[i].CheckDecision();
        }
    }
}
