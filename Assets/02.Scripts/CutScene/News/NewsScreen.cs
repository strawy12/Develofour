using DG.Tweening;
using ExtenstionMethod;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class NewsScreenData
{
    public Sprite screenSprite;
    public Sprite screenTitleSprite;

    public Vector2 targetPosition;
    public Vector3 targetScale;
}

public class NewsScreen : MonoBehaviour
{
    public enum ENewsScreenType
    {
        AIMurder,
        AIRegulation,
        AIRegulationPass
    }
    [SerializeField]
    private Image screenTitleImage;

    [SerializeField]
    private List<NewsScreenData> screenDataList;


    private Image screenImage;
    private CanvasGroup canvasGroup;
    public RectTransform rectTransform { get; private set; }


    private ENewsScreenType currentScreenType;

    public void Init()
    {
        screenImage ??= GetComponent<Image>();
        canvasGroup ??= GetComponent<CanvasGroup>();
        rectTransform ??= GetComponent<RectTransform>();
    }
    public void ChangeScreen(ENewsScreenType type, float fadeTime = 0f)
    {
        StartCoroutine(ChangeScreenCoroutine(type, fadeTime));
    }

    private IEnumerator ChangeScreenCoroutine(ENewsScreenType type, float fadeTime)
    {
        currentScreenType = type;
        NewsScreenData data = screenDataList[(int)currentScreenType];

        if (canvasGroup.alpha != 0f)
        {
            rectTransform.DOScale(0, fadeTime).SetEase(Ease.InSine);
            canvasGroup.DOFade(0f, fadeTime * 0.75f).SetEase(Ease.OutCubic);
            yield return new WaitForSeconds(fadeTime);
            rectTransform.localScale = data.targetScale * 0.8f;
        }

        rectTransform.localPosition = data.targetPosition;
        screenImage.sprite = data.screenSprite;

        if (data.screenTitleSprite == null)
        {
            screenTitleImage.ChangeImageAlpha(0f);
        }

        else
        {
            screenTitleImage.ChangeImageAlpha(1f);
            screenTitleImage.sprite = data.screenTitleSprite;
        }


        if (fadeTime != 0f)
        {
            rectTransform.DOScale(data.targetScale, fadeTime / 3);
            rectTransform.DOShakeAnchorPos(fadeTime / 2f, 25, 50);
            canvasGroup.DOFade(1f, fadeTime).SetEase(Ease.OutSine);
        }
    }

    public void Release()
    {
        canvasGroup.alpha = 0f;
    }
}
