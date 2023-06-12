﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constant.ProfilerInfoKey;

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
    
    public List<int> infoDataIDList;

    public List<NeedInfoData> needInfoList;

    [SerializeField] protected int monoLogType;
    public int completeMonologType = 0;
    public float delay;
    public bool isFakeInfo;

    protected int playMonologType = 0;

    public int MonologID {
        get
        {
            return monoLogType;
        }
        set
        {
            monoLogType = value;
        }
    }

    public int fildID;

    protected virtual void Start()
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

    }

    public void FindInfo()
    {
        Bind();
        if (!CheckAllInfoFound())
        {
            bool playMonolog = false;

            foreach (var infoID in infoDataIDList)
            {
                if (!DataManager.Inst.GetIsClearTutorial())
                {
                    int idx = DataManager.Inst.GetProfilerTutorialIdx();
                    if (
                        (idx == 0 &&
                        infoID == INCIDENTREPORT_TITLE)
                        ||
                        (idx == 2 &&
                        (infoID == KIMYUJIN_NAME || infoID  == PARKJUYOUNG_NAME))
                    )
                    {
                        playMonolog = true;
                    }
                    else
                    continue;
                }
                playMonolog = true;
                if(!isFakeInfo)
                {
                    EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[1] { infoID });
                }
            }

            if (!playMonolog)
            {
                MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.TUTORIAL_NOT_FIND_INFO, 0.1f, false);
                return;
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
        MonologSystem.OnStartMonolog?.Invoke(playMonologType, delay, false);


    }

    public void GetInfo()
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall) return;


        if (infoDataIDList.Count == 0 || infoDataIDList == null)
        {
            MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, false);
            return;
        }
        playMonologType = monoLogType;

        foreach (NeedInfoData needData in needInfoList)
        {
            // 이것이 -1이라면 즉시 정보 획득을 하라는 의미
            if (needData.getInfo)
            {
                playMonologType = needData.monologID;
                break;
            }

            if (!DataManager.Inst.IsProfilerInfoData(needData.needInfoID))
            {
                int id = needData.monologID == 0 ? Constant.MonologKey.NEEDINFO : needData.monologID;
                Debug.Log(id);
                MonologSystem.OnStartMonolog?.Invoke(id, delay, false);
                Sound.OnPlaySound?.Invoke(Sound.EAudioType.LockInfoTrigger);
                return;
            }

        }

  
            FindInfo();
     
    }


    protected bool CheckAllInfoFound()
    {
        foreach (var infoID in infoDataIDList)
        {
            if (!DataManager.Inst.IsProfilerInfoData(infoID))  // 찾은 정보인지 확인
            {
                return false;
            }
        }
        return true;
    }
}

