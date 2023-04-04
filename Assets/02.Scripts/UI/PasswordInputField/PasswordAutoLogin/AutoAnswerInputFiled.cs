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

    public bool isLock
    {
        get
        {
            return DataManager.Inst.IsWindowLock(location);
        }
    }
    public string location;
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
