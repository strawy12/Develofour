using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DecisionData
{
    public string decisionName;
    public bool isComplete;
}

[CreateAssetMenu(menuName = "SO/QuestData")]
public class QuestDataSO : ScriptableObject
{
    public string questName;
    public EQuestEvent questEvent;
    private int successRate = 0;
    public List<DecisionData> decisionClearList; 
    public Action OnChangeSuccessRate;
    public bool isClear;
    public int SuccessRate { get => successRate; }

    public void ChangeSuccessRate(int value)
    {
        successRate += value;
        successRate = Mathf.Clamp(successRate, 0, 100);
        OnChangeSuccessRate?.Invoke();
    }
}
