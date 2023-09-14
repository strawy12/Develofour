using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinderWindows : MonoBehaviour
{
    [SerializeField]
    private FinderCallAnswerUI callAnswerPanel;
    [SerializeField]
    private FinderCallWindow callWindowPanel;



    [SerializeField]
    private CanvasGroup canvasGroup;



    public Action OnCompleted;

    public void Show(float duration)
    {
        gameObject.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, duration);
    }

    public void StartScene()
    {
        callAnswerPanel.SetActive(true);
        StartCoroutine(PlayPhoneSoundAndShake());

        callAnswerBtn.onClick.AddListener(OnClickBtn);
    }


    private void OnClickBtn()
    {
        isRecieveCall = false;
        callAnswerPanel.SetActive(false);
        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.PhoneAlarm);
        StopAllCoroutines();
        callWindowPanel.SizeDoTween();

        

        //EndScene();
    }

    private void EndScene()
    {
        OnCompleted?.Invoke();
        OnCompleted = null;
    }

    public void Hide(float duration)
    {
        canvasGroup.DOFade(0f, duration).OnComplete(() => gameObject.SetActive(false));
    }
}
