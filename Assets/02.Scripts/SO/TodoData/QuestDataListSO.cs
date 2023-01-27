using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/Quest/QuestList")]
public class QuestDataListSO : ScriptableObject
{
    [SerializeField]
    private List<QuestDataSO> todoDataList;

    public QuestDataSO this[int idx]
    {
        get => todoDataList[idx];
    }

    public int Count => todoDataList.Count;
}
