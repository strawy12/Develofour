using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuAttributeUI : MonoUI, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float expandDelayTime = 2f;

    private RectTransform rectTransform;

    private bool isExpand = false;

    private bool isStop = false;

    private Vector2 contractSize = new Vector2(60f, 0f);
    private Vector2 expendSize = new Vector2(320f, 800f);

    private Coroutine expandCoroutine = null;

    private void Awake()
    {
        Bind();
        Init();
    }

    private void Init()
    {
        MenuAttributePanel[] btns = GetComponentsInChildren<MenuAttributePanel>();

        foreach (MenuAttributePanel btn in btns)
        {
            btn.OnEnter += OnPointerEnter;
        }

        EventManager.StartListening(EEvent.ExpendMenu, ToggleMenu);
        EventManager.StartListening(EEvent.ActivePowerPanel, (obj) =>
        {
            isStop = (bool)obj;

            if (isStop)
            {
                StopAllCoroutines();
            }
        });

        EventManager.StartListening(EEvent.LeftButtonClick, CheckClose);
    }

    private void Bind()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void CheckClose(object hits)
    {
        if (Define.ExistInHits(gameObject, hits) == false)
        {
            Close();
        }
    }

    public void Show()
    {
        rectTransform.sizeDelta = contractSize;

        SetActive(true);

        DOTween.To(
            () => rectTransform.sizeDelta.y,
            (value) => rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, value),
            expendSize.y,
            0.2f
        ).SetEase(Ease.InCubic);
    }

    public void Close()
    {
        SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isStop) return;
        if (!isExpand)
        {
            if(expandCoroutine != null)
            {
                StopCoroutine(expandCoroutine);
            }

            expandCoroutine = StartCoroutine(ExpandDelayCoroutine());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isStop) return;

        if (Raycast(eventData)) return;

        isExpand = false;

        CheckCoroutine();
        ContractPanel();
    }

    private void CheckCoroutine()
    {

        if (expandCoroutine != null)
        {
            StopCoroutine(expandCoroutine);
            expandCoroutine = null;
        }
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

    private void ExpandPanel()
    {
        CheckCoroutine();

        DOTween.KillAll();

        DOTween.To(
            () => rectTransform.sizeDelta.x,
            (value) => rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y),
            expendSize.x,
            0.25f
        ).SetEase(Ease.OutCubic);

        isExpand = true;
    }

    private void ContractPanel()
    {
        DOTween.KillAll();

        DOTween.To(
            () => rectTransform.sizeDelta.x,
            (value) => rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y),
            contractSize.x,
            0.25f
        ).SetEase(Ease.OutCubic);

        isExpand = false;
    }

    private void ToggleMenu(object obj)
    {
        if(isExpand)
        {
            ContractPanel();
        }
        else
        {
            ExpandPanel();
        }
    }
}
