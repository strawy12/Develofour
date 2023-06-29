﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using System;

using static Constant;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;

public class NoticePanel : MonoUI, IPointerEnterHandler, IPointerExitHandler
{
    public float noticeAlphalightly = 1f;

    [SerializeField]
    public TMP_Text headText;
    [SerializeField]
    private TMP_Text bodyText;
    [SerializeField]
    private TMP_Text dateText;
    [SerializeField]
    private TMP_Text sameTagText;
    [SerializeField]
    private RectTransform textSort;
    [SerializeField]
    private HorizontalLayoutGroup horizontalLayoutGroup;

    [SerializeField]
    private Image iconImage;

    private ContentSizeFitter contentSizeFitter;

    [SerializeField]
    private bool canDeleted = true;

    public Action<NoticePanel> OnCompeleted;
    public Action<NoticePanel> OnClosed;

    private RectTransform rectTransform;
    private Image backgroundImage;

    private TouchDragNotice dragNotice;

    private Coroutine stopDelayCoroutine = null;

    private bool isEnter;
    private bool isOpen;
    private bool isEmpahasis;

    private bool isCompleted = false;

    private Canvas canvas;
    //연장 변수
    private float addTime = 2f;
    private bool isNoticeExtend;
    private int extendCount = 0;

    public string HeadText { get { return headText.text; } }
    private void Bind()
    {
        canvas ??= GetComponent<Canvas>();
        canvasGroup ??= GetComponent<CanvasGroup>();
        rectTransform ??= GetComponent<RectTransform>();
        backgroundImage ??= GetComponent<Image>();

        dragNotice ??= GetComponent<TouchDragNotice>();
        contentSizeFitter ??= GetComponent<ContentSizeFitter>();
    }

    public void Init(bool canDelete)
    {
        Bind();

        if(!canDeleted)
        {
            EnableTouchDragNotice(false);
            SetActive(true);
            return;
        }
        
        canDeleted = canDelete;
        OnCompeleted += (x) => Compelete();
        sameTagText.gameObject.SetActive(false);
        dragNotice.OnClickNotice += Drag;
        dragNotice.OnChangeAlpha += ChangeAlpha;
        dragNotice.OnDragNotice += ImmediatelyStop;
    }

    public void Notice(NoticeDataSO data, bool isOpenSystem)
    {
        sameTagText.text = data.sameTextString;
        Notice(data.Head, data.Body, data.Icon, data.noticeData.color, isOpenSystem);
    }
    private void NoticeSetting(string head, string body, Sprite icon, Color color)
    {
        headText.SetText(head);
        bodyText.SetText(body);
        DateTime time  = TimeSystem.TimeCount();

        string timeText = "";
        if(time.Hour > 12)
        {
            timeText = $"오후 {time.Hour - 12:00}:{time.Minute:00}";
        }
        else if(time.Hour == 12)
        {
            timeText = $"오후 {time.Hour}:{time.Minute:00}";
        }
        else if(time.Hour == 0)
        {
            timeText = $"오전 {12}:{time.Minute:00}";
        }
        else
        {
            timeText = $"오전 {time.Hour:00}:{time.Minute:00}";
        }
        dateText.SetText(timeText);

        if (icon != null)
        {
            iconImage.gameObject.SetActive(true);
            iconImage.sprite = icon;
            iconImage.color = color;
            Vector2 vec = textSort.sizeDelta;
            vec.x = 230;
            horizontalLayoutGroup.padding.left = 10;
            textSort.sizeDelta = vec;
        }
        else
        {
            iconImage.gameObject.SetActive(false);
            Vector2 vec = textSort.sizeDelta;
            vec.x = 330;
            horizontalLayoutGroup.padding.left = 23;
            textSort.sizeDelta = vec;
        }

        rectTransform.anchorMin = new Vector2(1f, 0.5f);
        rectTransform.anchorMax = new Vector2(1f, 0.5f);
        rectTransform.pivot = new Vector2(1f, 0f);
        rectTransform.localScale = Vector3.one;

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
    }
    public void Notice(string head, string body, Sprite icon, Color color, bool isOpenSystem)
    {
        NoticeSetting(head, body, icon, color);

        Vector2 pos = new Vector2(rectTransform.rect.width, NOTICE_POS.y);
        rectTransform.anchoredPosition = pos;
        rectTransform.localScale = Vector3.one;

        SetActive(true);

        Sound.OnPlaySound?.Invoke(Sound.EAudioType.Notice);

        if (isOpenSystem)
        {
            NoticeSystem.OnTagReset?.Invoke();
            OnCompeleted?.Invoke(this);
            return;
        }

        EventManager.TriggerEvent(ENoticeEvent.GeneratedNotice);

        NoticeUXEmphasis();

        stopDelayCoroutine = StartCoroutine(NoticeCoroutine());
    }

    public void LoadNotice(NoticeData data) 
    {
        NoticeSetting(data.head, data.body, data.icon, data.color);
    }

    public void NoticeUXEmphasis()
    {
        Sequence seq = DOTween.Sequence();

        isEmpahasis = true;
        seq.Append(rectTransform.DOScale(1.25f, NOTICE_DURATION + NOTICE_SIZE_DURATION));
        seq.Join(rectTransform.DOAnchorPosX(NOTICE_POS.x, NOTICE_DURATION));
        seq.AppendCallback(() =>
        {
            isOpen = true;

            Sequence seq2 = DOTween.Sequence();

            seq2.Append(rectTransform.DOScale(1f, NOTICE_DURATION));
            seq2.AppendCallback(() =>
            {
                isEmpahasis = false;
            });

        });
    }

    private void StartDrag()
    {
        if (stopDelayCoroutine != null)
        {
            StopCoroutine(stopDelayCoroutine);
            stopDelayCoroutine = null;
        }
    }

    private void EndDrag()
    {
        if (stopDelayCoroutine == null)
        {
            stopDelayCoroutine = StartCoroutine(NoticeCoroutine());
        }
    }

    private void Drag()
    {
        if (dragNotice.isClick)
        {
            StartDrag();
        }
        else
        {
            EndDrag();
        }
    }

    private void ChangeAlpha()
    {
        if (dragNotice.isInvisibility)
        {
            canvasGroup.alpha = noticeAlphalightly / 2;
        }
        else if (!dragNotice.isInvisibility)
        {
            canvasGroup.alpha = noticeAlphalightly;
        }
    }


    public void ImmediatelyStop()
    {
        rectTransform.DOKill();
        StopAllCoroutines();
        NoticeData data = new NoticeData();
        data.head = headText.text;
        data.body = bodyText.text;
        data.icon = iconImage.sprite;
        data.canDeleted = true;
        data.delay = 0; 
        OnCompeleted?.Invoke(this);
        EventManager.StopListening(ENoticeEvent.OpenNoticeSystem, NoticeStopEvent);
    }

    public void EnableTouchDragNotice(bool isEnabled)
    {
        dragNotice.enabled = isEnabled;
    }

    private void Compelete()
    {
        if (isEnter)
        {
            backgroundImage.DOColor(new Color(0f, 0f, 0f), 0.1f);
            rectTransform.DOScale(Vector3.one,0.1f);
            isEnter = false;
        }

        if (isCompleted)
        {
            SetActive(false);
            OnClosed?.Invoke(this);
        }

        if (canDeleted == false)
        {
            EnableTouchDragNotice(false);
        }
        
        if(isOpen)
        {
            isOpen = false;
        }

        StopAllCoroutines();
        isCompleted = !isCompleted;
    }

    private IEnumerator NoticeCoroutine()
    {
        yield return new WaitForSeconds(NOTICE_DELAYTIME);

        //yield return new WaitForSeconds(addTime);

        if(isNoticeExtend)
        {
            isNoticeExtend = false;
            ExtendNotice();
        }
        else
        {
            StartCoroutine(NoticeCoroutineEnd());
        }
    }

    private void ExtendNotice()
    {
        StopAllCoroutines();
        StartCoroutine(ExtendNoticeCoroutine());
    }

    private IEnumerator NoticeCoroutineEnd()
    {
        rectTransform.DOAnchorPosX(rectTransform.rect.width, NOTICE_DURATION);
        yield return new WaitForSeconds(NOTICE_DURATION);
        NoticeSystem.OnTagReset?.Invoke();
        OnCompeleted?.Invoke(this);
    }

    private IEnumerator ExtendNoticeCoroutine()
    {
        //이 함수가 5번 실행되면 그냥 imme삭제 하면 될거같은데..?
        yield return new WaitForSeconds(addTime);

        //패널마다 extendcount가 달라

        extendCount++;

        if (extendCount == 4)
        {
            StartCoroutine(NoticeCoroutineEnd());
        }

        if (isNoticeExtend)
        {
            isNoticeExtend = false;
            StartCoroutine(ExtendNoticeCoroutine());
        }
        else
        {
            StartCoroutine(NoticeCoroutineEnd());
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isEmpahasis)
            return;

        if (!isEnter && isOpen)
        {
            backgroundImage.DOColor(new Color(0.1f, 0.1f, 0.1f, 0.9f), 0.1f);
            rectTransform.DOScale(Vector3.one * 1.05f, 0.1f);

            isEnter = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isEnter && isOpen)
        {
            backgroundImage.DOColor(new Color(0f, 0f, 0f, 0.9f), 0.1f);
            rectTransform.DOScale(Vector3.one, 0.1f);

            isEnter = false;
        }
    }

    public void SameTagTextAdd(string str, bool isEnd = false)
    {
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.Notice);
        isNoticeExtend = true;
        string saveStr = bodyText.text;
        saveStr += '\n';
        saveStr += '\n';
        saveStr += str;
        bodyText.text = saveStr;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
    }

    private void NoticeStopEvent(object[] ps)
    {
        if (isCompleted) return;
        ImmediatelyStop();
    }

    public void CanvasSortingSetting()
    {
        if (canvas != null)
            canvas.overrideSorting = false;
    }

    private void OnEnable()
    {
        EventManager.StartListening(ENoticeEvent.OpenNoticeSystem, NoticeStopEvent);
    }

    private void OnDisable()
    {
        EventManager.StopListening(ENoticeEvent.OpenNoticeSystem, NoticeStopEvent); 
    }

    private void OnDestroy()
    {
        EventManager.StopListening(ENoticeEvent.OpenNoticeSystem, NoticeStopEvent);
    }
}
