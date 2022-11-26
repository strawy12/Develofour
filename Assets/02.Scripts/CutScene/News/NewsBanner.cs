using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using ExtenstionMethod;

public class NewsBanner : MonoBehaviour
{
    public bool isBannerPlay = false;

    public float newsBannerSpeed;
    public float newsBannerDelay;

    public Vector2 startTextPos;
    public Vector2 endTextPos;

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void Init()
    {
        isBannerPlay = true;
        rectTransform.anchoredPosition = startTextPos;

        StartCoroutine(MoveNewsBanner());
    }

    // 이거 호출하면 배너 시작됨
    IEnumerator MoveNewsBanner()
    {
        float time = Time.time;

        while(true)
        {
            rectTransform.anchoredPosition 
                = rectTransform.anchoredPosition.Calculation(EOperator.Subtraction, x:Time.deltaTime * newsBannerSpeed);

            if(rectTransform.anchoredPosition.x <= endTextPos.x)
            {
                rectTransform.anchoredPosition = startTextPos;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void BannelStop()
    {
        StopCoroutine(MoveNewsBanner());
    }
}
