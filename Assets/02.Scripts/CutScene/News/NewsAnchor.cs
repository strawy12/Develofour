using DG.Tweening;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NewsAnchor : NewsCharacter
{
    [SerializeField]
    private float speakDelay = 0.2f;

    [SerializeField]
    private Vector2 speakOffset;

    [SerializeField]
    private Sprite faceSprite;

    [SerializeField]
    private Sprite speakFaceSprite;

    [SerializeField]
    private Image faceImage;


    public RectTransform rectTransform { get; private set; }
    public CanvasGroup canvasGroup { get; private set; }


    private bool isSpeak;

    public void Init()
    {
        canvasGroup ??= GetComponent<CanvasGroup>();
        rectTransform ??= GetComponent<RectTransform>();

        canvasGroup.alpha = 0f;
    }

    public override void StartSpeak()
    {
        if (isSpeak) return;
        isSpeak = true;

       

        StartCoroutine(SpeakCoroutine());
    }

    public override void EndSpeak()
    {
        if (!isSpeak) return;
        isSpeak = false;
    }

    private IEnumerator SpeakCoroutine()
    {
        while (isSpeak)
        {
            faceImage.sprite = faceSprite;
            faceImage.rectTransform.anchoredPosition += speakOffset;
            yield return new WaitForSeconds(speakDelay);
            faceImage.sprite = speakFaceSprite;
            faceImage.rectTransform.anchoredPosition -= speakOffset;
            yield return new WaitForSeconds(speakDelay);
        }

        faceImage.sprite = faceSprite;
    }
}
