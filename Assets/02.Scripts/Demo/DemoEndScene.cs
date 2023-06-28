using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;
using TMPro;

public class DemoEndScene : MonoBehaviour
{

    [SerializeField]
    private float duration = 1.5f;
    [SerializeField]
    private float endDuration = 0.8f;
    [SerializeField]
    private Button exitBtn;
    [SerializeField]
    private Image quitPanel;

    [SerializeField]
    private TMP_Text text;


    public void Open()
    {
        exitBtn.gameObject.SetActive(false);
        gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(text.DOFade(1f, duration));
        sequence.AppendCallback(() => EnableExitButton());

    }

    private void EnableExitButton()
    {
        exitBtn.gameObject.SetActive(true);
        exitBtn.onClick.AddListener(BlackImage);
    }

    private void BlackImage()
    {

        exitBtn.gameObject.SetActive(false);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(quitPanel.DOFade(1f, endDuration));
        sequence.AppendCallback(() => { GameManager.Inst.GameQuit(); });
    }
}
