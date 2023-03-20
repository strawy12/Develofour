using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEngine.UI;


public class ProfileInfoPanel : MonoBehaviour
{
    public EProfileCategory category;
    [SerializeField]
    private TMP_Text categoryNameText;
    //동적 저장을 위해서는 활성화 비활성화 여부를 들고있는 SO 혹은 Json이 저장 정보를 불러오고 저장
    [SerializeField]
    public List<ProfileInfoText> infoTextList;

    private ProfileInfoDataSO saveData;
    //이 패널이 정보를 모두 찾았다면 연결된 패널들이 보임
    [SerializeField]
    private List<ProfileInfoPanel> LinkInfoPenelList;
    public void Init(ProfileInfoDataSO profileInfoDataSO)
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

        if(saveData.isShowCategory)
        {
            ShowPost();
        }
        else
        {
            HidePost();
        }

        foreach (var save in saveData.saveList)
        {
            if (save.isShow == false)
            {
                continue;
            }
            foreach (var infoText in infoTextList)
            {
                if (infoText.infoNameKey == save.key)
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

            if (infoText.infoNameKey == key)
            {
                if(gameObject.activeSelf == false)
                {
                    ShowPost();
                }
                infoText.ChangeText();

                if(saveData.GetSaveData(key).isShow == false)
                {
                    saveData.GetSaveData(key).isShow = true;
                    FindAlarm(categoryNameText.text, key);
                }

                Debug.Log(key);
                Debug.Log(GameManager.Inst.GameState);
                if (key == "SuspectName" && DataManager.Inst.SaveData.isTutorialStart)
                {
                    EventManager.TriggerEvent(ETutorialEvent.EndClickInfoTutorial);
                }
            }
        }
    }

    public bool CheckIsTrue(string str)
    {
        return saveData.GetSaveData(str).isShow;
    }

    public void FindAlarm(string category, string key)
    {
        EventManager.TriggerEvent(ENoticeEvent.GeneratedProfileFindNotice, new object[2] { category, key });
    }
    private void ShowPost()
    {
        gameObject.SetActive(true);
        saveData.isShowCategory = true;
    }

    private void HidePost()
    {
        gameObject.SetActive(false);
    }


    private void ShowLinkedPost()
    {
        if(LinkInfoPenelList.Count == 0)
        {
            return;
        }

        if(GetIsFindAll())
        {
            foreach(var infoPost in LinkInfoPenelList)
            {
                infoPost.ShowPost();
            }
        }
    }

    public bool GetIsFindAll()
    {
        foreach(var info in infoTextList)
        {
            if(info.isFind == false)
            {
                return false;
            }
        }
        return true;
    }
}