﻿using System.Collections;
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

public class ProfilerChatting : MonoUI
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
    private ProfileGuidePanel profileGuidePanel;
    private float defaultOffsetMinY;
    [SerializeField]
    protected GameObject loadingPanel;


    [SerializeField]
    protected float moveDuration;
    protected bool isMoving;
    protected RectTransform movePanelRect;

    protected float currentValue;
    [SerializeField]
    protected ProfilerAIText textPrefab;
    [SerializeField]
    protected GameObject imagePrefab;

    [SerializeField]
    protected NewAIChattingImage newImagePrefab;

    public bool isUsingNewImage;

    [SerializeField]
    protected Transform textParent;

    public int ChattingCount
    {
        get
        {
            if (textParent.childCount <= 3)
                return 0;
            return textParent.childCount - 3;
        }
    }

    public int saveChatCount;

    [SerializeField]
    protected ScrollRect scroll;
    [SerializeField]
    protected RectTransform scrollrectTransform;
    [SerializeField]
    protected ContentSizeFitter contentSizeFitter;

    [SerializeField]
    private ProfilerWindow profiler;

    private bool isFirstChat;

    public void Init()
    {
        saveChatCount = DataManager.Inst.AIChattingListCount();
        EventManager.StartListening(EProfilerEvent.ProfilerSendMessage, PrintChat);
        AddSaveTexts();
        currentValue = GetComponent<RectTransform>().sizeDelta.x;
        OpenCloseButton.onClick.AddListener(HidePanel);
        movePanelRect = GetComponent<RectTransform>();
        profileGuidePanel.Init();
        SetScrollView();

        ShowPanel();
        defaultOffsetMinY = scrollrectTransform.offsetMax.y;
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
            profileGuidePanel.SetGuideParentWeight(true);
            isMoving = false;
            loadingPanel.SetActive(false);
        });
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
            profileGuidePanel.SetGuideParentWeight(false);
            isMoving = false;
            loadingPanel.SetActive(false);
        });
    }
    protected void SetWidths()
    {
        RectTransform[] rects = textParent.GetComponentsInChildren<RectTransform>();
        foreach (var temp in rects)
        {
            if(temp.gameObject.GetComponent<Image>() == null)
            {
                temp.sizeDelta = new Vector2(currentValue - 60, 0);
            }
            else if (temp.gameObject.name == "Line")
            {
                temp.sizeDelta = new Vector2(700, 1);
            }
        }
    }
    private void PrintChat(object[] ps)
    {
        if (ps[0] == null)
        {
            return;
        }

        if (ps[0] is string)
        {
            string msg = (ps[0] as string);
            CreateTextUI(msg);
        }

        if (ps[0] is Sprite)
        {
            Sprite sprite = ps[0] as Sprite;
            float sizeY = 0;
            if (ps.Length == 2)
            {
                sizeY = (float)ps[1];
            }

            if (sizeY == 0)
                CreateImageUI(sprite);
            else
                CreateImageUI(sprite, sizeY);
        }

        SetScrollView();

    }

    private void AddSaveTexts()
    {
        List<AIChat> list = DataManager.Inst.SaveData.aiChattingList;

        foreach (AIChat data in list)
        {
            if (data.sprite == null && data.text != null)
            {
                CreateTextUI(data.text);
            }
            else if (data.sprite != null)
            {
                CreateImageUI(data.sprite);
            }
        }
    }

    private void CheckNewChatting()
    {
        if (profiler == null) return;

        //if ((ISelectable)profiler != WindowManager.Inst.SelectedObject
        //    || ((ISelectable)profiler == WindowManager.Inst.SelectedObject && canvasGroup.interactable == false))
        //{
        //    if (!isUsingNewImage)
        //    {
        //        isUsingNewImage = true;
        //        ActiveNewImageUI(true);
        //    }
        //}

        SetScrollView();//스크롤뷰 가장 밑으로 내리기;
    }

    private TMP_Text CreateTextUI(string msg)
    {

        CheckNewChatting();

        ProfilerAIText textUI = Instantiate(textPrefab, textParent);
        textUI.text.text = msg;


        SetLastWidth();
        LayoutRebuilder.ForceRebuildLayoutImmediate(textUI.text.rectTransform);
        SetScrollView();
        textUI.gameObject.SetActive(true);
        SetLastWidth();


        if (!isFirstChat)
        {
            isFirstChat = true;
        }
        else
        {
            if(!ProfilerChattingSystem.isFirstChatting)
            {
                textUI.line.SetActive(true);
            }
        }

        saveChatCount = ChattingCount;

        return textUI.text;
    }


    private GameObject CreateImageUI(Sprite sprite, float YSize = 100)
    {
        CheckNewChatting();

        GameObject imageUI = Instantiate(imagePrefab, textParent); //이미지 프리팹 생성

        Image image = imageUI.transform.GetChild(0).GetComponent<Image>(); //이미지 컴포넌트 가져오고

        float x = sprite.bounds.size.x;
        float y = sprite.bounds.size.y;

        float remain = YSize / x;

        float spriteY = y * remain;

        Vector2 size = new Vector2(YSize, spriteY);

        image.GetComponent<RectTransform>().sizeDelta = size; //크기 맞춰주고

        image.sprite = sprite; //스프라이트 변경

        RectTransform imageRect = imageUI.GetComponent<RectTransform>(); //자식의 이미지 크기랑 height랑 같게함
        imageRect.sizeDelta = new Vector2(imageRect.sizeDelta.x, spriteY);

        SetLastWidth();
        LayoutRebuilder.ForceRebuildLayoutImmediate(imageRect);
        SetScrollView();
        imageUI.gameObject.SetActive(true);
        SetLastWidth();

        saveChatCount = ChattingCount;

        return imageUI;
    }

    private void ActiveNewImageUI(bool flag)
    {
        if (ChattingCount == 0) return;

        newImagePrefab.transform.SetAsLastSibling();
        newImagePrefab.gameObject.SetActive(flag);
        SetScrollView();
    }

    protected void SetScrollView()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
        scroll.verticalNormalizedPosition = 0;
    }

    protected void SetLastWidth()
    {
        RectTransform[] rects = textParent.GetComponentsInChildren<RectTransform>();
        if (rects[rects.Length - 1].gameObject.GetComponent<Image>() == null)
        {
            rects[rects.Length - 1].sizeDelta = new Vector2(currentValue - 60, 0);
        }
    }


    protected void OnDestroy()
    {
        EventManager.StopListening(EProfilerEvent.ProfilerSendMessage, PrintChat);
    }
}
