using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ExtenstionMethod;
using TMPro;

public class NewsBanner : MonoBehaviour
{
    [SerializeField]
    TMP_Text bannerText;

    private bool isBannerPlay = false;

    [SerializeField]
    private float newsBannerSpeed;
    [SerializeField]
    private float turnOnDuration;

    private Vector2 startTextPos;
    private Vector2 endTextPos;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public void Init()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();


    }

    public void SetText(string msg)
    {
        bannerText.SetText(msg);

        startTextPos = rectTransform.anchoredPosition;
        startTextPos.x = rectTransform.rect.width * 0.5f + bannerText.rectTransform.sizeDelta.x * 0.5f;

        endTextPos = rectTransform.anchoredPosition;
        endTextPos.x = -(rectTransform.rect.width * 0.5f + bannerText.rectTransform.sizeDelta.x * 0.5f);
    }

    public void StartBanner(string msg)
    {
        SetText(msg);
        bannerText.rectTransform.anchoredPosition = startTextPos;

        isBannerPlay = true;
        StartCoroutine(MoveNewsBanner());
    }

    // 이거 호출하면 배너 시작됨
    IEnumerator MoveNewsBanner()
    {
        canvasGroup.DOFade(1f, turnOnDuration);

        while (isBannerPlay)
        {
            bannerText.rectTransform.anchoredPosition 
                = bannerText.rectTransform.anchoredPosition.Calculation(EOperator.Subtraction, x:Time.deltaTime * newsBannerSpeed);

            if(bannerText.rectTransform.anchoredPosition.x <= endTextPos.x)
            {
                bannerText.rectTransform.anchoredPosition = startTextPos;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void BannelStop()
    {
        isBannerPlay = false;
        canvasGroup.DOKill();
        canvasGroup.alpha = 0f;

        StopAllCoroutines();
    }
}
