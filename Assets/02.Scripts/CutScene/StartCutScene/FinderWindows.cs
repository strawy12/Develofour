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
        callAnswerPanel.SetCharacter(ECharacterType.Assistant);
        callAnswerPanel.Show();
        callAnswerPanel.OnClick += OnClickBtn;

        callAnswerPanel.PlayCall();
    }


    private void OnClickBtn()
    {
        callAnswerPanel.Hide();
        callWindowPanel.Show();

        callWindowPanel.AddChacter(ECharacterType.Assistant, TalkAssistant_1);

        //EndScene();
    }

    private void TalkAssistant_1()
    {
        string monologID = $"{Constant.MonologKey.STARTCUTSCENE_3}_{1}";
        MonologSystem.AddOnEndMonologEvent(monologID, CallPolice);
        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
    }


    private void CallPolice()
    {
        callAnswerPanel.SetCharacter(ECharacterType.Police);
        callAnswerPanel.Show();
        callAnswerPanel.OnClick += AddPolice;

        callAnswerPanel.PlayCall();

        string monologID = $"{Constant.MonologKey.STARTCUTSCENE_3}_{2}";
        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
    }

    private void AddPolice()
    {
        callAnswerPanel.Hide();
        callWindowPanel.AddChacter(ECharacterType.Police, TalkPolice);
    }

    private void TalkPolice()
    {
        string monologID = Constant.MonologKey.STARTCUTSCENE_4;
        MonologSystem.AddOnEndMonologEvent(monologID, OutGoingAssistant);
        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
    }

    private void OutGoingAssistant()
    {
        callWindowPanel.AddChacter(ECharacterType.Assistant, TalkAssistant_2);
    }

    private void TalkAssistant_2()
    {
        string monologID = Constant.MonologKey.STARTCUTSCENE_5;
        MonologSystem.AddOnEndMonologEvent(monologID, EndScene);
        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
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
