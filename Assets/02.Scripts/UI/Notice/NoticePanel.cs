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
    public TMP_Text headText;
    [SerializeField]
    private TMP_Text bodyText;
    [SerializeField]
    private TMP_Text dateText;
    [SerializeField]
    private TMP_Text sameTagText;

    private string saveText;

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

    private float addTime = 2f;

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
        sameTagText.gameObject.SetActive(false);
        dragNotice.OnClickNotice += Drag;
        dragNotice.OnChangeAlpha += ChangeAlpha;
        dragNotice.OnDragNotice += ImmediatelyStop;
    }

    public void Notice(NoticeDataSO data)
    {
        sameTagText.text = data.sameTextString;
        Notice(data.Head, data.Body, data.Icon);
    }
    private void NoticeSetting(string head, string body, Sprite icon)
    {
        headText.SetText(head);
        bodyText.SetText(body);
        iconImage.sprite = icon;

        rectTransform.anchorMin = new Vector2(1f, 0.5f);
        rectTransform.anchorMax = new Vector2(1f, 0.5f);
        rectTransform.pivot = new Vector2(1f, 0f);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);

    }
    public void Notice(string head, string body, Sprite icon)
    {
        NoticeSetting(head, body, icon);

        Vector2 pos = new Vector2(rectTransform.rect.width, NOTICE_POS.y);
        rectTransform.anchoredPosition = pos;

        SetActive(true);

        //EventManager.TriggerEvent(ENoticeEvent.GeneratedNotice);
        rectTransform.DOAnchorPosX(NOTICE_POS.x, NOTICE_DURATION);

        NoticeUXEmphasis();

       // Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.WindowAlarmSound);

        stopDelayCoroutine = StartCoroutine(NoticeCoroutine());
    }

    public void LoadNotice(NoticeData data) 
    {
        NoticeSetting(data.head, data.body, data.icon);
    }

    public void NoticeUXEmphasis()
    {
        Sequence seq = DOTween.Sequence();

        isEmpahasis = true;
        seq.Append(rectTransform.DOScale(1.25f, scaleGotDuration / 2));
        seq.AppendCallback(() =>
        {
            isOpen = true;

            Sequence seq2 = DOTween.Sequence();

            seq2.Append(rectTransform.DOScale(1f, scaleGotDuration / 2));
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

        yield return new WaitForSeconds(addTime);

        rectTransform.DOAnchorPosX(rectTransform.rect.width, NOTICE_DURATION);
        yield return new WaitForSeconds(NOTICE_DURATION);

        NoticeSystem.OnTagReset?.Invoke();
        OnCompeleted?.Invoke(this);
        
        addTime = 0f;
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

    public void SameTagTextAdd(string str)
    {
        addTime = 2;
        string saveStr = bodyText.text;
        saveStr += '\n';
        saveStr += str;
        bodyText.text = saveStr;
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)contentSizeFitter.transform);
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
