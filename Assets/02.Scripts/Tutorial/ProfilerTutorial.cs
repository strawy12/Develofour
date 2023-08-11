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
    private TutorialTextSO tutorialTextList;

    #region 상수 변수들
    private int FIRST_TUTORIAL = 0;
    private int OVERLAY_TUTORIAL = 1;
    private int CHARACTER_TUTORIAL = 2;
    private int INCIDENT_TUTORIAL = 3;
    private int COMPLETE_TUTORIAL = 4;

    private string ASSISTANT_CALL_DATA_ID = "C_A_I_1";
    private string ASSISTANT_CALL_PROFILE_ID = "CD_AS";

    private string PROFILER_MESSAGE_CHECK = "T_M_97";
    #endregion

    private string popupText = "튜토리얼을 시작하시겠습니까?";
    [SerializeField]
    private FileSO profiler;
    [SerializeField]
    private RectTransform libraryRect;
    public static EGuideObject guideObjectName;


    public static bool IsExistCharacterTODO; //튜토리얼 중 아직 할 일이 남음 ( 정보 튜토리얼 중 패널까지 클릭해야할때 )
    public static bool IsExistIncidentTODO; //튜토리얼 중 아직 할 일이 남음 ( 정보 튜토리얼 중 패널까지 클릭해야할때 )

    public static bool IsLibraryGuide;

    private Library library;

    void Start()
    {
        EventManager.StartListening(ETutorialEvent.TutorialStart, CreatePopUp);
        EventManager.StartListening(ETutorialEvent.LibraryGuide, LibraryRect);
    }

    private void CreatePopUp(object[] ps)
    {
#if UNITY_EDITOR
        WindowManager.Inst.PopupOpen(profiler, popupText, () => StartTutorial(null), null/*, () => StartCompleteProfilerTutorial()*/);
#else
        WindowManager.Inst.PopupOpen(profiler, popupText, () => StartTutorial(null), null);
#endif
    }

    private void StartTutorial(object[] ps)
    {
        DataManager.Inst.SetStartProfilerTutorial(true);
        StartCoroutine(StartProfilerTutorial());
    }

    public void StartChatting(int textListIndex)
    {
        ProfilerChattingSystem.OnChatEnd += () => EventManager.TriggerEvent(ETutorialEvent.CheckTutorialState);
        GameManager.Inst.ChangeGameState(EGameState.Tutorial_Chat);
        ProfilerChattingSystem.OnPlayChatList?.Invoke(tutorialTextList.tutorialChatData[textListIndex], 1.5f, true);
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
        DataManager.Inst.SetPlayingProfilerTutorial(true);

        EventManager.StartListening(ETutorialEvent.IncidentReportOpen, OpenIncidentReport);

        StartChatting(FIRST_TUTORIAL);

        GetIncidentInfoEvent();
        GetCharacterInfoEvent();

        GetAllInfoEvent();

        EventManager.StartListening(ETutorialEvent.CompleteEvent, CompleteTutorial);

        EventManager.StartListening(ETutorialEvent.SelectLibrary, OpenLibrary);
    }



    private void StartTutorialSetting()
    {
        IsLibraryGuide = true;
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

    private void ProfilerCheckMonolog()
    {
        MonologSystem.AddOnEndMonologEvent(PROFILER_MESSAGE_CHECK, (() => GameManager.Inst.ChangeGameState(EGameState.Tutorial_Chat)));
        MonologSystem.OnStartMonolog?.Invoke(PROFILER_MESSAGE_CHECK, false);
    }


    #region Overlay Event
    private void OpenIncidentReport(object[] obj)
    {
        EventManager.StopListening(ETutorialEvent.IncidentReportOpen, OpenIncidentReport);
        IsLibraryGuide = false;
        GuideUISystem.EndAllGuide?.Invoke();
        ProfilerCheckMonolog();
        StartChatting(OVERLAY_TUTORIAL);
    }
    #endregion


    #region Character Event

    private void GetCharacterInfoEvent()
    {
        EventManager.StartListening(ETutorialEvent.GetCharacterInfo, GetCharacterInfo);
    }

    private string CharacterID = string.Empty;

    public void GetCharacterInfo(object[] ps)
    {
        EventManager.StopListening(ETutorialEvent.GetCharacterInfo, GetCharacterInfo);
        IsExistCharacterTODO = true;

        CharacterID = (string)ps[0];
        Debug.Log(CharacterID);

        if (IsExistIncidentTODO) return;

        GetCharacterInfoEvent(ps);
    }

    public void GetCharacterInfoEvent(object[] ps)
    {
        if (ps[0] is bool && (bool)ps[0] == true)
        {
            StartChatting(CHARACTER_TUTORIAL);
        }
        else
        { 
            MonologSystem.AddOnEndMonologEvent(CharacterID, (() => StartChatting(CHARACTER_TUTORIAL)));
            MonologSystem.AddOnEndMonologEvent(CharacterID, (() => ProfilerCheckMonolog()));
        }     

        ProfilerChattingSystem.OnChatEnd +=
            (() => EventManager.TriggerEvent(ETutorialEvent.GuideObject, new object[] { EGuideObject.CharacterTab }));
        //프로파일러 캐릭터 탭 노란색 가이드 필요
        EventManager.StartListening(EProfilerEvent.ClickCharacterTab, ClickedCharacterTab);
    }

    private void ClickedCharacterTab(object[] obj)
    {
        EventManager.StopListening(EProfilerEvent.ClickCharacterTab, ClickedCharacterTab);
        GuideUISystem.EndAllGuide?.Invoke();
        EventManager.TriggerEvent(ETutorialEvent.GuideObject, new object[] { EGuideObject.CharacterCategory });
        EventManager.StartListening(ETutorialEvent.ClickCharacterCategory, ClickedCharacterCategory);
    }

    private void ClickedCharacterCategory(object[] obj)
    {
        EventManager.StopListening(ETutorialEvent.ClickCharacterCategory, ClickedCharacterCategory);
        GuideUISystem.EndAllGuide?.Invoke();
        EventManager.TriggerEvent(ETutorialEvent.CompleteEvent);

        if(IsExistIncidentTODO)
        {
            //ps 전역변수로 빼서 ㄱㄱ?
            GetIncidentInfoEvent(new object[] { true });
        }
    }

    #endregion


    #region Incident Event

    private void GetIncidentInfoEvent()
    {
        EventManager.StartListening(ETutorialEvent.GetIncidentInfo, GetIncidentInfo);
    }

    private string IncidentID = string.Empty;

    public void GetIncidentInfo(object[] ps)
    {
        EventManager.StopListening(ETutorialEvent.GetIncidentInfo, GetIncidentInfo);
        IsExistIncidentTODO = true;
        IncidentID = (string)ps[0];
        Debug.Log(IncidentID);

        if (IsExistCharacterTODO) return;

        GetIncidentInfoEvent(ps);
    }



    private void GetIncidentInfoEvent(object[] ps)
    {
        if (ps[0] is bool && (bool)ps[0] == true)
        {
            StartChatting(INCIDENT_TUTORIAL);
        }
        else
        {
            MonologSystem.AddOnEndMonologEvent(IncidentID, (() => StartChatting(INCIDENT_TUTORIAL)));
            MonologSystem.AddOnEndMonologEvent(IncidentID, (() => ProfilerCheckMonolog()));
        }

        ProfilerChattingSystem.OnChatEnd +=
            (() => EventManager.TriggerEvent(ETutorialEvent.GuideObject, new object[] { EGuideObject.IncidentTab }));

        //프로파일러 사건 탭 노란색 가이드 필요
        EventManager.StartListening(EProfilerEvent.ClickIncidentTab, ClickedIncidentTab);
    }    

    private void ClickedIncidentTab(object[] obj)
    {
        EventManager.StopListening(EProfilerEvent.ClickIncidentTab, ClickedIncidentTab);
        GuideUISystem.EndAllGuide?.Invoke();
        EventManager.TriggerEvent(ETutorialEvent.GuideObject, new object[] { EGuideObject.IncidentCategory });
        EventManager.StartListening(ETutorialEvent.ClickIncidentCategory, ClickedIncidentCategory);
    }

    private void ClickedIncidentCategory(object[] obj)
    {
        EventManager.StopListening(ETutorialEvent.ClickIncidentCategory, ClickedIncidentCategory);
        GuideUISystem.EndAllGuide?.Invoke();
        EventManager.TriggerEvent(ETutorialEvent.CompleteEvent);

        if(IsExistCharacterTODO)
        {
            GetCharacterInfoEvent(new object[] { true });
        }

    }

    #endregion


    #region Call Event
    private void GetAllInfoEvent()
    {
        EventManager.StartListening(ETutorialEvent.GetAllInfo, CallTutorial);
    }

    private void CallTutorial(object[] obj)
    {
        DataManager.Inst.SetCallTutorial(true);
        GameManager.Inst.ChangeGameState(EGameState.Tutorial_Call);
        EventManager.StopListening(ETutorialEvent.GetAllInfo, CallTutorial);
        EventManager.StartListening(ETutorialEvent.OutGoingCall, TutorialEnd);
        //전화걸기
        Debug.Log("전화");
        CallSystem.OnInComingCall?.Invoke(ASSISTANT_CALL_PROFILE_ID, ASSISTANT_CALL_DATA_ID);
    }


    #endregion


    public void CompleteTutorial(object[] ps) //끝이 아니라 캐릭터 혹은 사건 튜토리얼 완료
    {
        EventManager.StopListening(EProfilerEvent.ClickIncidentTab, CompleteTutorial);
        EventManager.StopListening(EProfilerEvent.ClickCharacterTab, CompleteTutorial);
        StartChatting(COMPLETE_TUTORIAL);
        ProfilerChattingSystem.OnChatEnd = null; //startchatting의 게임 스테이트 변경되는거 강제로 막기
        GameManager.Inst.ChangeGameState(EGameState.Tutorial_NotChat); //그리고 스테이트 원래대로
    }

    private void TutorialEnd(object[] obj)
    {
        EventManager.StopListening(ETutorialEvent.OutGoingCall, TutorialEnd);
        GameManager.Inst.ChangeGameState(EGameState.Game); //그리고 스테이트 원래대로
        DataManager.Inst.SetPlayingProfilerTutorial(false);
        DataManager.Inst.SetIsClearTutorial(true);
    }


#if UNITY_EDITOR
    public void StartCompleteProfilerTutorial(object[] ps = null) // For Debug
    {
        EventManager.TriggerEvent(ECallEvent.AddAutoCompleteCallBtn, new object[1] { "01023459876" });
        ProfilerChattingSystem.OnImmediatelyEndChat?.Invoke();
        ProfilerChattingSystem.OnChatEnd = null;

        //TutorialEnd();
        StopAllCoroutines();

    }
#endif

    private void LibraryRect(object[] ps)
    {
        GuideUISystem.OnGuide?.Invoke(libraryRect);
    }
}


