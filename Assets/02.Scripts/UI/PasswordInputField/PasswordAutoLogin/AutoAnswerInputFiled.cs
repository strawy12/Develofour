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
}

public class AutoAnswerInputFiled : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private List<TMP_InputField> inputFields;

    [SerializeField]
    private List<AutoAnswerData> autoAnswerDatas;

    [SerializeField]
    private AutoInputSystem inputSystem;

    private bool isComplete;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(!isComplete)
        {
            isComplete = true;
            inputSystem.ShowPanel(inputFields, autoAnswerDatas);
        }
    }
}
