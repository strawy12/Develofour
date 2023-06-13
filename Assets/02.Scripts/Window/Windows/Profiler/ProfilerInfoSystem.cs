using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constant.ProfilerInfoKey;

public class ProfilerInfoSystem : MonoBehaviour
{
    [SerializeField]
    private Sprite profileSprite;

    private Dictionary<EProfilerCategory, ProfilerCategoryDataSO> infoList = new Dictionary<EProfilerCategory, ProfilerCategoryDataSO>();

    private void Start()
    {
        GameManager.Inst.OnStartCallback += StartCallback;
    }

    private void StartCallback()
    { 
        infoList = ResourceManager.Inst.GetProfilerCategoryList();
        EventManager.StartListening(EProfilerEvent.FindInfoText, ChangeValue);
    }

    private void ChangeValue(object[] ps) 
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        EProfilerCategory category = EProfilerCategory.None;
        int id = 0;

        if (ps[0] is int)
        {
            id = (int)ps[0];
            category = ResourceManager.Inst.GetProfilerInfoData(id).category;
            Debug.Log(category);
        }
        else if (ps[0] is EProfilerCategory && ps[1] is int)
        {
            category = (EProfilerCategory)ps[0];
            id = (int)ps[1];
        }
        else
        {
            Debug.Log("Return");
            return;
        }

        if (!DataManager.Inst.IsProfilerInfoData(id))
        {
            DataManager.Inst.AddProfilerSaveData(category, id);

            if(!DataManager.Inst.IsCategoryShow(category))
            {
                DataManager.Inst.SetCategoryData(category, true);
                EventManager.TriggerEvent(ECallEvent.GetMonologSettingIncomingData, new object[] { id } );
                SendCategoryNotice(category);
            }
            EventManager.TriggerEvent(EProfilerEvent.FindInfoInProfiler, ps);
            SendAlarm(category, id);
        }
        else
        {
            return;
        }
    }

    public void SendAlarm(EProfilerCategory category, int id)
    {
        string temp = "nullError";
        ProfilerCategoryDataSO categoryData = infoList[category];
        foreach (var infoText in categoryData.infoTextList)
        {
            if (id == infoText.id)
            {
                temp = infoText.noticeText; 
            }
        }

        string text;
        if (temp == "nullError") return;

        if (category != EProfilerCategory.InvisibleInformation)
        {
            text = categoryData.categoryName + " 카테고리의 " + temp + "정보가 업데이트 되었습니다.";
            NoticeSystem.OnNotice.Invoke("Profiler 정보가 업데이트가 되었습니다!", text, 0, true, profileSprite, Color.white, ENoticeTag.Profiler);
        }
        else
        {
            text = temp + " 정보가 확인되었습니다.";
            NoticeSystem.OnNotice.Invoke("Profiler 정보가 확인되었습니다!", text, 0, true, profileSprite, Color.white, ENoticeTag.Profiler);
        }
        
    }
    private void SendCategoryNotice(EProfilerCategory category)
    {
        string head = "새로운 카테고리가 추가되었습니다";
        string body = "";
        if (category != EProfilerCategory.InvisibleInformation)
        {
            body = $"새 카테고리 {infoList[category].categoryName}가 추가되었습니다.";
        }

        NoticeSystem.OnNotice?.Invoke(head, body, 0f, false, null, Color.white, ENoticeTag.Profiler);
    }
}
