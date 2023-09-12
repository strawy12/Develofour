using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinderWindows : MonoBehaviour
{
    [SerializeField]
    private GameObject callAnswerPanel;
    [SerializeField]
    private RectTransform callWindowPanel;

    [SerializeField]
    private Button callAnswerBtn;

    [SerializeField]
    private CanvasGroup canvasGroup;

    private bool isRecieveCall;

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

    private IEnumerator PlayPhoneSoundAndShake()
    {
        yield return new WaitForSeconds(0.8f);
        while (!isRecieveCall)
        {
            callAnswerPanel.transform.DOKill(true);
            callAnswerPanel.transform.DOShakePosition(2.5f, 5);
            float soundSecond = (float)Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneAlarm);
            yield return new WaitForSeconds(soundSecond + Constant.PHONECALLSOUND_DELAY);
        }
        isRecieveCall = false;
    }

    public virtual void SizeDoTween()
    {
        float minDuration = 0.16f;
        callWindowPanel.localScale = new Vector2(0.9f, 0.9f);
        callWindowPanel.gameObject.SetActive(true);

        Sequence sequence = DOTween.Sequence();
        sequence.Join(callWindowPanel.DOScale(1, minDuration));
    }

    private void OnClickBtn()
    {
        isRecieveCall = false;
        callAnswerPanel.SetActive(false);
        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.PhoneAlarm);
        StopAllCoroutines();
        SizeDoTween();

        

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
