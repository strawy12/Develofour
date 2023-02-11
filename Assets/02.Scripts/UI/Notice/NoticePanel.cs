using System.Collections;
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
    public float scaleGotDuration = 1f;
    public float noticeAlphalightly = 1f;

    [SerializeField]
    private TMP_Text headText;
    [SerializeField]
    private TMP_Text bodyText;
    [SerializeField]
    private TMP_Text dateText;

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

    private bool isCompleted = false;

    public string HeadText { get { return headText.text; } }
    private void Bind()
    {
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

        dragNotice.OnClickNotice += Drag;
        dragNotice.OnChangeAlpha += ChangeAlpha;
        dragNotice.OnDragNotice += ImmediatelyStop;
    }

    public void Notice(NoticeDataSO data)
    {
        Notice(data.Head, data.Body, data.Icon);
    }

    public void Notice(string head, string body, Sprite icon)
    {
        headText.SetText(head);
        bodyText.SetText(body);
        iconImage.sprite = icon;
        
        rectTransform.anchorMin = new Vector2(1f, 0.5f);
        rectTransform.anchorMax = new Vector2(1f, 0.5f);
        rectTransform.pivot = new Vector2(1f, 0f);

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);

        Vector2 pos = new Vector2(rectTransform.rect.width, NOTICE_POS.y);
        rectTransform.anchoredPosition = pos;

        SetActive(true);

        EventManager.TriggerEvent(ENoticeEvent.GeneratedNotice);
        rectTransform.DOAnchorPosX(NOTICE_POS.x, NOTICE_DURATION);

        NoticeUXEmphasis();

       // Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.WindowAlarmSound);

        stopDelayCoroutine = StartCoroutine(NoticeCoroutine());
    }


    public void NoticeUXEmphasis()
    {
        Sequence seq = DOTween.Sequence();

        seq.Append(rectTransform.DOScale(1.25f, scaleGotDuration / 2));
        seq.AppendCallback(() =>
        {
            rectTransform.DOScale(1f, scaleGotDuration / 2);
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
            rectTransform.DOScale(Vector3.one, 0.1f);

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

        StopAllCoroutines();
        isCompleted = !isCompleted;
    }

    private IEnumerator NoticeCoroutine()
    {
        yield return new WaitForSeconds(NOTICE_DELAYTIME);

        rectTransform.DOAnchorPosX(rectTransform.rect.width, NOTICE_DURATION);
        yield return new WaitForSeconds(NOTICE_DURATION);
        OnCompeleted?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isEnter)
        {
            backgroundImage.DOColor(new Color(0.1f, 0.1f, 0.1f, 0.9f), 0.1f);
            rectTransform.DOScale(Vector3.one * 1.05f, 0.1f);

            isEnter = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isEnter)
        {
            backgroundImage.DOColor(new Color(0f, 0f, 0f, 0.9f), 0.1f);
            rectTransform.DOScale(Vector3.one, 0.1f);

            isEnter = false;
        }
    }

    private void NoticeStopEvent(object[] ps)
    {
        if (isCompleted) return;
        ImmediatelyStop();
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
