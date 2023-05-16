using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProfileInfoText : MonoBehaviour
{

    private ProfileInfoTextDataSO currentInfoData;

    public ProfileInfoTextDataSO InfoData
    {
        get
        {
            return currentInfoData;
        }
    }

    [SerializeField]
    private TMP_Text infoText;

    [SerializeField]
    private ProfileShowInfoTextPanel showPanel;
    private bool isFind;

    public bool IsFind
    {
        get => isFind;
        set => isFind = value;
    }

    public Action OnFindText;

    private RectTransform rectTransform;

    public RectTransform RectTrm
    {
        get
        {
            rectTransform ??= GetComponent<RectTransform>();
            return rectTransform;
        }
    }

    public void Init()
    {
        rectTransform ??= GetComponent<RectTransform>();
    }

    public void Setting(ProfileInfoTextDataSO infoData)
    {
        currentInfoData = infoData;
    }

    public void Show()
    {
        infoText.text = currentInfoData.infomationText;
        isFind = true;
        
        OnFindText?.Invoke();
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
