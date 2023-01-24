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

[System.Serializable]
public class QuestText
{
    [Header("Äù½ºÆ® Á¦¸ñ")] 
    public string head;

    [Header("Äù½ºÆ® ÂªÀº ¼Ò°³")]
    [TextArea(1,3)]
    public string body;

    [Header("Äù½ºÆ® ÀÚ¼¼ÇÑ ¼Ò°³")]
    [TextArea(5,10)]
    public string detailBody;
}

public enum EQuestCategory
{
    None,
    End
}

[CreateAssetMenu(menuName = "SO/Quest/QuestData")]
public class QuestDataSO : ScriptableObject
{
    public EQuestEvent questEvent;
    public EQuestCategory category;

    public QuestText questText;

    private int successRate = 0;

    [HideInInspector]
    public List<DecisionData> decisionClearList; 
    public Action OnChangeSuccessRate;
    [HideInInspector]
    public bool isClear;
    public int SuccessRate { get => successRate; }

    public void ChangeSuccessRate(int value)
    {
        successRate += value;
        successRate = Mathf.Clamp(successRate, 0, 100);
        OnChangeSuccessRate?.Invoke();
    }
}
