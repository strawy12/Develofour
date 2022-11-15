using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewsScreen : MonoBehaviour
{
    public enum ENewsScreenType
    {
        AIMurder,
        AIRegulation
    }

    [SerializeField]
    private List<Sprite> screenSpriteList;

    private Image screenImage;

    private ENewsScreenType currentScreenType;

    private void Awake()
    {
        screenImage = GetComponent<Image>();
    }

    public void ChangeScreen(ENewsScreenType type, float fadeTime = 0f)
    {
        StartCoroutine(ChangeScreenCoroutine(type, fadeTime));
    }

    private IEnumerator ChangeScreenCoroutine(ENewsScreenType type, float fadeTime)
    {
        if (screenImage.color.a != 0f)
        {
            screenImage.rectTransform.DOScale(0, fadeTime).SetEase(Ease.InSine);
            screenImage.DOFade(0f, fadeTime * 0.75f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(fadeTime);
            screenImage.rectTransform.DOScale(0.8f, 0);
        }

        currentScreenType = type;
        screenImage.sprite = screenSpriteList[(int)currentScreenType];

        if (fadeTime != 0f)
        {
            screenImage.rectTransform.DOScale(1, fadeTime / 3);
            screenImage.rectTransform.DOShakeAnchorPos(fadeTime / 2f, 25, 50);
            screenImage.DOFade(1f, fadeTime).SetEase(Ease.OutSine);
        }
    }

    public void Release()
    {
        Color color = screenImage.color;
        color.a = 0f;
        screenImage.color = color;
    }
}
