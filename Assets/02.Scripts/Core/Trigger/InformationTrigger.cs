using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constant.ProfilerInfoKey;

[System.Serializable]
public class NeedInfoData
{
    public string needInfoID;
    public string monologID;
    // needInfoID를 충족하지 않더라도 즉시 정보를 획득
    public bool getInfo;
}

public class InformationTrigger : MonoBehaviour
{
    [SerializeField]
    protected string triggerID;

    protected TriggerDataSO triggerData;
    public TriggerDataSO TriggerData => triggerData;

    protected string playMonologType = "";

    //public string MonologID => triggerData.monoLogType;
    //public List<int> infoDataIDList;
    //public int fileID;
    //public List<NeedInfoData> needInfoList;
    ////public int fileID
    //[SerializeField] protected int monoLogType;
    //public int completeMonologType = 0;
    //public float delay;
    //public bool isFakeInfo;

    protected virtual void Awake()
    {
        if (!DataLoadingScreen.completedDataLoad)
        {
            GameManager.Inst.OnStartCallback += Bind;
        }
        else
        {
            Bind();
        }
    }

    protected virtual void Bind()
    {
        if (triggerData == null)
        {
            triggerData = ResourceManager.Inst.GetResource<TriggerDataSO>(triggerID);
            if (triggerData == null)
            {
                return;
            }
        }
    }

    public void FindInfo()
    {
        Bind();

        if (GameManager.Inst.GameState == EGameState.Tutorial_Chat)
        {
            MonologSystem.OnStartMonolog(Constant.MonologKey.TUTORIAL_CANNOT_GETINFO, false);
            return;
        }
        if (GameManager.Inst.GameState == EGameState.Tutorial_Call)
        {
            MonologSystem.OnStartMonolog(Constant.MonologKey.TUTORIAL_CANNOT_GETINFO_CALL, false);
            return;
        }


        if (!CheckAllInfoFound())
        {
            bool playMonolog = false;

            foreach (var infoID in triggerData.infoDataIDList)
            {
                playMonolog = true;
                if (!triggerData.isFakeInfo)
                {
                    EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[1] { infoID });
                }
            }

            if (!playMonolog)
            {
                MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.TUTORIAL_NOT_FIND_INFO, false);
                return;
            }
        }
        else
        {
            if (!string.IsNullOrEmpty(triggerData.completeMonologType))
            {
                playMonologType = triggerData.completeMonologType;
            }
        }
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.FindInfoTrigger);
        MonologSystem.OnStartMonolog?.Invoke(playMonologType, false);


    }

    public void GetInfo()
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall || !DataManager.Inst.IsStartProfilerTutorial()) return;

        if (triggerData.infoDataIDList.Count == 0 || triggerData.infoDataIDList == null)
        {
            MonologSystem.OnStartMonolog?.Invoke(triggerData.monoLogType, false);
            return;
        }
        playMonologType = triggerData.monoLogType;

        foreach (NeedInfoData needData in triggerData.needInfoList)
        {
            // 이것이 -1이라면 즉시 정보 획득을 하라는 의미
            if (needData.getInfo)
            {
                playMonologType = needData.monologID;
                break;
            }

            if (!DataManager.Inst.IsProfilerInfoData(needData.needInfoID))
            {
                string id = string.IsNullOrEmpty(needData.needInfoID) ? Constant.MonologKey.NEEDINFO : needData.monologID;
                Debug.Log(id);
                MonologSystem.OnStartMonolog?.Invoke(id, false);
                Sound.OnPlaySound?.Invoke(Sound.EAudioType.LockInfoTrigger);
                return;
            }

        }
        FindInfo();
    }


    protected bool CheckAllInfoFound()
    {
        foreach (var infoID in triggerData.infoDataIDList)
        {
            if (!DataManager.Inst.IsProfilerInfoData(infoID))  // 찾은 정보인지 확인
            {
                return false;
            }
        }
        return true;
    }
}

