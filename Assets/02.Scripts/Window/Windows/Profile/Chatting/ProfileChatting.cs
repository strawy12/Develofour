using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;
using Unity.VisualScripting;

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
    protected float showWeightValue;
    [SerializeField]
    protected float hideWeightValue;
    [SerializeField]
    protected float showHeightValue;
    [SerializeField]
    protected float hideHeightValue;

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
    protected RectTransform scrollrectTransform;
    [SerializeField]
    protected ContentSizeFitter contentSizeFitter;
    [SerializeField]
    private ProfileGuidePanel profileGuidePanel;

    protected float currentDelay;

    private float defaultOffsetMinY;


    public void Init()
    {
        EventManager.StartListening(EProfileEvent.ProfileSendMessage, PrintText);
        currentValue = GetComponent<RectTransform>().sizeDelta.x;
        //스크롤뷰 가장 밑으로 내리기;
        OpenCloseButton.onClick.AddListener(HidePanel);
        movePanelRect = GetComponent<RectTransform>();
        profileGuidePanel.Init();
        AddSaveTexts();

        SetScrollView();

        ShowPanel();

        defaultOffsetMinY = scrollrectTransform.offsetMax.y;
        EventManager.StartListening(EProfileEvent.ClickGuideToggleButton, SetChattingHeight);
    }

    private void SetChattingHeight(object[] ps)
    {
        if (ps[0] == null)
        {
            return;
        }

        bool isGuideOnOff = (bool)ps[0];

        if (isGuideOnOff) // 가이드 패널 오픈 시
        {
            if (isMoving) return;
            isMoving = true;

            DG.Tweening.Sequence seq = DOTween.Sequence();

            seq.Append(
                DOTween.To(() => scrollrectTransform.offsetMin, (x) => scrollrectTransform.offsetMin = x, new Vector2(0, 200), 0.2f));
            
            seq.AppendCallback(() =>
            {
                isMoving = false;
                SetScrollView();
            });
        }
        else if (!isGuideOnOff) // 가이드 패널 Close 시
        {
            if (isMoving) return;
            isMoving = true;

            DG.Tweening.Sequence seq = DOTween.Sequence();

            seq.Append(
                DOTween.To(() => scrollrectTransform.offsetMin, (x) => scrollrectTransform.offsetMin = x, new Vector2(0, 0), 0.2f));

            seq.AppendCallback(() =>
            {
                isMoving = false;
                SetScrollView();
            });
        }
    }

    private void PrintText(object[] ps)
    {
        if (ps[0] == null || !(ps[0] is string))
        {
            return;
        }

        string msg = (ps[0] as string);
        CreateTextUI(msg);
    }

    private void AddSaveTexts()
    {
        List<string> list = DataManager.Inst.SaveData.aiChattingList;

        foreach (string data in list)
        {
            CreateTextUI(data);
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
        movePanelRect.DOSizeDelta(new Vector2(hideWeightValue, 0), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            currentValue = hideWeightValue;
            OpenCloseButton.onClick.RemoveAllListeners();
            OpenCloseButton.onClick.AddListener(ShowPanel);
            SetWidths();
            SetScrollView();
            hideImage.SetActive(false);
            showImage.SetActive(true);
            isMoving = false;
            loadingPanel.SetActive(false);
        });
    }

    protected virtual void ShowPanel()
    {
        if (isMoving) return;
        isMoving = true;
        loadingPanel.SetActive(true);
        movePanelRect.DOSizeDelta(new Vector2(showWeightValue, 0), moveDuration).SetEase(Ease.Linear).OnComplete(() =>
        {
            currentValue = showWeightValue;
            OpenCloseButton.onClick.RemoveAllListeners();
            OpenCloseButton.onClick.AddListener(HidePanel);
            SetWidths();
            SetScrollView();
            hideImage.SetActive(true);
            showImage.SetActive(false);
            isMoving = false;
            loadingPanel.SetActive(false);
        });
    }

    protected void SetScrollView()
    {
        if (this.gameObject.activeInHierarchy)
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


    protected void OnDestroy()
    {
        EventManager.StopListening(EProfileEvent.ProfileSendMessage, PrintText);
        EventManager.StopListening(EProfileEvent.ClickGuideToggleButton, SetChattingHeight);

    }
}
