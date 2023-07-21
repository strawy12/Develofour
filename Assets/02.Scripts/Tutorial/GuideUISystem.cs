﻿using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class GuideUISystem : MonoBehaviour 
{
    [SerializeField]
    private Image guideUI;

    private bool isSign;

    public static Action<RectTransform> OnGuide;
    public static Action EndAllGuide;
    public static Action<RectTransform> EndGuide;
    public static Action<RectTransform> FullSizeGuide;
    public static Action CenterSizeGuide;

    private RectTransform currentRectTransform;


    private void Start()
    {
        guideUI.gameObject.SetActive(false);

        OnGuide += StartGuide;
        EndAllGuide += StopGuideUICor;
        EndGuide += EndGuideThis;
        FullSizeGuide += ChangeFullSize;
        CenterSizeGuide += ChangeCenterSize;
    }

    private void ChangeFullSize(RectTransform obj)
    {
        guideUI.rectTransform.anchoredPosition = Vector2.zero;
        guideUI.rectTransform.anchorMin = Vector2.zero;
        guideUI.rectTransform.anchorMax = Vector2.one;
    }

    private void ChangeCenterSize()
    {
        guideUI.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        guideUI.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        guideUI.rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    private void StartGuide(RectTransform rect)
    {
        StopGuideUICor();
        StartCoroutine(GuideSignCor(rect));
    }

    private IEnumerator GuideSignCor(RectTransform rect)
    {
        if(rect == null)
        {
            Debug.Log("rect is null");
            yield break;
        }
      
        guideUI.rectTransform.SetParent(rect);
        guideUI.rectTransform.anchorMin = rect.anchorMin;
        guideUI.rectTransform.anchorMax = rect.anchorMax;
        guideUI.rectTransform.pivot = rect.pivot;
        guideUI.rectTransform.localPosition = Vector2.zero;
        guideUI.rectTransform.sizeDelta = rect.sizeDelta * 1.1f;
        guideUI.gameObject.SetActive(true);

        currentRectTransform = rect;

        isSign = true;

        while (isSign)
        {
            guideUI.DOFade(0.3f, 2f);
            yield return new WaitForSeconds(2f);
            guideUI.DOFade( 0.6f, 2f);
            yield return new WaitForSeconds(2f);
        }
    }

    private void EndGuideThis(RectTransform rect)
    {
        if(currentRectTransform == null)
        {
            return;
        }

        if(rect.root == currentRectTransform.root)
        {
            StopGuideUICor();
        }
        else
        {
        }
        return;
    }


    private void StopGuideUICor()
    {
        currentRectTransform = null;
        guideUI.transform.SetParent(transform);
        isSign = false;
        StopAllCoroutines();
        guideUI.gameObject.SetActive(false);
        guideUI.DOKill();
    }
}
