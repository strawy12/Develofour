using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        Debug.Log(11);
        infoList = ResourceManager.Inst.GetProfileCategoryDataList();
        EventManager.StartListening(EProfileEvent.FindInfoText, ChangeValue);
    }

    private void ChangeValue(object[] ps) // string 값으로 들고옴
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        if (!(ps[0] is EProfileCategory) || !(ps[1] is string))
        {
            return;
        }

        EProfileCategory category = (EProfileCategory)ps[0];
        string key = ps[1] as string;

        //if(DataManager.Inst.GetIsStartTutorial(ETutorialType.Profiler) && !DataManager.Inst.GetIsClearTutorial(ETutorialType.Profiler))
        //{
        //    if(key != Constant.ProfileInfoKey.SUSPECTNAME)
        //    {
        //        //MonologSystem.OnStartMonolog?.Invoke(Constant.MonologKey.TUTORIALNOTFINDNAME, 0.1f, false);
        //        return;
        //    }
        //}

        if (!DataManager.Inst.IsProfileInfoData(category, key))
        {
            DataManager.Inst.AddProfileSaveData(category, key);

            if(!DataManager.Inst.IsCategoryShow(category))
            {
                DataManager.Inst.SetCategoryData(category, true);
                SendCategoryNotice(category);
            }
            EventManager.TriggerEvent(EProfileEvent.FindInfoInProfile, ps);
            SendAlarm(category, key);
        }
        else
        {
            return;
        }

        if (key == "SuspectName" && DataManager.Inst.GetIsStartTutorial(ETutorialType.Profiler))
        {
            EventManager.TriggerEvent(ETutorialEvent.EndClickInfoTutorial);
        }


    }

    public void SendAlarm(EProfileCategory category, string key)
    {
        string temp = "nullError";
        ProfileCategoryDataSO categoryData = infoList[category];
        foreach (var infoText in categoryData.infoTextList)
        {
            if (key == infoText.key)
            {
                temp = infoText.noticeText; 
            }
        }

        string text;
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
