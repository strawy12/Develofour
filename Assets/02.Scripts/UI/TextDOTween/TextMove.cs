using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using UnityEditor.Experimental.GraphView;

public class TextMove : MonoBehaviour
{
    private bool isShaking;

    private Vector3 originPos;
    
    [SerializeField]
    private float moveDuration;
    [SerializeField]
    private float colorDuration;

    [SerializeField]
    private Vector3 selectPos;
    [SerializeField]
    private Vector3 selectScale;

    [SerializeField]
    private TMP_Text gamilText;

    private RectTransform rectTransform;

    [Header("Skaking Data")]
    [SerializeField]
    private int strength;
    [SerializeField]
    private int vibrato;
    [SerializeField]
    private float shakeDuration;
    [SerializeField]
    private Color shakingColor;


    void Awake()
    {
        Init();
    }

    private void Init()
    {
        isShaking = false;

        rectTransform = GetComponent<RectTransform>();

        originPos = rectTransform.anchoredPosition;
    }

    public void PlaceholderEffect(bool isSelect)
    {
        Sequence sequence = DOTween.Sequence();

        Vector3 targetPos = isSelect ? selectPos : originPos;
        Vector3 targetScale = isSelect ? selectScale : Vector3.one;

        sequence.Append(rectTransform.DOScale(targetScale, moveDuration));
        sequence.Join(rectTransform.DOAnchorPosY(targetPos.y, moveDuration));
    }

    public void FaliedInput()
    {
        if(isShaking)
        {
            return;
        }
        isShaking = true;
        
        gamilText.text = "다시 입력하세요.";

        Sequence sequence = DOTween.Sequence();

        sequence.Append(gamilText.DOColor(shakingColor, colorDuration));
        sequence.Join(gamilText.transform.DOShakePosition(shakeDuration, strength, vibrato));

        sequence.AppendCallback(() => isShaking = false);
    }
}
