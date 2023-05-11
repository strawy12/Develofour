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

    public ProfileInfoTextDataSO infoData;
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
        Debug.Log("ClickInputField");
        inputSystem.ShowPanel(inputField, autoAnswerDatas);
    }
}
