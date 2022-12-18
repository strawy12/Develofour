using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;



public class AnimationObject : MonoBehaviour
{
    [SerializeField]
    private TMP_Text text;
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private CanvasGroup canvasGroup;

    [Header("Text")]
    [SerializeField]
    private float textDelay = 0.1f;
    [SerializeField]
    private Ease textEase = Ease.Linear;

    [Header("ImageFade")]
    [SerializeField]
    private float fadeDuration = 1f;
    [SerializeField]
    private Ease fadeEase = Ease.InCirc;

    [Space(20)]
    [SerializeField]
    private bool firstText = true;
    [SerializeField]
    private float delay = 0f;
    public float Duration => text.text.Length * textDelay + fadeDuration;

    private bool isInit = false;

    [ContextMenu("Show")]
    public void Show()
    {
        StartCoroutine(ShowAnimation());
    }
    public void Hide()
    {
        canvasGroup.alpha = 0f;
    }

    public float ShowText()
    {
        ShowInit();

        float textDuration = text.text.Length * textDelay;
        text.DOVisible(textDuration).SetEase(textEase);
        return textDuration;
    }

    public float ShowImage()
    {
        ShowInit();

        iconImage.DOFade(1f, fadeDuration).SetEase(fadeEase);
        return fadeDuration;
    }

    private void ShowInit()
    {
        if(isInit) { return; }
        isInit = true;

        if (text.maxVisibleCharacters != 0)
        {
            text.maxVisibleCharacters = 0;
        }

        if (canvasGroup.alpha == 0f)
        {
            canvasGroup.alpha = 1f;
        }
    }
    public IEnumerator ShowAnimation()
    {
        ShowInit();

        if (firstText)
        {
            yield return new WaitForSeconds(ShowText());

            yield return new WaitForSeconds(delay);

            yield return new WaitForSeconds(ShowImage());
        }
        else
        {
            yield return new WaitForSeconds(ShowImage());

            yield return new WaitForSeconds(delay);

            yield return new WaitForSeconds(ShowText());
        }
    }


}
