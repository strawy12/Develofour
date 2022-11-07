using DG.Tweening;
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
        if (fadeTime != 0f)
        {
            screenImage.DOFade(0f, 0f);
        }

        currentScreenType = type;
        screenImage.sprite = screenSpriteList[(int)currentScreenType];

        if (fadeTime != 0f)
        {
            screenImage.DOFade(1f, fadeTime);
        }
    }
}
