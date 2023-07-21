using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class ProfileOverlaySystem : MonoBehaviour
{
    public static Action<string, List<InformationTrigger>> OnOpen; //fileid, completeCnt, wholeCnt
    public static Action<string> OnAdd; //fileid , profileid
    public static Action OnClose;

    #region overlay
    public GameObject overlayPanel;
    public TMP_Text overlayText;
    #endregion

    [SerializeField]
    private List<string> triggerIDList = new List<string>();

    private string currentFileID;
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
        currentFileID = string.Empty;
        triggerIDList.Clear();
    }

    public void Open(string id, List<InformationTrigger> triggerList)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall || !DataManager.Inst.IsStartProfilerTutorial()) return;

        ResetCount();
        currentFileID = id;

        if(id == Constant.FileID.INCIDENT_REPORT)
        {
            EventManager.TriggerEvent(ETutorialEvent.IncidentReportOpen);
        }

        GetTriggerIDList(triggerList);

        wholeProfileCount = GetWholeCount();
        completeProfileCount = GetCompleteCount();

        Setting(id);
    }

    public void Add(string id)
    {
        if (completeProfileCount == wholeProfileCount)
        {
            return;
        }

        completeProfileCount = GetCompleteCount();

        if (completeProfileCount == wholeProfileCount)
        {
            //TODO 새로운 이펙트
            Debug.Log("asdf");
            if(DataManager.Inst.IsPlayingProfilerTutorial())
            {
                Debug.Log("asdf22222222222222222");
                EventManager.TriggerEvent(ETutorialEvent.GetAllInfo);
            }
        }

        Setting(id);
    }

    public void Setting(string id)
    {
        //if (currentFileID != id) //fileID 체크
        //{
        //    Debug.Log("현재 오버레이의 fileId와 다릅니다.");
        //    return;
        //}

        overlayText.text = GetCompleteCount() + " / " + GetWholeCount();

        if (GetWholeCount() == 0)
        {
            OnClose?.Invoke();
            return;
        }
            

        overlayPanel.SetActive(true);
    }

    private List<string> GetTriggerIDList(List<InformationTrigger> list)
    {
        list.ForEach((trigger) =>
        {
            triggerIDList.Add(trigger.TriggerData.id);
        });
        return triggerIDList;
    }

    private int GetWholeCount()
    {
        return triggerIDList.Count;
    }

    private int GetCompleteCount()
    {
        int count = 0;
        triggerIDList.ForEach((id) =>
        {
            TriggerDataSO data = ResourceManager.Inst.GetResource<TriggerDataSO>(id);
            bool flag = false;
            foreach(var infoID in data.infoDataIDList)
            {
                if(!DataManager.Inst.IsProfilerInfoData(infoID))
                {
                    flag = true;
                }
            }

            if(!flag)
            {
                count++;
            }
        });
        return count;
    }
}

