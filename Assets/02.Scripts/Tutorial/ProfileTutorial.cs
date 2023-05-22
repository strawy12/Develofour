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
    private float findNameGuideDelay = 60f;
    [SerializeField]
    private FileSO profiler;
    [SerializeField]
    private RectTransform libraryRect;

    public static EGuideObject guideObjectName;

    [SerializeField]
    private int targetIncidentID = 76;

    [SerializeField]
    private int targetCharID1 = 1;
    [SerializeField]
    private int targetCharID2 = 11;

    private Library library;

    void Start()
    {
        EventManager.StartListening(ETutorialEvent.TutorialStart, CreatePopUp);
    }

    private void CreatePopUp(object[] ps)
    {
#if UNITY_EDITOR
        WindowManager.Inst.PopupOpen(profiler, profileTutorialTextData.popText, () => StartTutorial(null), () => StartCompleteProfileTutorial());
#else
        WindowManager.Inst.PopupOpen(profiler, profileTutorialTextData.popText, () => StartTutorial(null), null);
#endif
    }

    private void StartTutorial(object[] ps)
    {
        StartCoroutine(StartProfileTutorial());
    }

    public void StartChatting(int textListIndex)
    {
        ProfileChattingSystem.OnChatEnd += () => DataManager.Inst.SetProfilerTutorialIdx();
        ProfileChattingSystem.OnPlayChatList?.Invoke(profileTutorialTextData.tutorialTexts[textListIndex].data, 1.5f, true);
    }

    private IEnumerator StartProfileTutorial()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        ProfileChattingSystem.OnChatEnd += StartTutorialSetting;

        GetIncidentInfoEvent();
        EventManager.StartListening(ETutorialEvent.SelectLibrary, OpenLibrary);
        StartChatting(0);
    }

    private void StartTutorialSetting()
    {
        LibraryRect();
    }
    private void OpenLibrary(object[] ps)
    {
        if (ps[0] is Library)
        {
            library = ps[0] as Library;
            library.SetLibrary();
        }

        //EventManager.TriggerEvent(ETutorialEvent.LibraryEventTrigger);
        //라이브러리가 오픈될 때 가 아니라 select가 될때
    }

    private void GetCharacterInfoEvent()
    {
        EventManager.StartListening(EProfileEvent.FindInfoText, GetCharacterInfo);
    }

    public void GetCharacterInfo(object[] ps)
    {
        int id = (int)ps[1];
        if (id == targetCharID1 || id == targetCharID2)
        {
            EventManager.StopListening(EProfileEvent.FindInfoText, GetCharacterInfo);
            ProfileChattingSystem.OnChatEnd += CharacterTabGuide;
            StartChatting(3);
        }
    }

    private void GetIncidentInfoEvent()
    {
        EventManager.StartListening(EProfileEvent.FindInfoText, GetIncidentInfo);
    }

    public void GetIncidentInfo(object[] ps)
    {
        int id = (int)ps[1];
        if (id == targetIncidentID)
        {
            EventManager.StopListening(EProfileEvent.FindInfoText, GetIncidentInfo);
            ProfileChattingSystem.OnChatEnd += IncidentTabGuide;
            StartChatting(1);
        }
    }


    private void IncidentTabGuide()
    {
        EventManager.StartListening(EProfileEvent.ClickIncidentTab, ClickedIncidentTab);
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookIncidentTab, 2f);
        guideObjectName = EGuideObject.IncidentTab;
        EventManager.TriggerEvent(ETutorialEvent.GuideObject, new object[] { EGuideObject.IncidentTab });

        // 사건 탭이 클릭되는 이벤트 듣고
    }

    private void ClickedIncidentTab(object[] obj)
    {
        EventManager.StopListening(EProfileEvent.ClickIncidentTab, ClickedIncidentTab);
        GuideUISystem.EndAllGuide?.Invoke();
        ProfileChattingSystem.OnChatEnd += StartTutorialSetting;
        GetCharacterInfoEvent();
        StartChatting(2);
    }

    private void CharacterTabGuide()
    {
        EventManager.StartListening(EProfileEvent.ClickCharacterTab, ClickedCharacterTab);
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookCharacterTab, 2f);
        guideObjectName = EGuideObject.CharacterTab;
        EventManager.TriggerEvent(ETutorialEvent.GuideObject, new object[] { EGuideObject.CharacterTab });
        
        // 인물 탭이 클릭되는 이벤트 듣고
    }

    private void ClickedCharacterTab(object[] obj)
    {

        GuideUISystem.EndAllGuide?.Invoke();
        TutorialEnd();
        StartChatting(4);
    }

    private void TutorialEnd()
    {
        EventManager.StopListening(EProfileEvent.ClickCharacterTab, ClickedCharacterTab);
        GameManager.Inst.ChangeGameState(EGameState.Game);
        if(library != null)
        {
            library.TutorialLibraryClickRemoveEvent();
        }

        EventManager.StopListening(ETutorialEvent.SelectLibrary, OpenLibrary);
        DataManager.Inst.SetProfilerTutorial(false);
    }

    public void StartCompleteProfileTutorial(object[] ps = null)
    {
        ProfileChattingSystem.OnImmediatelyEndChat?.Invoke();
        ProfileChattingSystem.OnChatEnd = null;

        GuideUISystem.EndAllGuide?.Invoke();

        DataManager.Inst.SetProfilerTutorialIdx(5);
        StopAllCoroutines();

    }

    private void LibraryRect()
    {
        GuideUISystem.OnGuide(libraryRect);
    }
}


