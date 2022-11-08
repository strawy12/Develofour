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

    private void Start()
    {
        screenImage.sprite = null;
    }

    public void ChangeScreen(ENewsScreenType type, float fadeTime = 0f)
    {
        Sequence seq = DOTween.Sequence();
        if (screenImage.sprite != null)
        {
            seq.Append(screenImage.DOFade(0f, fadeTime));
            

        }

        seq.AppendCallback(() =>
        {
            currentScreenType = type;
            screenImage.sprite = screenSpriteList[(int)currentScreenType];
        });
        

        if (fadeTime != 0f)
        {
            seq.Append(screenImage.DOFade(1f, fadeTime));
        }
        seq.Play();
    }
}
