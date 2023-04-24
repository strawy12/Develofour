using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProfileInfoText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public ProfileInfoTextDataSO textDataSO;

    public bool isTitleShow = false;
    public TMP_Text infoText;

    public ProfileShowInfoTextPanel showPanel;

    public TMP_Text infoTitleText;

    private string infoTitle;

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

    //이전 텍스트로 변경, 이후 텍스트로 변경해주는 함수

    public void Init()
    {
        rectTransform ??= GetComponent<RectTransform>();
    }

    public void ShowTitle()
    {
        infoTitleText.text = infoTitle;
    }


    public void ChangeText()
    {
        infoText.text = textDataSO.infomationText;
        isFind = true;
        
        OnFindText?.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (infoText.text != textDataSO.infomationText)
        {
            return;
        }

        showPanel.text.text = infoText.text;
        showPanel.transform.SetParent(gameObject.transform.parent);
        showPanel.RectTrm.position = rectTransform.position;
        showPanel.transform.SetParent(showPanel.showPanelParent.transform);
        showPanel.RectTrm.anchoredPosition += new Vector2(20, 35);
        showPanel.SetDownText();
        showPanel.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showPanel == null) return;
        showPanel.gameObject.SetActive(false);
    }
}
