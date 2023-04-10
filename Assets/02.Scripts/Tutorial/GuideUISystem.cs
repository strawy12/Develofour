using DG.Tweening;
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

    private RectTransform currentRectTransform;

    private void Start()
    {
        guideUI.gameObject.SetActive(false);

        OnGuide += StartGuide;
        EndAllGuide += StopGuideUICor;
        EndGuide += EndGuideThis;
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
        guideUI.rectTransform.sizeDelta = rect.sizeDelta;
        guideUI.gameObject.SetActive(true);

        isSign = true;

        while (isSign)
        {
            guideUI.DOColor(new Color(255, 255, 255, 0.5f), 2f);
            yield return new WaitForSeconds(2f);
            guideUI.DOColor(new Color(255, 255, 255, 1), 2f);
            yield return new WaitForSeconds(2f);
        }
    }

    private void EndGuideThis(RectTransform rect)
    {
        if(rect == currentRectTransform)
        {
            StopGuideUICor();
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
