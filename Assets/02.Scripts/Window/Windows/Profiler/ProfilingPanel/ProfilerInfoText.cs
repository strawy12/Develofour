using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ProfilerInfoText : MonoBehaviour
{

    private ProfilerInfoDataSO currentInfoData;

    public ProfilerInfoDataSO InfoData
    {
        get
        {
            return currentInfoData;
        }
    }

    [SerializeField]
    private TMP_Text infoText;

    [SerializeField]
    private RectTransform lineRect;
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

    public void Setting(ProfilerInfoDataSO infoData)
    {
        currentInfoData = infoData;
    }

    public void Show()
    {
        infoText.text = currentInfoData.infomationText;
        isFind = true;
        this.gameObject.SetActive(true);
        OnFindText?.Invoke();
    }

    public void RefreshSize()
    {
        if (gameObject.activeSelf == false) return;
        LayoutRebuilder.ForceRebuildLayoutImmediate(infoText.rectTransform);
        StartCoroutine(RefreshSizeCoroutine());
    }

    private IEnumerator RefreshSizeCoroutine()
    {
        yield return new WaitForSeconds(0.01f);
        float sizeY = lineRect.sizeDelta.y + infoText.rectTransform.sizeDelta.y;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, sizeY + 7f);
        lineRect.localPosition = new Vector2(infoText.rectTransform.localPosition.x, infoText.rectTransform.localPosition.y - 7f);
    }

    public void Hide()
    {
        StopCoroutine(RefreshSizeCoroutine());
        gameObject.SetActive(false);
    }

}
