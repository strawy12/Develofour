using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ProfilerTutorial : MonoBehaviour
{
    //갈아엎기
    [SerializeField] 
    private List<AIChattingDataSO> aiChattingDataList;

    #region 상수 변수들
    private int FIRST_TUTORIAL = 0;
    private int OVERLAY_TUTORIAL = 1;
    private int CHARACTER_TUTORIAL = 2;
    private int INCIDENT_TUTORIAL = 3;
    private int COMPLETE_TUTORIAL = 4;
    #endregion

    private string popupText = "튜토리얼을 시작하시겠습니까?";

    [SerializeField]
    private float findNameGuideDelay = 60f;
    [SerializeField]
    private FileSO profiler;
    [SerializeField]
    private RectTransform libraryRect;

    public static EGuideObject guideObjectName;

    [SerializeField]
    private string targetIncidentID = Constant.ProfilerInfoKey.INCIDENTREPORT_TITLE;

    [SerializeField]
    private string targetCharID1 = Constant.ProfilerInfoKey.PARKJUYOUNG_NAME;
    [SerializeField]
    private string targetCharID2 = Constant.ProfilerInfoKey.KIMYUJIN_NAME;

    private Library library;

    void Start()
    {
        EventManager.StartListening(ETutorialEvent.TutorialStart, CreatePopUp);
        EventManager.StartListening(ETutorialEvent.LibraryGuide, LibraryRect);
    }

    private void CreatePopUp(object[] ps)
    {
#if UNITY_EDITOR
        WindowManager.Inst.PopupOpen(profiler, popupText, () => StartTutorial(null), () => StartCompleteProfilerTutorial());
#else
        WindowManager.Inst.PopupOpen(profiler, popupText, () => StartTutorial(null), null);
#endif
    }

    private void StartTutorial(object[] ps)
    {
        StartCoroutine(StartProfilerTutorial());
    }

    public void StartChatting(int textListIndex)
    {
        ProfilerChattingSystem.OnChatEnd += () => EventManager.TriggerEvent(ETutorialEvent.CheckTutorialState);
        GameManager.Inst.ChangeGameState(EGameState.Tutorial_Chat);
        ProfilerChattingSystem.OnPlayChatList?.Invoke(aiChattingDataList[textListIndex], 1.5f, true);
    }



    private IEnumerator StartProfilerTutorial()
    {
        //튜토리얼 첫 시작
        //게임 스테이트 변경
        //텍스트 출력
        //사건보고서 가이드
        //사건보고서가 열렸을 때 이벤트 추가
        yield return new WaitForSeconds(0.5f);
        GameManager.Inst.ChangeGameState(EGameState.Tutorial_Chat);
        ProfilerChattingSystem.OnChatEnd += StartTutorialSetting; //라이브러리 가이드
        ProfilerChattingSystem.OnChatEnd += (() => GameManager.Inst.ChangeGameState(EGameState.Tutorial_NotChat));

        EventManager.StartListening(ETutorialEvent.IncidentReportOpen, OpenIncidentReport);

        StartChatting(FIRST_TUTORIAL);

        GetIncidentInfoEvent();
        GetCharacterInfoEvent();

        EventManager.StartListening(ETutorialEvent.SelectLibrary, OpenLibrary);
    }

    private void OpenIncidentReport(object[] obj)
    {
        EventManager.StopListening(ETutorialEvent.IncidentReportOpen, OpenIncidentReport);
        StartChatting(OVERLAY_TUTORIAL);
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
        string id = (string)ps[0];
        //TODO 얻은 정보가 캐릭터 정보라면 if문 적기

        EventManager.StopListening(EProfilerEvent.FindInfoText, GetCharacterInfo);
        StartChatting(CHARACTER_TUTORIAL);
        EventManager.StartListening(EProfilerEvent.ClickCharacterTab, CompleteTutorial);
    }

    private void GetIncidentInfoEvent()
    {
        EventManager.StartListening(EProfilerEvent.FindInfoText, GetIncidentInfo);
    }

    public void GetIncidentInfo(object[] ps)
    {
        string id = (string)ps[0];
        //TODO 얻은 정보가 사건 정보라면 if문 적기
        //사건보고서의 모든 사건 정보를 가져와서 하면 될듯?
        //일단 리소스 매니저 되돌려놓고 so 직접 만들어서 디버그해보기
        //버그 많을듯ㅋㅋ
        EventManager.StopListening(EProfilerEvent.FindInfoText, GetIncidentInfo);
        StartChatting(INCIDENT_TUTORIAL);
        EventManager.StartListening(EProfilerEvent.ClickIncidentTab, CompleteTutorial);
    }

    public void CompleteTutorial(object[] ps)
    {
        EventManager.StopListening(EProfilerEvent.ClickIncidentTab, CompleteTutorial);
        EventManager.StopListening(EProfilerEvent.ClickCharacterTab, CompleteTutorial);
        StartChatting(COMPLETE_TUTORIAL);
        ProfilerChattingSystem.OnChatEnd = null; //startchatting의 게임 스테이트 변경되는거 강제로 막기
        GameManager.Inst.ChangeGameState(EGameState.Tutorial_NotChat); //그리고 스테이트 원래대로
    }

    //private void IncidentTabGuide()
    //{
    //    EventManager.StartListening(EProfilerEvent.ClickIncidentTab, ClickedIncidentTab);
    //    NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookIncidentTab, 2f);
    //    guideObjectName = EGuideObject.IncidentTab;
    //    EventManager.TriggerEvent(ETutorialEvent.GuideObject, new object[] { EGuideObject.IncidentTab });

    //    // 사건 탭이 클릭되는 이벤트 듣고
    //}

    //private void ClickedIncidentTab(object[] obj)
    //{
    //    EventManager.StopListening(EProfilerEvent.ClickIncidentTab, ClickedIncidentTab);
    //    GuideUISystem.EndAllGuide?.Invoke();
    //    ProfilerChattingSystem.OnChatEnd += StartTutorialSetting;
    //    GetCharacterInfoEvent();
    //    StartChatting(2);
    //}

    //private void CharacterTabGuide()
    //{
    //    EventManager.StartListening(EProfilerEvent.ClickCharacterTab, ClickedCharacterTab);
    //    NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookCharacterTab, 2f);
    //    guideObjectName = EGuideObject.CharacterTab;
    //    EventManager.TriggerEvent(ETutorialEvent.GuideObject, new object[] { EGuideObject.CharacterTab });
        
    //    // 인물 탭이 클릭되는 이벤트 듣고
    //}

    //private void ClickedCharacterTab(object[] obj)
    //{

    //    GuideUISystem.EndAllGuide?.Invoke();
    //    TutorialEnd();
    //    StartChatting(4);
    //}

    //private void TutorialEnd()
    //{
    //    EventManager.StopListening(EProfilerEvent.ClickCharacterTab, ClickedCharacterTab);
    //    GameManager.Inst.ChangeGameState(EGameState.Game);
    //    GuideUISystem.EndAllGuide?.Invoke();
    //    if (library != null)
    //    {
    //        library.TutorialLibraryClickRemoveEvent();
    //    }

    //    EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { "IC_C_10", "" });
    //    EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { "IC_C_11", "" });
    //    EventManager.TriggerEvent(EProfilerEvent.AddGuideButton);
    //    CallSystem.OnInComingCall(Constant.CharacterKey.ASSISTANT, Constant.MonologKey.END_PROFILER_TUTORIAL);
    //    EventManager.StopListening(ETutorialEvent.SelectLibrary, OpenLibrary);
    //    EventManager.TriggerEvent(ETutorialEvent.EndTutorial);
    //}

#if UNITY_EDITOR
    public void StartCompleteProfilerTutorial(object[] ps = null) // For Debug
    {
        EventManager.TriggerEvent(ECallEvent.AddAutoCompleteCallBtn, new object[1] { "01023459876" });
        ProfilerChattingSystem.OnImmediatelyEndChat?.Invoke();
        ProfilerChattingSystem.OnChatEnd = null;

        TutorialEnd();
        StopAllCoroutines();

    }
#endif

    private void LibraryRect(object[] ps)
    {
        GuideUISystem.OnGuide?.Invoke(libraryRect);
        Debug.Log(libraryRect);
    }
}


