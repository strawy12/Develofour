using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;
using System;

using static Constant;

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

    private bool isEnter;

    private void Bind()
    {
        canvasGroup ??= GetComponent<CanvasGroup>();
        rectTransform ??= GetComponent<RectTransform>();
        backgroundImage ??= GetComponent<Image>();
    }

    public void Init()
    {
        Bind();
        OnCompeleted += (x) => Compelete();
        EventManager.StartListening(EEvent.OpenNoticeSystem, (obj) => ImmediatelyStop());
    }

    public void Notice(NoticeData data)
    {
        headText.SetText(data.head);
        bodyText.SetText(data.body);

        rectTransform.anchorMin = new Vector2(1f, 0.5f);
        rectTransform.anchorMax = new Vector2(1f, 0.5f);
        rectTransform.pivot = new Vector2(1f, 0.5f);

        Vector2 pos = new Vector2(rectTransform.rect.width, NOTICE_POS.y);
        rectTransform.anchoredPosition = pos;

        EventManager.TriggerEvent(EEvent.GeneratedNotice);
        rectTransform.DOAnchorPosX(NOTICE_POS.x, NOTICE_DURATION);

        StartCoroutine(NoticeCoroutine());
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
            StopAllCoroutines();

            backgroundImage.DOColor(new Color(0.1f, 0.1f, 0.1f, 0.9f), 0.1f);
            rectTransform.DOScale(Vector3.one * 1.05f, 0.1f);

            isEnter = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isEnter)
        {
            StartCoroutine(NoticeCoroutine());

            backgroundImage.DOColor(new Color(0f, 0f, 0f, 0.9f), 0.1f);
            rectTransform.DOScale(Vector3.one, 0.1f);

            isEnter = false;
        }
    }

    private void OnEnable()
    {
        EventManager.StartListening(EEvent.OpenNoticeSystem, (obj) => ImmediatelyStop());
    }

    private void OnDisable()
    {
        EventManager.StartListening(EEvent.OpenNoticeSystem, (obj) => ImmediatelyStop());
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EEvent.OpenNoticeSystem, (obj) => ImmediatelyStop());
    }
}
