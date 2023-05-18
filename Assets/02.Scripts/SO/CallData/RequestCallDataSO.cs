using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[System.Serializable]
public class MonologLockDecision
{
    public enum EDecisionType
    {
        Infomation,
        Monolog
    }

    public EDecisionType decisionType;
    public int key;
}

[System.Serializable]
public class MonologLockData
{
    public List<MonologLockDecision> decisions;
    public int monologID;
    public ReturnMonologData returnMonologData;
}

[System.Serializable]
public class ReturnMonologData
{
    [SerializeField]
    private int returnMonologID;
    [SerializeField]
    private int returnDelay;

    public List<MonologLockDecision> decisions;

    private int endDelayTime;
    private ECharacterDataType characterType;

    public int MonologID => returnMonologID;
    public int Delay => returnDelay;

    public int EndDelayTime => endDelayTime;
    public ECharacterDataType CharacterType => characterType;

    public ReturnMonologData(ECharacterDataType type, int returnMonologID, int returnDelay, List<MonologLockDecision> decisions)
    {
        characterType = type;
        this.returnMonologID = returnMonologID;
        this.returnDelay = returnDelay;
        this.decisions = decisions;
    }
    public void SetEndDelayTime()
    {
        // DataManager 기준으로 딜레이가 끝나느 시간을 저장시켜줄겁니다.
        endDelayTime = DataManager.Inst.GetCurrentTime() + returnDelay;
    }
}

[CreateAssetMenu(menuName = "SO/Call/RequestData")]
public class RequestCallDataSO : ScriptableObject
{
    public ECharacterDataType characterType;
    public int defaultMonologID;
    public int notExistMonoLogID;

    public List<MonologLockDecision> defaultDecisions;
    public List<MonologLockData> monologLockList;
}
