using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class GuideUI : MonoBehaviour
{
    private Image currentImage;
    public RectTransform targetRectTrm { get; private set; }
    private bool isSign;

    public Action<GuideUI> OnObjectDestroy;

    public void Show(RectTransform target)
    {
        currentImage ??= GetComponent<Image>();

        if (transform.localScale != Vector3.one)
        {
            transform.localScale = Vector3.one;
        }

        targetRectTrm = target;
        gameObject.SetActive(true);
        StartCoroutine(GuideSignCor(targetRectTrm));
    }

    public void Hide()
    {
        if (this.targetRectTrm == null) return;
        isSign = false;
        targetRectTrm = null;
        StopAllCoroutines();
        gameObject.SetActive(false);
    }

    public void ChangeFullSize()
    {
        currentImage.rectTransform.anchoredPosition = Vector2.zero;
        currentImage.rectTransform.anchorMin = Vector2.zero;
        currentImage.rectTransform.anchorMax = Vector2.one;
    }

    public void ChangeCenterSize()
    {
        currentImage.rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        currentImage.rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        currentImage.rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }

    public IEnumerator GuideSignCor(RectTransform rect)
    {
        if (rect == null)
        {
            Debug.Log("rect is null");
            yield break;
        }

        currentImage.rectTransform.SetParent(rect);
        currentImage.rectTransform.anchorMin = rect.anchorMin;
        currentImage.rectTransform.anchorMax = rect.anchorMax;
        currentImage.rectTransform.pivot = rect.pivot;
        currentImage.rectTransform.localPosition = Vector2.zero;
        currentImage.rectTransform.sizeDelta = rect.sizeDelta * 1.1f;
        currentImage.gameObject.SetActive(true);

        targetRectTrm = rect;
        isSign = true;

        while (isSign)
        {
            currentImage.DOFade(0.3f, 2f);
            yield return new WaitForSeconds(2f);
            currentImage.DOFade(0.6f, 2f);
            yield return new WaitForSeconds(2f);
        }
    }

    private void OnDestroy()
    {
        OnObjectDestroy?.Invoke(this);
    }
}