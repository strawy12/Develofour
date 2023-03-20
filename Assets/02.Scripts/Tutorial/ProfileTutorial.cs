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

    public float delay = 2f;

    void Start()
    {
        EventManager.StartListening(ETutorialEvent.TutorialStart, delegate { StartCoroutine(StartProfileTutorial()); });
        EventManager.StartListening(ETutorialEvent.EndClickInfoTutorial, delegate { StartCoroutine(NoticeProfileChattingTutorial()); });

        //skip debug 코드
        EventManager.StartListening(EDebugSkipEvent.TutorialSkip, delegate { delay = 0.05f; });
        EventManager.StartListening(ELibraryEvent.IconClickOpenFile, FirstOpenUSBFile);
        // 만약 USB 화면 들어가면
    }

    public void FirstOpenUSBFile(object[] ps)
    {
        if (ps[0] == null) 
        {
            return;   
        }

        FileSO fileData = (FileSO)ps[0];


        if(fileData.fileName == "BestUSB")
        {
            MonologSystem.OnStartMonolog.Invoke(ETextDataType.OnUSBFileMonoLog, 1f, 3);
        }
        
        EventManager.StopListening(ELibraryEvent.IconClickOpenFile, FirstOpenUSBFile);
    }

    public IEnumerator StartProfileTutorial()
    {
        Debug.Log("프로파일러 튜토리얼 시작");

        WindowManager.Inst.StartTutorialSetting();
        yield return new WaitForSeconds(0.5f);
        
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        EventManager.StartListening(ETutorialEvent.ProfileInfoEnd, delegate { StartCoroutine(StartProfileLastTutorial()); });

        for (int i = 0; i < 3; i++)
        {
            AIChatting(startAIChatting[i]);
            if (i == 2)
            {
                MonologSystem.OnEndMonologEvent += StartContinueProfileTutorial;
                MonologSystem.OnTutoMonolog(ETextDataType.TutorialMonolog1, 0.1f, 1);
            }

            yield return new WaitForSeconds(delay);
        }
    }

    private void StartContinueProfileTutorial()
    {
        StartCoroutine(ContinueProfileAiChatting());
    }

    public IEnumerator ContinueProfileAiChatting()
    {
        MonologSystem.OnEndMonologEvent -= StartContinueProfileTutorial;
        for (int i = 3; i < startAIChatting.Length; i++)
        {
            AIChatting(startAIChatting[i]);

            yield return new WaitForSeconds(delay);
        }

        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookBackground, 0.1f);

        EventManager.TriggerEvent(ETutorialEvent.BackgroundSignStart);
    }

    private void AIChatting(string str)
    {
        EventManager.TriggerEvent(EProfileEvent.SaveMessage, new object[1] { str });
    }

    public IEnumerator NoticeProfileChattingTutorial()
    {
        EventManager.StopAllListening(ETutorialEvent.LibraryRootCheck);
        //NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
        EventManager.TriggerEvent(ETutorialEvent.ProfileInfoStart);
        foreach (string str in findNoticeAIChatting)
        {
            AIChatting(str);
            yield return new WaitForSeconds(delay);
        }
        yield return new WaitForSeconds(1.5f);
        StartCoroutine(StartProfileLastTutorial());
    }

    public IEnumerator StartProfileLastTutorial()
    {
        //NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.AiMessageAlarm, 0f);
        foreach (string str in completeProfileChatting)
        {
            AIChatting(str);
            yield return new WaitForSeconds(delay);
        }

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
