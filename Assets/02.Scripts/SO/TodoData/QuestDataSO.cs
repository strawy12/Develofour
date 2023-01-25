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
    [Header("퀘스트 제목")] 
    public string head;

    // Notice에 들어갈 내용?
    [Header("퀘스트 짧은 소개")]
    [TextArea(1,3)]
    public string body;

    // TODO UI의 내용에 들어감
    [Header("퀘스트 자세한 소개")]
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
    public EQuestCategory category; // string으로 바꿀까 고민중

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
