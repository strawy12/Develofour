using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class MenuAttributeUI : MonoUI, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float expandDelayTime = 0.8f;

    private RectTransform rectTransform;

    private bool isOpen = false;
    private bool isExpand = false;

    private bool isStop = false;

    private Vector2 contractSize = new Vector2(60f, 0f);
    private Vector2 expendSize = new Vector2(320f, 800f);

    private Coroutine expandCoroutine = null;

    public TMP_Text nameText;

    [SerializeField]
    private ProfileOverlayOpenTrigger overlayTrigger;

    private void Awake()
    {
        Bind();
        Init();
    }
    private void Bind()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        overlayTrigger ??= GetComponent<ProfileOverlayOpenTrigger>();
    }

    private void Init()
    {
        MenuAttributePanel[] btns = GetComponentsInChildren<MenuAttributePanel>();

        foreach (MenuAttributePanel btn in btns)
        {
            btn.OnEnter += OnPointerEnter;
        }

        EventManager.StartListening(EWindowEvent.ExpendMenu, ToggleMenu);
        EventManager.StartListening(EWindowEvent.ActivePowerPanel, ActivePowerPanel);
        EventManager.StartListening(EWindowEvent.CloseAttribute, Close);
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
    }

    private void CheckClose(object[] hits)
    {
        if(isStop) { return; }
        if (!isOpen) { return; }

        if (Define.ExistInHits(gameObject, hits[0]) == false)
        {
            Close();
        }
    }

    public void Show()
    {
        if (isOpen) { return; }
        isOpen = true;

        rectTransform.sizeDelta = contractSize;

        SetActive(true);
        overlayTrigger.fileID = Constant.OverlayID.START_ATTRIBUTE.ToString();
        overlayTrigger.Open();

        DOTween.To(
            () => rectTransform.sizeDelta.y,
            (value) => rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, value),
            expendSize.y,
            0.2f
        ).SetEase(Ease.InCubic);
    }

    public void Close(object[] ps)
    {
        Close();
    }

    public void Close()
    {
        if (isOpen == false) { return; }
        overlayTrigger.Close();
        isOpen = false;
        SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isStop) { return; }
        if (!isExpand)
        {
            if (expandCoroutine != null)
            {
                StopCoroutine(expandCoroutine);
            }

            expandCoroutine = StartCoroutine(ExpandDelayCoroutine());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isStop) { return; }

        if (Raycast(eventData))
        {
            return;
        }

        isExpand = false;

        CheckExpendCoroutine();
        ContractPanel();
    }

    private void CheckExpendCoroutine()
    {
        if (expandCoroutine != null)
        {
            StopCoroutine(expandCoroutine);
            expandCoroutine = null;
        }
    }

    private void ActivePowerPanel(object[] ps)
    {
        isStop = (bool)ps[0];

        if (isStop)
        {
            StopAllCoroutines();
        }

        //else if (isExpand)
        //{
        //    PointerEventData data = new PointerEventData(EventSystem.current);
        //    data.position = Input.mousePosition;
        //    OnPointerExit(data);
        //}
    }

    private bool Raycast(PointerEventData eventData)
    {
        List<RaycastResult> hits = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, hits);
        foreach (RaycastResult hit in hits)
        {
            if (hit.gameObject == gameObject)
            {
                return true;
            }
        }

        return false;
    }

    private IEnumerator ExpandDelayCoroutine()
    {
        yield return new WaitForSeconds(expandDelayTime);

        expandCoroutine = null;
        ExpandPanel();
    }

    // 확장
    private void ExpandPanel()
    {
        CheckExpendCoroutine();

        DOTween.KillAll(true);

        DOTween.To(
            () => rectTransform.sizeDelta.x,
            (value) => rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y),
            expendSize.x,
            0.25f
        ).SetEase(Ease.OutCubic);

        isExpand = true;
    }

    //축소
    private void ContractPanel()
    {
        DOTween.KillAll(true);

        DOTween.To(
            () => rectTransform.sizeDelta.x,
            (value) => rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y),
            contractSize.x,
            0.25f
        ).SetEase(Ease.OutCubic);

        isExpand = false;
    }

    private void ToggleMenu(object[] obj)
    {
        if (isExpand)
        {
            ContractPanel();
        }
        else
        {
            ExpandPanel();
        }
    }
}
