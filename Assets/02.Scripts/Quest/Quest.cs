using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quest : MonoBehaviour
{
    [SerializeField]
    private QuestDataSO questData;

    private void Start()
    {
        foreach (var decisionData in questData.decisionDataList)
        {
            decisionData.decision.Init();
            decisionData.SettingDecisionClear();
        }

        if (CheckDecisions() || questData.isClear)
        {
            questData.ChangeSuccessRate(100);
            questData.isClear = true;
            Destroy(gameObject);
            return;
        }

        foreach (var decisionData in questData.decisionDataList)
        {
            decisionData.decision.OnChangedValue += CheckClearQuest;
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
        foreach (var decisionData in questData.decisionDataList)
        {
            if (!decisionData.decision.CheckDecision())
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
        foreach (var decisionData in questData.decisionDataList)
        {
            decisionData.isClaer = decisionData.decision.CheckDecision();
        }
    }
    public void OnApplicationQuit()
    {
        foreach (var decisionData in questData.decisionDataList)
        {
            decisionData.isClaer = decisionData.decision.CheckDecision();
        }
    }
}
