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
        ProfileChattingSystem.OnChatEnd += StartProfileMonolog;
        StartChatting(0);
    }


    public void StartProfileMonolog()
    {
        StartProfileNextTutorial();
        //MonologSystem.OnStartMonolog(EMonologTextDataType.TutorialMonolog1, 0.1f, true);
    }
    public void StartProfileNextTutorial()
    {
        MonologSystem.OnEndMonologEvent -= StartProfileNextTutorial;
        ProfileChattingSystem.OnChatEnd += CheckMaximumWindow;
        StartChatting(2);
        
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

    public void StartCompleteProfileTutorial()
    {
        EventManager.StopListening(ETutorialEvent.EndClickInfoTutorial, delegate { StartCompleteProfileTutorial(); });
        GuideUISystem.EndGuide?.Invoke();
        ProfileChattingSystem.OnChatEnd += StartProfileEnd;
        StartChatting(2);
    }

    public void StartProfileEnd()
    {
        EndTutoMonologEvent();
        EventManager.StopListening(ETutorialEvent.TutorialStart, delegate { StartCoroutine(StartProfileTutorial()); });
    }

    private void EndTutoMonologEvent()
    {
        GameManager.Inst.ChangeGameState(EGameState.Game);
        DataManager.Inst.SetIsClearTutorial(ETutorialType.Profiler, true);
        EventManager.TriggerEvent(EGuideButtonTutorialEvent.TutorialStart);
    }
}
