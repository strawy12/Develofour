using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

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

    private ContentSizeFitter fitter;

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
        fitter ??= GetComponent<ContentSizeFitter>();
    }

    public void Setting(ProfileInfoTextDataSO infoData)
    {
        currentInfoData = infoData;
    }

    public void Show()
    {
        infoText.text = currentInfoData.infomationText;
        isFind = true;
        Debug.Log("show");
        EventManager.StartListening(EProfileEvent.Maximum, RefreshSize);
        EventManager.StartListening(EProfileEvent.Minimum, RefreshSize);
        this.gameObject.SetActive(true);
        OnFindText?.Invoke();
    }

    private void RefreshSize(object[] ps)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)fitter.transform);
    }

    public void Hide()
    {
        EventManager.StopListening(EProfileEvent.Maximum, RefreshSize);
        EventManager.StopListening(EProfileEvent.Minimum, RefreshSize);
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.Maximum, RefreshSize);
        EventManager.StopListening(EProfileEvent.Minimum, RefreshSize);
    }
}
