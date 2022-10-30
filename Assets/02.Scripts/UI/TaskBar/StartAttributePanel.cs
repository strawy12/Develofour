using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StartAttributePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float expandDelayTime = 2f;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private bool isExpand = false;

    private void Awake()
    {
        Bind();

        StartAttributeBtn[] btns = GetComponentsInChildren<StartAttributeBtn>();

        foreach (StartAttributeBtn btn in btns)
        {
            btn.OnEnter += OnPointerEnter;
        }
    }

    private void Bind()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Show()
    {
        SetActive(true);
        rectTransform.sizeDelta = new Vector2(60f, 0f);

        Vector2 oldPivot = rectTransform.pivot;
        rectTransform.pivot = new Vector2(0.5f, 0f);

        DOTween.To(
            () => rectTransform.sizeDelta.y,
            (value) => rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, value),
            800f,
            0.1f
        ).SetEase(Ease.OutCubic).OnComplete(() =>
        {
            rectTransform.pivot = oldPivot;
        });
    }

    private void SetActive(bool isActive)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!isExpand)
        {
            isExpand = true;
            StartCoroutine(ExpandDelayCoroutine());
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Raycast(eventData)) return;

        isExpand = false;
        StopAllCoroutines();
        ContractPanel();
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

        DOTween.KillAll();

        DOTween.To(
            () => rectTransform.sizeDelta.x,
            (value) => rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y),
            320f,
            0.2f
        ).SetEase(Ease.OutCubic);
    }

    private void ContractPanel()
    {
        DOTween.KillAll();

        DOTween.To( 
            () => rectTransform.sizeDelta.x,
            (value) => rectTransform.sizeDelta = new Vector2(value, rectTransform.sizeDelta.y),
            70f,
            0.2f
        ).SetEase(Ease.OutCubic);
    }
}
