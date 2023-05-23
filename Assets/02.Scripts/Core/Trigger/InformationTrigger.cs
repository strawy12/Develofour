using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NeedInfoData
{
    public int needInfoID;
    public int monologID;
    // needInfoID를 충족하지 않더라도 즉시 정보를 획득
    public bool getInfo;
}

public class InformationTrigger : MonoBehaviour
{
    /// <summary>
    /// 정보들 id들 넣는 곳입니다.
    /// </summary>
    [SerializeField]
    protected List<int> infoDataIDList;

    [SerializeField]
    protected List<NeedInfoData> needInfoList;

    protected List<ProfileInfoTextDataSO> infomaitionDataList;

    [SerializeField] protected int monoLogType;
    [SerializeField] protected int completeMonologType = 0;
    [SerializeField] protected float delay;
    [SerializeField] protected bool isFakeInfo;

    protected int playMonologType = 0;

    protected virtual void Start()
    {
        Bind();
    }

    protected virtual void Bind()
    {
        if (infoDataIDList.Count != 0 || infomaitionDataList == null)
        {
            infomaitionDataList = new List<ProfileInfoTextDataSO>();
            foreach (var id in infoDataIDList)
            {
                infomaitionDataList.Add(ResourceManager.Inst.GetProfileInfoData(id));
            }
        }
    }

    protected void FindInfo()
    {
        Bind();

        if (!CheckAllInfoFound())
        {
            foreach (var infoData in infomaitionDataList)
            {
                EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[2] { infoData.category, infoData.id });
            }
        }

        else
        {
            if (completeMonologType != 0)
            {
                playMonologType = completeMonologType;
            }
        }

        Sound.OnPlaySound?.Invoke(Sound.EAudioType.FindInfoTrigger);
        MonologSystem.OnStartMonolog?.Invoke(playMonologType, delay, true);


    }

    protected void GetInfo()
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall) return;

        if(!isFakeInfo)
        {
            if (infomaitionDataList == null || infomaitionDataList.Count == 0)
            {
                MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
                return;
            }
        }
        
        playMonologType = monoLogType;

        foreach (NeedInfoData needData in needInfoList)
        {
            if (!DataManager.Inst.IsProfileInfoData(needData.needInfoID))
            {
                // 이것이 -1이라면 즉시 정보 획득을 하라는 의미
                if (needData.getInfo)
                {
                    playMonologType = needData.monologID;
                    break;
                }

                int id = needData.monologID == 0 ? Constant.NEED_INFO_MONOLOG_ID : needData.monologID;
                MonologSystem.OnStartMonolog?.Invoke(id, delay, true);
                Sound.OnPlaySound?.Invoke(Sound.EAudioType.LockInfoTrigger);
                return;
            }
        }

        if(!isFakeInfo)
        {
            FindInfo();
        }
    }


    protected bool CheckAllInfoFound()
    {
        foreach (var infoID in infoDataIDList)
        {
            if (!DataManager.Inst.IsProfileInfoData(infoID))  // 찾은 정보인지 확인
            {
                return false;
            }
        }
        return true;
    }
}

