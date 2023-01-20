using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Quest : MonoBehaviour
{
    [SerializeField]
    private QuestDataSO questData;

    private List<DecisionData> decisionDataList;

    private void Start()
    {
        decisionDataList = questData.decisionDataList;
        foreach (var decisionData in decisionDataList)
        {
            decisionData.decision.Init();
            decisionData.SettingDecisionClear();
        }

        if (CheckDecisions())
        {
            Destroy(gameObject);
            return;
        }
        foreach (var decisionData in decisionDataList)
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
        foreach (var decisionData in decisionDataList)
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
        foreach (var decisionData in decisionDataList)
        {
            decisionData.isClaer = decisionData.decision.CheckDecision();
        }
    }
    public void OnApplicationQuit()
    {
        foreach (var decisionData in decisionDataList)
        {
            decisionData.isClaer = decisionData.decision.CheckDecision();
        }
    }
}
