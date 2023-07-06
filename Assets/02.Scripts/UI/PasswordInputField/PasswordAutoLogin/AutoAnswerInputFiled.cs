using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using System;

[Serializable]
public class AutoAnswerData
{
    public string answer;
    
    //public ProfilerInfoTextDataSO infoData;
    public List<string> needInfoData;
}

public class AutoAnswerInputFiled : MonoBehaviour, IPointerClickHandler
{
    public TMP_InputField inputField; 

    public List<AutoAnswerData> autoAnswerDatas;

    public AutoInput inputSystem;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        inputSystem.ShowPanel(inputField, autoAnswerDatas);
    }
}
