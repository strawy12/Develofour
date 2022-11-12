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
    private bool haveSprite;

    private void Awake()
    {
        screenImage = GetComponent<Image>();
        haveSprite = false;
    }

    public void ChangeScreen(ENewsScreenType type, float fadeTime = 0f)
    {
        StartCoroutine(ChangeScreenCoroutine(type, fadeTime));
    }

    private IEnumerator ChangeScreenCoroutine(ENewsScreenType type, float fadeTime)
    {
        if (haveSprite)
        {
            screenImage.rectTransform.DOShakeAnchorPos(fadeTime / 2f);
            screenImage.DOFade(0f, fadeTime).SetEase(Ease.InBounce);
            yield return new WaitForSeconds(fadeTime);
        }

        currentScreenType = type;
        screenImage.sprite = screenSpriteList[(int)currentScreenType];

        haveSprite = true;

        if (fadeTime != 0f)
        {
            screenImage.rectTransform.DOShakeAnchorPos(fadeTime / 2f);
            screenImage.DOFade(1f, fadeTime).SetEase(Ease.OutBounce);
        }
    }

    public void Release()
    {
        Color color = screenImage.color;
        color.a = 0f;
        screenImage.color = color;
        haveSprite = false;
    }
}
