using DG.Tweening;
using Mono.Cecil;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class NewsAnchor : NewsCharacter
{
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
    }

    public override void EndSpeak()
    {
        if (!isSpeak) return;
        isSpeak = false;
    }

}
