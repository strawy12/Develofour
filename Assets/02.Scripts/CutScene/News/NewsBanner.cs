using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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
        Sequence seq = DOTween.Sequence();

        seq.Append(rectTransform.DOAnchorPos(endTextPos, newsBannerSpeed));

        seq.AppendCallback(() =>
        {
            rectTransform.anchoredPosition = startTextPos;
        });

        yield return new WaitForSeconds(newsBannerDelay);
    }

    public void BannelStop()
    {
        StopCoroutine(MoveNewsBanner());
    }
}
