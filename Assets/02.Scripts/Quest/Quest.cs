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
        Init();
        questData.isClear = false;
    }
    public void Init()
    {
        LoadQuestDatas();

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
            decision.OnChangedValue += ChangeRate;
            //decision.OnClearPanel += ShowClearDecisionPanel;
        }
    }
    //private void ShowClearDecisionPanel(Decision decision) 
    //{
    //    NoticeSystem.OnGeneratedNotice.Invoke(EQuestEvent)
    //}

    private void ChangeRate()
    {
        SaveDecisionDatas();
        questData.OnChangeSuccessRate.Invoke();
    }
    private void LoadQuestDatas()
    {
        foreach (Decision decision in decisionList)
        {
            decision.Init();
            DecisionData data = questData.decisionClearList.Find(x => x.decisionName == decision.decisionName);
            decision.isClear = data.isComplete;
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
        //Destroy(gameObject);
    }

    public void SaveDecisionDatas()
    {
        foreach (Decision decision in decisionList)
        {
            DecisionData data = questData.decisionClearList.Find(x => x.decisionName == decision.decisionName);
            data.isComplete = decision.CheckDecision();
        }
    }

    public void OnDestroy()
    {
        SaveDecisionDatas();
    }
    //public void OnApplicationQuit()
    //{
    //    SaveDecisionDatas();
    //}

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        questData.isClear = false;
        foreach(var data in questData.decisionClearList)
        {
            data.isComplete = false;
        }
        foreach(var data in decisionList)
        {
            data.isClear = false;
        }
    }
#endif
}
