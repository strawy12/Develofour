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

    private ProfileCategoryDataSO saveData;
    //이 패널이 정보를 모두 찾았다면 연결된 패널들이 보임
    [SerializeField]
    private List<ProfileInfoPanel> LinkInfoPenelList;
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


                Debug.Log("1");

                DataManager.Inst.AddProfileinfoData(saveData.category, key);
                EventManager.TriggerEvent(EProfileEvent.AddGuideButton, new object[2] { category, key });

                if (key == "SuspectName" && DataManager.Inst.SaveData.isTutorialStart)
                {
                    EventManager.TriggerEvent(ETutorialEvent.EndClickInfoTutorial);
                }
            }
        }
    }

    private void ShowPost()
    {
        gameObject.SetActive(true);
        DataManager.Inst.SetCategoryData(saveData.category, true);
    }

    private void HidePost()
    {
        gameObject.SetActive(false);
    }


    private void ShowLinkedPost()
    {
        if (LinkInfoPenelList.Count == 0)
        {
            return;
        }

        if (GetIsFindAll())
        {
            foreach (var infoPost in LinkInfoPenelList)
            {
                infoPost.ShowPost();
            }
        }
    }

    public bool GetIsFindAll()
    {
        foreach (var info in infoTextList)
        {
            if (info.isFind == false)
            {
                return false;
            }
        }
        return true;
    }
}