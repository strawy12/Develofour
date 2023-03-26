using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;


public class ProfileInfoPanel : MonoBehaviour
{
    public EProfileCategory category;
    [SerializeField]
    private TMP_Text categoryNameText;
    //동적 저장을 위해서는 활성화 비활성화 여부를 들고있는 SO 혹은 Json이 저장 정보를 불러오고 저장
    [SerializeField]
    private List<ProfileInfoText> infoTextList; 

    private ProfileCategoryDataSO saveData;
    //이 패널이 정보를 모두 찾았다면 연결된 패널들이 보임
    [SerializeField]
    private List<ProfileInfoPanel> linkInfoPenelList;
    public void Init(ProfileCategoryDataSO profileInfoDataSO)
    {
        saveData = profileInfoDataSO;

        Setting();
    }

    public void Setting()//켰을때 기초 세팅
    {

        foreach (var infoText in infoTextList)
        {
            infoText.Init();
            infoText.OnFindText += ShowLinkedPost;
        }

        if (DataManager.Inst.GetProfileSaveData(saveData.category).isShowCategory)
        {
            ShowPost();
        }
        else
        {
            HidePost();
        }

        foreach (var save in saveData.infoTextList)
        {
            if (DataManager.Inst.IsProfileInfoData(saveData.category, save.key) == false)
            {
                continue;
            }
            foreach (var infoText in infoTextList)
            {
                if (infoText.textDataSO.key == save.key)
                {
                    infoText.ChangeText(); 
                }
            }
        }
    }

    public void ChangeValue(string key)
    {
        foreach (var infoText in infoTextList)
        {
            if (infoText.textDataSO.key == key)
            {
                if (gameObject.activeSelf == false)
                {
                    ShowPost();
                }
                infoText.ChangeText();

                DataManager.Inst.AddProfileinfoData(saveData.category, key);
                EventManager.TriggerEvent(EProfileEvent.AddGuideButton, new object[2] { category, key });
                EventManager.TriggerEvent(EGuideEventType.ClearGuideType, new object[1] { saveData.infoTextList.Find(x => x.key == key).guideTopicName });
                if (key == "SuspectName" && DataManager.Inst.SaveData.isTutorialStart)
                {
                    EventManager.TriggerEvent(ETutorialEvent.EndClickInfoTutorial);
                }
            }
        }
    }

    private void SendNotice()
    {
        //string head, string body, float delay, bool canDelete, Sprite icon, Color color, ENoticeTag noticeTag

        string head = "새로운 카테고리가 추가되었습니다";
        string body = $"새 카테고리 {saveData.categoryTitle}가 추가되었습니다.";

        NoticeSystem.OnNotice?.Invoke(head, body, 0f, false, null, Color.white, ENoticeTag.Profiler);

    }

    public void ShowPost()
    {
        gameObject.SetActive(true);

        SendNotice();
        DataManager.Inst.SetCategoryData(saveData.category, true);
    }

    private void HidePost()
    {
        gameObject.SetActive(false);
    }


    private void ShowLinkedPost()
    {
        if (linkInfoPenelList.Count == 0)
        {
            return;
        }

        if (GetIsFindAll())
        {
            foreach (var infoPost in linkInfoPenelList)
            {
                infoPost.ShowPost();
            }
        }
    }

    public bool GetIsFindAll()
    {
        foreach (var info in infoTextList)
        {
            if (info.IsFind == false)
            {
                return false;
            }
        }
        return true;
    }

    public string SetInfoText(string key)
    {
        string answer = "";
        foreach (var infoText in infoTextList)
        {
            if (key == infoText.textDataSO.key)
            {
                answer = infoText.infoTitleText.text;
            }
        }

        return answer;
    }
}