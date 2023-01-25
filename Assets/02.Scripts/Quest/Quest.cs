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
        LoadQuestData();

        if (CheckDecisions() || questData.isClear)
        {
            //questData.ChangeSuccessRate(100);
            questData.isClear = true;
            Destroy(gameObject);
            return;
        }

        foreach (var decision in decisionList)
        {
            decision.OnChangedValue += CheckClearQuest;
            decision.OnClearPanel += ShowClearDecisionPanel;
        }

    }
    private void ShowClearDecisionPanel(Decision decision) 
    {
        
    }

    private void LoadQuestData()
    {
        for (int i = 0; i < decisionList.Count; i++)
        {
            decisionList[i].Init();
            decisionList[i].isClear = questData.decisionClearList[i];
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
