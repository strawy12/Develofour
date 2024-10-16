﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Constant.ProfilerInfoKey;

public class ProfilerInfoSystem : MonoBehaviour
{
    [SerializeField]
    private Sprite profileSprite;

    private void Start()
    {
        GameManager.Inst.OnStartCallback += StartCallback;
    }

    private void StartCallback()
    { 
        EventManager.StartListening(EProfilerEvent.FindInfoText, ChangeValue);
    }

    private void ChangeValue(object[] ps) 
    {

        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        if (ps.Length == 2)
        {
            if (!(ps[0] is string) || !(ps[1] is string))
            {
                return;
            }
            string category = (string)ps[0];
            string id = (string)ps[1];
            ChangeValue(category, id);
        }
        else if (ps.Length == 1)
        {
            if (!(ps[0] is string))
            {
                return;
            }
            string id = (string)ps[0];
            Debug.Log(id);
            ProfilerInfoDataSO infoDataSO = ResourceManager.Inst.GetResource<ProfilerInfoDataSO>(id);

            if(DataManager.Inst.IsPlayingProfilerTutorial())
            {
                string[] strArr = infoDataSO.categoryID.Split('_');
                List<TriggerDataSO> triggerList = ResourceManager.Inst.GetResourceList<TriggerDataSO>();
                var list = triggerList.Where(trigger => trigger.infoDataIDList.Contains(id)).ToList();

                if (strArr[1] == "I")
                {
                    EventManager.TriggerEvent(ETutorialEvent.GetIncidentInfo, new object[] { list[0].monoLogType });
                }

                if (strArr[1] == "C")
                {
                    EventManager.TriggerEvent(ETutorialEvent.GetCharacterInfo, new object[] { list[0].monoLogType });
                }

            }

            ChangeValue(infoDataSO.categoryID, id);
            ps = new object[2] { infoDataSO.categoryID, id };

        }
        EventManager.TriggerEvent(EProfilerEvent.RegisterInfo, ps);

    }
    private void ChangeValue(string categoryID, string id)
    {  
        if (!DataManager.Inst.IsProfilerInfoData(id))
        {
            DataManager.Inst.SaveProfilerInfoData(categoryID, id);

            if (!DataManager.Inst.IsCategoryShow(categoryID))
            {
                DataManager.Inst.SetCategoryShow(categoryID, true);
                SendCategoryNotice(categoryID);
            }
            SendAlarm(categoryID, id);
        }
        else
        {
            return;
        }
    }


    public void SendAlarm(string categoryID, string id)
    {
        string temp = "nullError";
        ProfilerCategoryDataSO categoryData = ResourceManager.Inst.GetResource<ProfilerCategoryDataSO>(categoryID);
        ProfilerInfoDataSO infoData = ResourceManager.Inst.GetResource<ProfilerInfoDataSO>(id);
        temp = infoData != null ? infoData.noticeText : temp;

        string text;
        if (temp == "nullError") return;

        if (categoryData != null && categoryData.categoryType != EProfilerCategoryType.Visiable)
        {
            text =  temp + "정보가 업데이트 되었습니다.";
            NoticeSystem.OnNotice.Invoke("Profiler 정보가 업데이트가 되었습니다!", text, 0, true, profileSprite, Color.white, ENoticeTag.Profiler, Constant.FileID.PROFILER);
        }
        else
        {
            text = temp + " 정보가 확인되었습니다.";
            NoticeSystem.OnNotice.Invoke("Profiler 정보가 확인되었습니다!", text, 0, true, profileSprite, Color.white, ENoticeTag.Profiler, Constant.FileID.PROFILER);
        }
        
    }
    private void SendCategoryNotice(string categoryID)
    {
        string head = "새로운 카테고리가 추가되었습니다";
        string body = "";

        ProfilerCategoryDataSO data = ResourceManager.Inst.GetResource<ProfilerCategoryDataSO>(categoryID);

        if (data != null && data.categoryType != EProfilerCategoryType.Visiable)
        {
            body = $"새 카테고리 {data.categoryName}가 추가되었습니다.";
        }

        NoticeSystem.OnNotice?.Invoke(head, body, 0f, false, null, Color.white, ENoticeTag.Profiler, Constant.FileID.PROFILER);
    }
}
