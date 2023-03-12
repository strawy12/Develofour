using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;
using UnityEngine.UI;

[Serializable]
public class ProfileInfoPart
{
    public bool isShow;
    public string partNameKey;
    public GameObject part;
    public TMP_Text titleText;
}

public class ProfileInfoPanel : MonoBehaviour
{
    public EProfileCategory category;

    [SerializeField]
    private TMP_Text categoryNameText;
    //동적 저장을 위해서는 활성화 비활성화 여부를 들고있는 SO 혹은 Json이 저장 정보를 불러오고 저장
    [SerializeField]
    public List<ProfileInfoText> infoTextList;

    
    private ProfileInfoDataSO saveData;


    public void Init(ProfileInfoDataSO profileInfoDataSO)
    {
        saveData = profileInfoDataSO;

        Setting();
    }

    public void Setting()//켰을때 기초 세팅
    {

        //string str ="";
        //for(int i = 0; i < part.partNameKey.Length;i++)
        //{
        //    str += "?";
        //}
        //part.titleText.text = str;

        foreach (var infoText in infoTextList)
        {
            infoText.Init();
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

    public void ChangeValue(string key)//특정어느것을 눌렀을때 세팅
    {
        //foreach (var part in infoPartList)
        //{
        //    Debug.Log(part.partNameKey + " " + key);
        //    if (key.Contains(part.partNameKey))
        //    {

        //        //저장 방식이 어케되는거?
        //        part.isShow = true;
        //        part.part.SetActive(true);
        //        part.titleText.text = part.partNameKey;
        //    }
        //} 
        //카테고리로 번경될 예정

        foreach (var infoText in infoTextList)
        {
            if (infoText.infoNameKey == key)
            {
                infoText.ChangeText();
                Debug.Log(saveData);
                Debug.Log(saveData.GetSaveData(key));
                Debug.Log(key);
                saveData.GetSaveData(key).isShow = true;
                if (key == "OwnerName" && GameManager.Inst.GameState == EGameState.Tutorial)
                {
                    EventManager.TriggerEvent(ETutorialEvent.EndClickInfoTutorial);
                }
            }
        }
    }
}