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

public class NoticePanel : MonoUI, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private TMP_Text headText;

    [SerializeField]
    private TMP_Text bodyText;

    [SerializeField]
    private TMP_Text dateText;

    public Action<NoticePanel> OnCompeleted;

    private RectTransform rectTransform;
    private Image backgroundImage;

    private TouchDragNotice dragNotice;

    private Coroutine stopDelayCoroutine = null;

    private bool isEnter;

    private void Bind()
    {
        canvasGroup ??= GetComponent<CanvasGroup>();
        rectTransform ??= GetComponent<RectTransform>();
        backgroundImage ??= GetComponent<Image>();

        dragNotice ??= GetComponent<TouchDragNotice>();
    }

    public void Init()
    {
        Bind();
        OnCompeleted += (x) => Compelete();

        dragNotice.OnClickNotice += () => NoticePanelStartEndDrag();
        dragNotice.OnChangeAlpha += () => NoticePanelAlphalightly();
        dragNotice.OnDragNotice += () => OnCompeleted?.Invoke(this);

        EventManager.StartListening(ENoticeEvent.OpenNoticeSystem, (obj) => ImmediatelyStop());
    }

    public void Notice(NoticeDataSO data)
    {
        headText.SetText(data.Head);
        bodyText.SetText(data.Body);

        rectTransform.anchorMin = new Vector2(1f, 0.5f);
        rectTransform.anchorMax = new Vector2(1f, 0.5f);
        rectTransform.pivot = new Vector2(1f, 0.5f);

        Vector2 pos = new Vector2(rectTransform.rect.width, NOTICE_POS.y);
        rectTransform.anchoredPosition = pos;

        EventManager.TriggerEvent(ENoticeEvent.GeneratedNotice);
        rectTransform.DOAnchorPosX(NOTICE_POS.x, NOTICE_DURATION);

        Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.WindowAlarmSound);

        stopDelayCoroutine = StartCoroutine(NoticeCoroutine());
    }

    private void NoticePanelStartEndDrag()
    {
        if(dragNotice.isClick) // 드래그 시작
        {
            if (stopDelayCoroutine != null)
            {
                StopCoroutine(stopDelayCoroutine);
                stopDelayCoroutine = null;
            }
        }
        else if(!dragNotice.isClick)
        {
            if (stopDelayCoroutine == null)
            {
                stopDelayCoroutine = StartCoroutine(NoticeCoroutine());
            }
        }
    }

    private void NoticePanelAlphalightly()
    {
        if (dragNotice.isInvisibility)
        {
            canvasGroup.alpha = 0.5f;
        }
        else if (!dragNotice.isInvisibility)
        {
            canvasGroup.alpha = 1f;
        }
    }


    public void ImmediatelyStop()
    {
        rectTransform.DOKill();
        StopAllCoroutines();
        OnCompeleted?.Invoke(this);
    }

    private void Compelete()
    {
        if (isEnter)
        {
            backgroundImage.DOColor(new Color(0f, 0f, 0f), 0.1f);
            rectTransform.DOScale(Vector3.one, 0.1f);

            isEnter = false;
        }
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
        ImmediatelyStop();
    }

    private void OnEnable()
    {
        EventManager.StartListening(ENoticeEvent.OpenNoticeSystem, NoticeStopEvent);
    }

    private void OnDisable()
    {

        EventManager.StartListening(ENoticeEvent.OpenNoticeSystem, NoticeStopEvent);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(ENoticeEvent.OpenNoticeSystem, NoticeStopEvent);
    }
}
