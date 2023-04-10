using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ProfileTutorial : MonoBehaviour
{
    [SerializeField]
    private TutorialTextSO profileTutorialTextData;
    [SerializeField]
    private WindowAlterationSO profileWindowAlteration;
    [SerializeField]
    private float guideDelayWhenEndTuto = 30f;

    void Start()
    {
        EventManager.StartListening(ETutorialEvent.TutorialStart, StartTutorial);
        //EventManager.StartListening(ETutorialEvent.EndClickInfoTutorial, delegate { StartCoroutine(NoticeProfileChattingTutorial()); });
        EventManager.StartListening(ETutorialEvent.EndClickInfoTutorial, StartCompleteProfileTutorial);

        //skip debug 코드
        
        EventManager.StartListening(ELibraryEvent.IconClickOpenFile, FirstOpenUSBFile);
        // 만약 USB 화면 들어가면
    }

    private void StartTutorial(object[] ps)
    {
        StartCoroutine(StartProfileTutorial());
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
            EventManager.StopListening(ELibraryEvent.IconClickOpenFile, FirstOpenUSBFile);
        }
    }

    public void StartChatting(int textListIndex)
    {
        ProfileChattingSystem.OnPlayChatList?.Invoke(profileTutorialTextData.tutorialTexts[textListIndex].data, 1f, true); 
    }

    public IEnumerator StartProfileTutorial()
    {
        yield return new WaitForSeconds(0.5f);
        
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        ProfileChattingSystem.OnChatEnd += StartProfileNextTutorial;
        StartChatting(0);
    }

    public void StartProfileNextTutorial()
    {
        ProfileChattingSystem.OnChatEnd += CheckMaximumWindow;
        StartChatting(1);
        
    }

    private void CheckMaximumWindow()
    {
        ProfileChattingSystem.OnChatEnd -= CheckMaximumWindow;

        DataManager.Inst.SetIsStartTutorial(ETutorialType.Profiler, true);

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

    public void StartCompleteProfileTutorial(object[] ps)
    {
        // 여기다가 전부 꺼주는 작업
        ProfileChattingSystem.OnImmediatelyEndChat?.Invoke();
        StopAllCoroutines();
        // ㅈㄴ 안 좋은 코드 ㅎ
        ProfileChattingSystem.OnChatEnd = null;

        EventManager.StopListening(ETutorialEvent.EndClickInfoTutorial, StartCompleteProfileTutorial);
        GuideUISystem.EndGuide?.Invoke();
        ProfileChattingSystem.OnChatEnd += StartProfileEnd;
        StartChatting(2);
    }

    public void StartProfileEnd()
    {
        EndTutoMonologEvent();
        EventManager.StopListening(ETutorialEvent.TutorialStart, StartTutorial);
    }

    private void EndTutoMonologEvent()
    {
        GameManager.Inst.ChangeGameState(EGameState.Game);
        DataManager.Inst.SetIsClearTutorial(ETutorialType.Profiler, true);
    }
}
