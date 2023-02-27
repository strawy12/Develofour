using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using System;

[Serializable]
public class ProfileInfoPart
{
    public bool isShow;
    public string partNameKey;
    public GameObject part;
}

public class ProfileInfoPanel : MonoBehaviour
{ 
    public EProfileCategory category;

    //동적 저장을 위해서는 활성화 비활성화 여부를 들고있는 SO 혹은 Json이 저장 정보를 불러오고 저장

    [SerializeField]
    public List<ProfileInfoText> infoTextList;
        
    [SerializeField]
    public List<ProfileInfoPart> infoPartList; //profile, branch, starbook

    private ProfileInfoDataSO saveData;

    public void Init(ProfileInfoDataSO profileInfoDataSO)
    {
        saveData = profileInfoDataSO;

        Setting();
    }

    public void Setting()//켰을때 기초 세팅
    {
        foreach(var part in infoPartList)
        {
            if(!part.isShow)
            {
                part.part.SetActive(false);
            }
        }

        foreach(var save in saveData.saveList)
        {
            if(save.isShow == false)
            {
                continue;
            }

            foreach(var infoText in infoTextList)
            {
                if(infoText.infoNameKey == save.key)
                {
                    infoText.ChangeText();
                }
            }
        }
    }

    public void Setting(string key)//특정어느것을 눌렀을때 세팅
    {
        foreach (var part in infoPartList)
        {
            Debug.Log(part.partNameKey + " " + key);
            if (part.partNameKey == key)
            {
                //저장 방식이 어케되는거?
                part.isShow = true;
                part.part.SetActive(true);
            }
        }

        foreach (var infoText in infoTextList)
        {
            if(infoText.infoNameKey == key)
            {
                infoText.ChangeText();
                saveData.GetSaveData(key).isShow = true;
            }
        }
    }
}