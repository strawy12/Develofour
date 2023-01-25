using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
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

    public List<DecisionData> decisionClearList; 
    public Action OnChangeSuccessRate;
    public bool isClear;

    public float CalcRate()
    {
        var clearList = decisionClearList.Where((x) => x.isComplete).ToList();
        int count = clearList.Count;
        if(decisionClearList.Count > 0) {
            Debug.Log($"clearList : {clearList.Count}, decisionClearList : {decisionClearList.Count}");
            float rate = count / decisionClearList.Count;
            Debug.Log(rate);
            return rate;
        }
        return 0;
    }
}
