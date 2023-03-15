using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ProfileTutorial : MonoBehaviour
{
    public string[] startAIChatting;
    public string[] findNoticeAIChatting;
    public string[] completeProfileChatting;


    void Start()
    {
        EventManager.StartListening(ETutorialEvent.TutorialStart, delegate { StartCoroutine(StartProfileTutorial()); });
        EventManager.StartListening(ETutorialEvent.EndClickInfoTutorial, delegate { StartCoroutine(NoticeProfileChattingTutorial()); });

        //skip debug 코드
        EventManager.StartListening(EDebugSkipEvent.TutorialSkip, delegate { StopAllCoroutines(); EndTutoMonologEvent(); });
    }

    public IEnumerator StartProfileTutorial()
    {
        Debug.Log("프로파일러 튜토리얼 시작");

        WindowManager.Inst.StartTutorialSetting();
        //tutorialPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        EventManager.StartListening(ETutorialEvent.ProfileInfoEnd, delegate { StartCoroutine(StartProfileLastTutorial()); });

        //NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);

        for (int i = 0; i < 3; i++)
        {
            AIChatting(startAIChatting[i]);
            if (i == 2)
            {
                MonologSystem.OnEndMonologEvent += StartContinueProfileTutorial;
                MonologSystem.OnTutoMonolog(ETextDataType.TutorialMonolog1, 0.1f, 1);
            }

            yield return new WaitForSeconds(2f);
        }
    }

    public void StartContinueProfileTutorial()
    {
        StartCoroutine(ContinueProfileTutorial());
    }

    public IEnumerator ContinueProfileTutorial()
    {
        MonologSystem.OnEndMonologEvent -= StartContinueProfileTutorial;
        for (int i = 3; i < startAIChatting.Length; i++)
        {
            AIChatting(startAIChatting[i]);

            yield return new WaitForSeconds(2f);
        }

        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookBackground, 0.1f);

        EventManager.TriggerEvent(ETutorialEvent.BackgroundSignStart);
    }

    private void AIChatting(string str)
    {
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { str });
    }

    public IEnumerator NoticeProfileChattingTutorial()
    {
        EventManager.StopAllListening(ETutorialEvent.LibraryRootCheck);
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
        EventManager.TriggerEvent(ETutorialEvent.ProfileInfoStart);
        foreach (string str in findNoticeAIChatting)
        {
            AIChatting(str);
            yield return new WaitForSeconds(2f);
        }
    }

    public IEnumerator StartProfileLastTutorial()
    {
        //NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
        yield return new WaitForSeconds(0.5f);
        foreach (string str in completeProfileChatting)
        {
            AIChatting(str);
            yield return new WaitForSeconds(2f);
        }

        MonologSystem.OnEndMonologEvent += EndTutoMonologEvent;
        MonologSystem.OnTutoMonolog(ETextDataType.TutorialMonolog2, 0.2f, 3);

        EventManager.StopListening(ETutorialEvent.TutorialStart, delegate { StartCoroutine(StartProfileTutorial()); });
    }

    private void EndTutoMonologEvent()
    {
        GameManager.Inst.ChangeGameState(EGameState.Game);
        GameManager.Inst.isTutorial = false;

        EventManager.TriggerEvent(EGuideEventType.OpenPlayGuide, new object[2] { 90f, EGuideTopicName.BrowserConnectGuide });
        MonologSystem.OnEndMonologEvent -= EndTutoMonologEvent;
    }


}
