using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour
{
    [SerializeField]
    private QuestDataSO questData;

    public QuestDataSO QuestData
    {
        get
        {
            return questData;
        }
    }

    [SerializeField]
    private List<Decision> decisionList;

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

        if (questData.isActive == false)
        {
            NoticeSystem.OnNotice.Invoke(questData.questText.head, questData.questText.body, null); // sprite 넣어야함
        }
        questData.isActive = true;
    }
    
    //private void ShowClearDecisionPanel(Decision decision) 
    //{
    //    NoticeSystem.OnGeneratedNotice.Invoke(EQuestEvent)
    //}

    private void ChangeRate()
    {
        SaveDecisionDatas();
        questData.OnChangeSuccessRate?.Invoke();
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
        questData.isClear = true;
        questData.isActive = false;

        Release();

        gameObject.SetActive(false);
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

    public void Release()
    {
        foreach (var decision in decisionList)
        {
            decision.OnChangedValue -= CheckClearQuest;
            decision.OnChangedValue -= ChangeRate;
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

    public void DebugReset()
    {
        questData.isClear = false;
        questData.isActive = false;
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
