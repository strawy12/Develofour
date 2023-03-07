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
    }

    public IEnumerator StartProfileTutorial()
    {
        WindowManager.Inst.StartTutorialSetting();
        //tutorialPanel.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        EventManager.StartListening(ETutorialEvent.ProfileInfoEnd, delegate { StartCoroutine(StartProfileLastTutorial()); });

        //NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);

        for (int i = 0; i < startAIChatting.Length; i++)
        {
            if (i == 3)
            {
                MonologSystem.OnTutoMonolog(ETextDataType.TutorialMonolog1, 0.1f, 1);
            }
            AIChatting(startAIChatting[i]);

            yield return new WaitForSeconds(1.5f);
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
            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator StartProfileLastTutorial()
    {
        //NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
        yield return new WaitForSeconds(0.5f);
        foreach (string str in completeProfileChatting)
        {
            AIChatting(str);
            yield return new WaitForSeconds(1f);
        }

        MonologSystem.OnEndMonologEvent += EndTutoMonologEvent;
        MonologSystem.OnTutoMonolog(ETextDataType.TutorialMonolog2, 0.2f, 3);

        EventManager.StopListening(ETutorialEvent.TutorialStart, delegate { StartCoroutine(StartProfileTutorial()); });
    }

    private void EndTutoMonologEvent()
    {
        GameManager.Inst.ChangeGameState(EGameState.Game);

        MonologSystem.OnStartMonolog(ETextDataType.TutorialMonolog3, 0f, 3);

        EventManager.TriggerEvent(ECoreEvent.OpenPlayGuide, new object[2] { 5f, EGuideType.BrowserConnectGuide });
        MonologSystem.OnEndMonologEvent -= EndTutoMonologEvent;
    }
}
