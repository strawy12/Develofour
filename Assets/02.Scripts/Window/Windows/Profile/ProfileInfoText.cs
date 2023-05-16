using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ProfileInfoText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        //if (infoText.text != currentInfoData.infomationText)
        //{
        //    return;
        //}

        showPanel.text.text = infoText.text;
        showPanel.transform.SetParent(gameObject.transform.parent);
        showPanel.RectTrm.position = rectTransform.position;
        showPanel.transform.SetParent(showPanel.showPanelParent.transform);
        showPanel.RectTrm.anchoredPosition += new Vector2(20, 35);
        showPanel.SetDownText();
        showPanel.gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (showPanel == null) return;
        showPanel.gameObject.SetActive(false);
    }
}
