using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/QuestData")]
public class QuestDataSO : ScriptableObject
{
    public string questName;
    public EQuestEvent questEvent;
    private int successRate = 0;
    public List<bool> decisionClearList;
    public Action OnChangeSuccessRate;
    public int SuccessRate { get => successRate; }
    public void ChangeSuccessRate(int value)
    {
        successRate += value;
        OnChangeSuccessRate?.Invoke();
    }
    
    public bool isClear;

    
}
