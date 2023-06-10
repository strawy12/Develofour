﻿using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileOverlaySystem : MonoBehaviour
{
    public static Action<int, List<InformationTrigger>> OnOpen; //fileid, completeCnt, wholeCnt
    public static Action<int> OnAdd; //fileid , profileid
    public static Action OnClose;

    #region overlay
    public GameObject overlayPanel;
    public TMP_Text overlayText;
    #endregion

    [SerializeField]
    private List<int> profileIDList = new List<int>();

    private int currentFileID;
    private int completeProfileCount;
    private int wholeProfileCount;

    void Start()
    {
        OnOpen += Open;
        OnAdd += Add;
        OnClose += Close;
    }

    private void Close()
    {
        ResetCount();
        overlayPanel.SetActive(false);
    }

    private void ResetCount()
    {
        completeProfileCount = 0;
        wholeProfileCount = 0;
        currentFileID = 0;
        profileIDList.Clear();
    }

    public void Open(int id, List<InformationTrigger> triggerList)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall) return;
        ResetCount();
        currentFileID = id;
        GetProfileIDList(triggerList);

        completeProfileCount = GetCompleteCount();
        wholeProfileCount = GetWholeCount();
        Setting(id);
    }

    public void Add(int id)
    {
        if(completeProfileCount == wholeProfileCount)
        {
            Debug.Log(completeProfileCount + " " + wholeProfileCount);
            return;
        }

        completeProfileCount = GetCompleteCount();

        if(completeProfileCount == wholeProfileCount)
        {
            Debug.Log(completeProfileCount + " " + wholeProfileCount);
            MonologSystem.OnStartMonolog(Constant.MonologKey.COMPLETE_OVERLAY, 1f, false);
        }

        Setting(id);
    }

    public void Setting(int id)
    {
        if(currentFileID != id) //fileID 체크
        {
            Debug.Log("현재 오버레이의 fileId와 다릅니다.");
            return;
        }

        overlayText.text = GetCompleteCount() + " / " + GetWholeCount();
        overlayPanel.SetActive(true);
    }

    private void GetProfileIDList(List<InformationTrigger> list)
    {
        list.ForEach((trigger) =>
        {
            for (int i = 0; i < trigger.infoDataIDList.Count; i++)
            {
                if (!profileIDList.Contains(trigger.infoDataIDList[i]))
                {
                    profileIDList.Add(trigger.infoDataIDList[i]);
                }
            }
        });
    }

    private int GetWholeCount()
    {
        return profileIDList.Count;
    }

    private int GetCompleteCount()
    {
        int count = 0;
        profileIDList.ForEach((id) =>
        {
            if (DataManager.Inst.IsProfilerInfoData(id))
            {
                count++;
            }
        });
        return count;
    }
}
