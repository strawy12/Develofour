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
    public List<MonologLockDecision> infoData;
}

public class AutoAnswerInputFiled : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    protected TMP_InputField inputField; 

    [SerializeField]
    protected List<AutoAnswerData> autoAnswerDatas;

    [SerializeField]
    protected AutoInput inputSystem;

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        inputSystem.ShowPanel(inputField, autoAnswerDatas);
    }
}
