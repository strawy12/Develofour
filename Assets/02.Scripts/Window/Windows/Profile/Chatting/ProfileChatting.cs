using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

public enum EProfileChatting
{
    Email,
    Password,
}

[Serializable]
public struct ChatData
{
    public EAiChatData eChat;
    [TextArea]
    public string script;
};

public class ProfileChatting : MonoBehaviour
{
    [Header("움직임 관련")]
    [SerializeField]
    protected Button OpenCloseButton;
    [SerializeField]
    protected GameObject showImage;
    [SerializeField]
    protected GameObject hideImage;
    [SerializeField]
    protected float showValue;
    [SerializeField]
    protected float hideValue;

    [SerializeField]
    protected GameObject loadingPanel;

    protected float currentValue;

    [SerializeField]
    protected float moveDuration;
    protected bool isMoving;
    protected RectTransform movePanelRect;


    [SerializeField]
    protected TMP_Text textPrefab;
    [SerializeField]
    protected Transform textParent;
    [SerializeField]
    protected ScrollRect scroll;
    [SerializeField]
    protected ContentSizeFitter contentSizeFitter;

    protected float currentDelay;

    [Header("가이드관련")]
    [SerializeField]
    private ProfileGuidePanel guidePanel;

    public void Init()
    {
        currentValue = GetComponent<RectTransform>().sizeDelta.x;
        //스크롤뷰 가장 밑으로 내리기;
        ConnectEvent();
        OpenCloseButton.onClick.AddListener(HidePanel);
        movePanelRect = GetComponent<RectTransform>();
        guidePanel.Init();
        AddSaveTexts();

        SetScrollView();

        ShowPanel();
    }

    protected virtual void ConnectEvent()
    {
        EventManager.StartListening(EProfileEvent.ProfileSendMessage, PrintText);
    }

    private void PrintText(object[] ps)
    {
        if (ps[0] == null || !(ps[0] is TextData))
        {
            return;
        }

        string msg = (ps[0] as TextData).text;
        CreateTextUI(msg);
    }

    private void AddSaveTexts()
    {
        List<TextData> list = DataManager.Inst.SaveData.aiChattingList;
        
        foreach(TextData data in list)
        {
            CreateTextUI(data.text);
        }
    }

    private TMP_Text CreateTextUI(string msg)
    {
        TMP_Text textUI = Instantiate(textPrefab, textParent);
        textUI.text = msg;

        SetLastWidth();
        LayoutRebuilder.ForceRebuildLayoutImmediate(textUI.rectTransform);
        SetScrollView();
        textUI.gameObject.SetActive(true);
        SetLastWidth();

        return textUI;
    }

    protected virtual void HidePanel()
    {
        if (isMoving) return;
        isMoving = true;
        loadingPanel.SetActive(true);
        movePanelRect.DOSizeDelta(new Vector2(hideValue, 0), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            currentValue = hideValue;
            OpenCloseButton.onClick.RemoveAllListeners();
            OpenCloseButton.onClick.AddListener(ShowPanel);
            SetWidths();
            SetScrollView();
            hideImage.SetActive(false);
            showImage.SetActive(true);
            isMoving = false;
            loadingPanel.SetActive(false);
            guidePanel.SetGuideParentWeight(false);
        });

    }

    protected virtual void ShowPanel()
    {
        if (isMoving) return;
        isMoving = true;
        loadingPanel.SetActive(true);
        movePanelRect.DOSizeDelta(new Vector2(showValue, 0), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            currentValue = showValue;
            OpenCloseButton.onClick.RemoveAllListeners();
            OpenCloseButton.onClick.AddListener(HidePanel);
            SetWidths();
            SetScrollView();
            hideImage.SetActive(true);
            showImage.SetActive(false);
            isMoving = false;
            loadingPanel.SetActive(false);
            guidePanel.SetGuideParentWeight(true);
        });
    }

    protected void SetScrollView()
    {
        if(this.gameObject.activeInHierarchy)
        {
            StartCoroutine(ScrollCor());
        }
        else
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
            scroll.verticalNormalizedPosition = 0;
        }
    }

    protected IEnumerator ScrollCor()
    {
        yield return new WaitForSeconds(0.025f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
        scroll.verticalNormalizedPosition = 0;
    }

    protected void SetWidths()
    {
        RectTransform[] rects = textParent.GetComponentsInChildren<RectTransform>();
        foreach (var temp in rects)
        {
            temp.sizeDelta = new Vector2(currentValue - 60, 0);
        }
    }
    protected void SetLastWidth()
    {
        RectTransform[] rects = textParent.GetComponentsInChildren<RectTransform>();
        rects[rects.Length - 1].sizeDelta = new Vector2(currentValue - 60, 0);
    }
}
