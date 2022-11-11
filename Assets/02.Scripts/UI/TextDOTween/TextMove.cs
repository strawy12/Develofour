using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class TextMove : MonoBehaviour
{
    private Vector3 originPos;
    
    [SerializeField]
    private float duration;

    [SerializeField]
    private Vector3 selectPos;
    [SerializeField]
    private Vector3 selectScale;

    private RectTransform rectTransform;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        rectTransform = GetComponent<RectTransform>();

        originPos = rectTransform.anchoredPosition;
    }

    public void PlaceholderEffect(bool isSelect)
    {
        Sequence sequence = DOTween.Sequence();

        Vector3 targetPos = isSelect ? selectPos : originPos;
        Vector3 targetScale = isSelect ? selectScale : Vector3.one;

        sequence.Append(rectTransform.DOScale(targetScale, duration));
        sequence.Join(rectTransform.DOAnchorPosY(targetPos.y, duration));
    }
}
