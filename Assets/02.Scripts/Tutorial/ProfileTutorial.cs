using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ProfileTutorial : MonoBehaviour
{
    [SerializeField]
    private WindowAlterationSO profileWindowAlteration;
    void Start()
    {
        EventManager.StartListening(ETutorialEvent.TutorialStart, delegate { StartCoroutine(StartProfileTutorial()); });
        //EventManager.StartListening(ETutorialEvent.EndClickInfoTutorial, delegate { StartCoroutine(NoticeProfileChattingTutorial()); });
        EventManager.StartListening(ETutorialEvent.EndClickInfoTutorial, delegate { StartCompleteProfileTutorial(); });

        //skip debug 코드
        
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

        if(fileData.fileName == Constant.USB_FILENAME)
        {
            MonologSystem.OnStartMonolog.Invoke(EMonologTextDataType.OnUSBFileMonoLog, 1f, 3);
            EventManager.StopListening(ELibraryEvent.IconClickOpenFile, FirstOpenUSBFile);
        }

    }

    public void StartChatting(EAIChattingTextDataType textDataType)
    {
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[] { textDataType });
    }

    public IEnumerator StartProfileTutorial()
    {
        Debug.Log("프로파일러 튜토리얼 시작");
        DataManager.Inst.SaveData.isTutorialStart = true;
        yield return new WaitForSeconds(0.5f);
        
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);

        ProfileChattingSystem.OnChatEnd += StartProfileMonolog;
        StartChatting(EAIChattingTextDataType.StartAIChatting);
    }


    public void StartProfileMonolog()
    {
        MonologSystem.OnEndMonologEvent += StartProfileNextTutorial;
        MonologSystem.OnTutoMonolog(EMonologTextDataType.TutorialMonolog1, 0.1f, 1);
    }
    public void StartProfileNextTutorial()
    {
        MonologSystem.OnEndMonologEvent -= StartProfileNextTutorial;
        ProfileChattingSystem.OnChatEnd += CheckMaximumWindow;
        StartChatting(EAIChattingTextDataType.StartNextAiChatting);
    }

    private void CheckMaximumWindow()
    {
        ProfileChattingSystem.OnChatEnd -= CheckMaximumWindow;

        if (profileWindowAlteration.isMaximum)
        {
            EventManager.TriggerEvent(ETutorialEvent.ProfileMidiumStart);
        }
        else
        {
            BackgroundNoticeTutorial();
        }
    }

    public void BackgroundNoticeTutorial()
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookBackground, 2f);
        EventManager.TriggerEvent(ETutorialEvent.BackgroundSignStart);

    }

    public void StartCompleteProfileTutorial()
    {
        EventManager.StopListening(ETutorialEvent.EndClickInfoTutorial, delegate { StartCompleteProfileTutorial(); });
        ProfileChattingSystem.OnChatEnd += StartProfileEnd;
        StartChatting(EAIChattingTextDataType.CompleteProfileAIChatting);
    }


    private void AIChatting(string str)
    {
        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[1] { str });
    }


    public void StartProfileEnd()
    {
        ProfileChattingSystem.OnChatEnd -= StartProfileEnd;
        GuideUISystem.EndGuide?.Invoke();
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
