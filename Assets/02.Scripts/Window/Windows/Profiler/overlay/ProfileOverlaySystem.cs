using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ProfileOverlaySystem : MonoBehaviour
{
    public static Action<int, List<InformationTrigger>> OnOpen; //fileid, completeCnt, wholeCnt
    public static Action<int, List<int>> OnOpenInt; //fileid, completeCnt, wholeCnt
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
        OnOpenInt += OpenInt;
    }

    private void OpenInt(int id, List<int> list)
    {
        Debug.Log(list.Count);
        if (!DataManager.Inst.SaveData.isProfilerInstall) return;
        ResetCount();
        currentFileID = id;
        profileIDList = list;

        completeProfileCount = GetCompleteCount();
        wholeProfileCount = GetWholeCount();
        Setting(id);
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
        if (completeProfileCount == wholeProfileCount)
        {
            return;
        }

        completeProfileCount = GetCompleteCount();

        if (completeProfileCount == wholeProfileCount)
        {
            if (!MonologSystem.isEndMonolog)//독백중이면
            {
                Debug.Log(1);
                MonologSystem.OnEndMonologEvent = () => { MonologSystem.OnStartMonolog(Constant.MonologKey.COMPLETE_OVERLAY, 0.5f, false); };
            }
            else
            {
                Debug.Log(2);
                MonologSystem.OnStartMonolog(Constant.MonologKey.COMPLETE_OVERLAY, 1f, false);
            }
        }

        Setting(id);
    }

    public void Setting(int id)
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

    private List<int> GetProfileIDList(List<InformationTrigger> list)
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

