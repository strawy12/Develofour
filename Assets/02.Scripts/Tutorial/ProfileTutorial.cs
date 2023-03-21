using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ProfileTutorial : MonoBehaviour
{
    public string[] startAIChatting;
    public string[] findNoticeAIChatting;
    public string[] completeProfileChatting;

    public float delay = 2f;
    public Action OnChatEnd;

    void Start()
    {
        EventManager.StartListening(ETutorialEvent.TutorialStart, delegate { StartCoroutine(StartProfileTutorial()); });
        //EventManager.StartListening(ETutorialEvent.EndClickInfoTutorial, delegate { StartCoroutine(NoticeProfileChattingTutorial()); });
        EventManager.StartListening(ETutorialEvent.EndClickInfoTutorial, delegate { Debug.Log("왜안됨"); StartCompleteProfileTutorial(); });

        //skip debug 코드
        EventManager.StartListening(EDebugSkipEvent.TutorialSkip, delegate { delay = 0.05f; });
    }

    public IEnumerator StartChatting(EAIChattingTextDataType textDataType)
    {
        AIChattingTextDataSO data = ResourceManager.Inst.GetAIChattingTextDataSO(textDataType);

        for(int i = 0; i < data.Count; i++)
        {
            //yield return new WaitUntil(() => )
            AIChatting(data[i].text);
            yield return new WaitForSeconds(delay);
        }

        OnChatEnd?.Invoke();
    }

    public IEnumerator StartProfileTutorial()
    {
        Debug.Log("프로파일러 튜토리얼 시작");
        DataManager.Inst.SaveData.isTutorialStart = true;
        WindowManager.Inst.StartTutorialSetting();
        yield return new WaitForSeconds(0.5f);
        
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);

        OnChatEnd += StartProfileMonolog;
        StartCoroutine(StartChatting(EAIChattingTextDataType.StartAIChatting));
    }

    public void StartProfileMonolog()
    {
        MonologSystem.OnEndMonologEvent += StartProfileNextTutorial;
        MonologSystem.OnStartMonolog(EMonologTextDataType.TutorialMonolog1, 0.1f, 1);
    }

    public void StartProfileNextTutorial()
    {
        MonologSystem.OnEndMonologEvent -= StartProfileNextTutorial;
        OnChatEnd += BackgroundNoticeTutorial;
        StartCoroutine(StartChatting(EAIChattingTextDataType.StartNextAiChatting));
    }

    public void BackgroundNoticeTutorial()
    {
        OnChatEnd -= BackgroundNoticeTutorial;
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookBackground, 3f);

        EventManager.TriggerEvent(ETutorialEvent.BackgroundSignStart);
    }


    public void StartCompleteProfileTutorial()
    {
        OnChatEnd += StartProfileEnd;
        StartCoroutine(StartChatting(EAIChattingTextDataType.CompleteProfileAIChatting));
    }


    private void AIChatting(string str)
    {
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { str });
    }


    public void StartProfileEnd()
    {
        OnChatEnd -= StartProfileEnd;
        EventManager.TriggerEvent(ETutorialEvent.ProfileEventStop);
        EndTutoMonologEvent();
        EventManager.StopListening(ETutorialEvent.TutorialStart, delegate { StartCoroutine(StartProfileTutorial()); });
    }

    private void EndTutoMonologEvent()
    {
        GameManager.Inst.ChangeGameState(EGameState.Game);
        GameManager.Inst.isTutorial = false;
        MonologSystem.OnEndMonologEvent -= EndTutoMonologEvent;
    }
}
