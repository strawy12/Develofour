using Cinemachine;
using DG.Tweening;
using ExtenstionMethod;
using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NewsTitle : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup titleCanvasGroup;
    [SerializeField]
    private RectTransform titleRectTransform;
    [SerializeField]
    private float maxTitleWidth;
    [SerializeField]
    private Image logoImage;
    [SerializeField]
    private TMP_Text titleText;

    [SerializeField]
    private float logoFadeDuration;

    [SerializeField]
    private float titleDuration;


    public void Show(bool useFadeEffect, string message = "")
    {
        Sequence sequence = DOTween.Sequence();

        SetTitleText(message, useFadeEffect);
        sequence.Append(LogoEffect(true));
        sequence.Append(TitleEffect(true));
        sequence.Join(TitleWidthEffect(true));
    }

    public void Hide(bool isHideLogo)
    {
        Sequence sequence = DOTween.Sequence();

        sequence.Append(LogoEffect(!isHideLogo));
        sequence.Append(TitleEffect(false));
        sequence.Join(TitleWidthEffect(false));
    }

    public void SetTitleText(string message, bool useFadeEffect)
    {
        if (string.IsNullOrEmpty(message)) { return; }

        if (useFadeEffect)
        {
            Sequence seq = DOTween.Sequence();
            seq.Append(titleText.DOFade(0f, 1f));
            seq.AppendCallback(() => titleText.SetText(message));
            seq.Append(titleText.DOFade(1f, 0.75f));
        }

        else
        {
            titleText.SetText(message);
        }

    }

    private Tween LogoEffect(bool isOpen)
    {
        return logoImage.DOFade(isOpen ? 1f : 0f, logoFadeDuration);
    }

    private Tween TitleEffect(bool isOpen)
    {

        return titleCanvasGroup.DOFade(isOpen ? 1f : 0f, titleDuration);
    }

    private Tween TitleWidthEffect(bool isOpen)
    {
        return DOTween.To(
            () => titleRectTransform.sizeDelta.x,
            (value) =>
            {
                titleRectTransform.sizeDelta = titleRectTransform.sizeDelta.ChangeValue(x: value);
            },
            isOpen ? maxTitleWidth : 0f, titleDuration
            );
    }

}

