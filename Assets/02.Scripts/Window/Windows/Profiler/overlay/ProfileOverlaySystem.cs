using System;
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
        Debug.Log("Fdsa");
        profileIDList.Clear();
    }

    public void Open(int id, List<InformationTrigger> triggerList)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall) return;
        ResetCount();
        currentFileID = id;
        Debug.Log("ㅁㄴㅇㄹ");
        GetProfileIDList(triggerList);
        Debug.Log(profileIDList.Count);
        completeProfileCount = GetCompleteCount();
        wholeProfileCount = GetWholeCount();
        Setting(id);
    }

    public void Add(int id)
    {
        if(completeProfileCount == wholeProfileCount)
        {
            return;
        }

        completeProfileCount = GetCompleteCount();

        if(completeProfileCount == wholeProfileCount)
        {
            if(!MonologSystem.isEndMonolog)//독백중이면
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
        Debug.Log("id는 " + id);
        Setting(id);    
    }

    public void Setting(int id)
    {
        if(currentFileID != id) //fileID 체크
        {
            Debug.Log("현재 오버레이의 fileId와 다릅니다.");
            return;
        }
        Debug.Log("Asdfasfd");
        overlayText.text = GetCompleteCount() + " / " + GetWholeCount();
        overlayPanel.SetActive(true);
    }

    private void GetProfileIDList(List<InformationTrigger> list)
    {
        Debug.Log(list.Count);
        list.ForEach((trigger) =>
        {
            Debug.Log(trigger.MonologID);
            Debug.Log(trigger.infoDataIDList.Count);
            for (int i = 0; i < trigger.infoDataIDList.Count; i++)
            {
                Debug.Log(trigger.infoDataIDList[i]);
                if (!profileIDList.Contains(trigger.infoDataIDList[i]))
                {
                    Debug.Log("add");
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
