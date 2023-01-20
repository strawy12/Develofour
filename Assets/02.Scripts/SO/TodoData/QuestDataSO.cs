using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionData
{
    public string decisionName;
    public Decision decision;
    public bool isClaer;

    public void SettingDecisionClear()
    {
        decision.SettingClear(isClaer);
    }
}


[CreateAssetMenu(menuName = "SO/QuestData")]
public class QuestDataSO : ScriptableObject
{
    public string questName;
    public List<DecisionData> decisionDataList;
    public EQuestEvent questEvent;
    private int successRate = 0;

    public Action OnChangeSuccessRate;
    public int SuccessRate { get => successRate; }
    public void ChangeSuccessRate(int value)
    {
        successRate += value;
        OnChangeSuccessRate?.Invoke();
    }
    
    public bool isClear;

    
}
