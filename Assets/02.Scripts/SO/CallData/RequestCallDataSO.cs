using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MonologLockDecision
{
    public enum EDecisionType
    {
        Infomation,
        Monolog
    }

    public EDecisionType decisionType;
    [SerializeField]
    private string key;
}

[System.Serializable]
public class MonologLockData
{
    public List<MonologLockDecision> decisions;
    public int monologID;
}


[CreateAssetMenu(menuName = "SO/Call/RequestData")]
public class RequestCallDataSO : ScriptableObject
{
    public ECharacterDataType characterType;
    public List<MonologLockDecision> defaultDecisions;
    public List<MonologLockData> monologLockList;
}
