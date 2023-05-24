using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constant.ProfileInfoKey;

public class ProfileInfoSystem : MonoBehaviour
{
    [SerializeField]
    private Sprite profileSprite;

    private Dictionary<EProfileCategory, ProfileCategoryDataSO> infoList = new Dictionary<EProfileCategory, ProfileCategoryDataSO>();

    private void Start()
    {
        GameManager.Inst.OnStartCallback += StartCallback;
    }

    private void StartCallback()
    { 
        infoList = ResourceManager.Inst.GetProfileCategoryDataList();
        EventManager.StartListening(EProfileEvent.FindInfoText, ChangeValue);
    }

    private void ChangeValue(object[] ps) 
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        if (!(ps[0] is EProfileCategory) || !(ps[1] is int))
        {
            return;
        }

        EProfileCategory category = (EProfileCategory)ps[0];
        int id = (int)ps[1];

        if (!DataManager.Inst.IsProfileInfoData(id))
        {
            DataManager.Inst.AddProfileSaveData(category, id);

            if(!DataManager.Inst.IsCategoryShow(category))
            {
                DataManager.Inst.SetCategoryData(category, true);
                SendCategoryNotice(category);
            }
            EventManager.TriggerEvent(EProfileEvent.FindInfoInProfile, ps);
            SendAlarm(category, id);
        }
        else
        {
            return;
        }
    }

    public void SendAlarm(EProfileCategory category, int id)
    {
        string temp = "nullError";
        ProfileCategoryDataSO categoryData = infoList[category];
        foreach (var infoText in categoryData.infoTextList)
        {
            if (id == infoText.id)
            {
                temp = infoText.noticeText; 
            }
        }

        string text;
        if (temp == "nullError") return;

        if (category != EProfileCategory.InvisibleInformation)
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
    private void SendCategoryNotice(EProfileCategory category)
    {
        string head = "새로운 카테고리가 추가되었습니다";
        string body = "";
        if (category != EProfileCategory.InvisibleInformation)
        {
            body = $"새 카테고리 {infoList[category].categoryName}가 추가되었습니다.";
        }

        NoticeSystem.OnNotice?.Invoke(head, body, 0f, false, null, Color.white, ENoticeTag.Profiler);
    }
}
