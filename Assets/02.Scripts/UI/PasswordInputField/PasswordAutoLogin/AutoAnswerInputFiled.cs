using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

[Serializable]
public class AutoAnswerData
{
    public string id;
    public string password;

    public bool isShow
    {
        get
        {
            return profileInfoData.GetSaveData(infoKey).isShow;
        }
    }

    public ProfileInfoDataSO profileInfoData;
    public string infoKey;
}

public class AutoAnswerInputFiled : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private List<TMP_InputField> inputFields;

    [SerializeField]
    private List<AutoAnswerData> autoAnswerDatas;

    [SerializeField]
    private AutoInputSystem inputSystem;

    public void OnPointerClick(PointerEventData eventData)
    {
        inputSystem.ShowPanel(inputFields, autoAnswerDatas);
    }
}
