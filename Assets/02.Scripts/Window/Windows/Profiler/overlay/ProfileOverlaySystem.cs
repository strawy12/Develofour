using System;
using System.Collections;
using System.Collections.Generic;
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
    private List<string> profileIDList = new List<string>();

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
        profileIDList.Clear();
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

        Debug.Log(triggerList.Count);
        GetProfileIDList(triggerList);

        completeProfileCount = GetCompleteCount();
        wholeProfileCount = GetWholeCount();
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
            if (!MonologSystem.IsPlayMonolog)//독백중이면
            {
                Debug.Log(1);
                MonologSystem.AddOnEndMonologEvent(id, () => { MonologSystem.OnStartMonolog(Constant.MonologKey.COMPLETE_OVERLAY, false); });
            }
            else
            {
                Debug.Log(2);
                MonologSystem.OnStartMonolog(Constant.MonologKey.COMPLETE_OVERLAY, false);
            }
        }

        Setting(id);
    }

    public void Setting(string id)
    {
        if (currentFileID != id) //fileID 체크
        {
            Debug.Log("현재 오버레이의 fileId와 다릅니다.");
            return;
        }

        overlayText.text = GetCompleteCount() + " / " + GetWholeCount();

        if (GetWholeCount() == 0)
        {
            OnClose?.Invoke();
            return;
        }
            

        overlayPanel.SetActive(true);
    }

    private List<string> GetProfileIDList(List<InformationTrigger> list)
    {
        list.ForEach((trigger) =>
        {
            Debug.Log(trigger.TriggerData);
            for (int i = 0; i < trigger.TriggerData.infoDataIDList.Count; i++)
            {
                if (!profileIDList.Contains(trigger.TriggerData.infoDataIDList[i]))
                {
                    profileIDList.Add(trigger.TriggerData.infoDataIDList[i]);
                }
            }
        });
        return profileIDList;
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

