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
    private TMP_InputField inputField; 

    [SerializeField]
    private List<AutoAnswerData> autoAnswerDatas;

    [SerializeField]
    private AutoInput inputSystem;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("ClickInputField");
        inputSystem.ShowPanel(inputField, autoAnswerDatas);
    }
}
