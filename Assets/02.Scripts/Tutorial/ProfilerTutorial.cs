using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ProfilerTutorial : MonoBehaviour
{
    [SerializeField]
    private TutorialTextSO profilerTutorialTextData;
    [SerializeField]
    private float findNameGuideDelay = 60f;
    [SerializeField]
    private FileSO profiler;
    [SerializeField]
    private RectTransform libraryRect;

    public static EGuideObject guideObjectName;

    [SerializeField]
    private int targetIncidentID = Constant.ProfilerInfoKey.INCIDENTREPORT_TITLE;

    [SerializeField]
    private int targetCharID1 = Constant.ProfilerInfoKey.PARKJUYOUNG_NAME;
    [SerializeField]
    private int targetCharID2 = Constant.ProfilerInfoKey.KIMYUJIN_NAME;

    private Library library;

    void Start()
    {
        EventManager.StartListening(ETutorialEvent.TutorialStart, CreatePopUp);
        EventManager.StartListening(ETutorialEvent.LibraryGuide, LibraryRect);
    }

    private void CreatePopUp(object[] ps)
    {
#if UNITY_EDITOR
        WindowManager.Inst.PopupOpen(profiler, profilerTutorialTextData.popText, () => StartTutorial(null), () => StartCompleteProfilerTutorial());
#else
        WindowManager.Inst.PopupOpen(profiler, profilerTutorialTextData.popText, () => StartTutorial(null), null);
#endif
    }

    private void StartTutorial(object[] ps)
    {
        StartCoroutine(StartProfilerTutorial());
    }

    public void StartChatting(int textListIndex)
    {
        ProfilerChattingSystem.OnChatEnd += () => DataManager.Inst.SetProfilerTutorialIdx();
        ProfilerChattingSystem.OnPlayChatList?.Invoke(profilerTutorialTextData.tutorialTexts[textListIndex].data, 1.5f, true);
    }

    private IEnumerator StartProfilerTutorial()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        ProfilerChattingSystem.OnChatEnd += StartTutorialSetting;

        GetIncidentInfoEvent();
        EventManager.StartListening(ETutorialEvent.SelectLibrary, OpenLibrary);
        StartChatting(0);
    }

    private void StartTutorialSetting()
    {
        LibraryRect(null);
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
        EventManager.StartListening(EProfilerEvent.FindInfoText, GetCharacterInfo);
    }

    public void GetCharacterInfo(object[] ps)
    {
        int id = (int)ps[1];
        if (id == targetCharID1 || id == targetCharID2)
        {
            EventManager.StopListening(EProfilerEvent.FindInfoText, GetCharacterInfo);
            ProfilerChattingSystem.OnChatEnd += CharacterTabGuide;
            StartChatting(3);
        }
    }

    private void GetIncidentInfoEvent()
    {
        EventManager.StartListening(EProfilerEvent.FindInfoText, GetIncidentInfo);
    }

    public void GetIncidentInfo(object[] ps)
    {
        int id = (int)ps[1];
        if (id == targetIncidentID)
        {
            EventManager.StopListening(EProfilerEvent.FindInfoText, GetIncidentInfo);
            ProfilerChattingSystem.OnChatEnd += IncidentTabGuide;
            StartChatting(1);
        }
    }


    private void IncidentTabGuide()
    {
        EventManager.StartListening(EProfilerEvent.ClickIncidentTab, ClickedIncidentTab);
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookIncidentTab, 2f);
        guideObjectName = EGuideObject.IncidentTab;
        EventManager.TriggerEvent(ETutorialEvent.GuideObject, new object[] { EGuideObject.IncidentTab });

        // 사건 탭이 클릭되는 이벤트 듣고
    }

    private void ClickedIncidentTab(object[] obj)
    {
        EventManager.StopListening(EProfilerEvent.ClickIncidentTab, ClickedIncidentTab);
        GuideUISystem.EndAllGuide?.Invoke();
        ProfilerChattingSystem.OnChatEnd += StartTutorialSetting;
        GetCharacterInfoEvent();
        StartChatting(2);
    }

    private void CharacterTabGuide()
    {
        EventManager.StartListening(EProfilerEvent.ClickCharacterTab, ClickedCharacterTab);
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
        EventManager.StopListening(EProfilerEvent.ClickCharacterTab, ClickedCharacterTab);
        GameManager.Inst.ChangeGameState(EGameState.Game);
        GuideUISystem.EndAllGuide?.Invoke();
        if (library != null)
        {
            library.TutorialLibraryClickRemoveEvent();
        }

        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { EProfilerCategory.AssistantProfile, 115 });
        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { EProfilerCategory.PoliceProfile, 116 });
        EventManager.TriggerEvent(EProfilerEvent.AddGuideButton);
        CallSystem.Inst.OnAnswerCall(ECharacterDataType.Assistant, Constant.MonologKey.END_PROFILER_TUTORIAL);
        EventManager.StopListening(ETutorialEvent.SelectLibrary, OpenLibrary);
    }

#if UNITY_EDITOR
    public void StartCompleteProfilerTutorial(object[] ps = null) // For Debug
    {
        EventManager.TriggerEvent(ECallEvent.AddAutoCompleteCallBtn, new object[1] { "01023459876" });
        ProfilerChattingSystem.OnImmediatelyEndChat?.Invoke();
        ProfilerChattingSystem.OnChatEnd = null;
        EventManager.TriggerEvent(EProfilerEvent.AddGuideButton);
        GuideUISystem.EndAllGuide?.Invoke();
        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { EProfilerCategory.AssistantProfile, 115 });
        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { EProfilerCategory.PoliceProfile, 116 });

        DataManager.Inst.SetProfilerTutorialIdx(5);
        StopAllCoroutines();

    }
#endif

    private void LibraryRect(object[] ps)
    {
        GuideUISystem.OnGuide(libraryRect);
        Debug.Log(libraryRect);
    }
}


