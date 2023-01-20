using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TodoData
{
    // юс╫ц©К
    public string todoName;
    public string category;
    [SerializeField]
    private int successRate;

    public Action OnChangeSuccessRate;
    public int SuccessRate { get => successRate; }
    public void ChangeSuccessRate(int value)
    {
        successRate += value;
        OnChangeSuccessRate?.Invoke();
    }
}


[CreateAssetMenu(menuName ="SO/Todo/DataList")]
public class TodoDataListSO : ScriptableObject
{
    [SerializeField]
    private List<TodoData> todoDataList;

    public TodoData this[int idx]
    {
        get => todoDataList[idx];
    }

    public int Count => todoDataList.Count;
}
