using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class ProfilerTutorial : MonoBehaviour
{
    ////갈아엎기
    //[SerializeField] 
    //private TutorialTextSO tutorialTextList;

    //#region 상수 변수들
    //private int FIRST_TUTORIAL = 0;
    //private int OVERLAY_TUTORIAL = 1;
    //private int CHARACTER_TUTORIAL = 2;
    //private int INCIDENT_TUTORIAL = 3;
    //private int COMPLETE_TUTORIAL = 4;
    [SerializeField]
    private TutorialTextSO profilerTutorialTextData;
    [SerializeField]
    private FileSO profiler;
    [SerializeField]
    private RectTransform libraryRect;

    [SerializeField]
    public static EGuideObject guideObjectName;

    private string targetIncidentID = Constant.ProfilerInfoKey.INCIDENTREPORT_TITLE;
    private string targetCharID1_1 = Constant.ProfilerInfoKey.PARKJUYOUNG_NAME;
    private string targetCharID1_2 = Constant.ProfilerInfoKey.PARKJUYOUNG_INCIDENT;
    private string targetCharID2_1 = Constant.ProfilerInfoKey.KIMYUJIN_NAME;

    private string popupText = "튜토리얼을 시작하시겠습니까?";


    private Library library;

    void Start()
    {
        GameManager.Inst.OnGameStartCallback += Init;
    }

    private void Init()
    {
        EventManager.StartListening(ETutorialEvent.TutorialStart, CheckState);
        EventManager.StartListening(ETutorialEvent.LibraryGuide, LibraryRect);

        if (profiler == null)
        {
            profiler = FileManager.Inst.GetFile(Constant.FileID.PROFILER);
        }
    }

    private void CheckState(object[] ps)
    {
        if (DataManager.Inst.GetProfilerTutorialState() == TutorialState.NotStart) // 맨 처음 시작
        {
#if UNITY_EDITOR
            WindowManager.Inst.PopupOpen(profiler, profilerTutorialTextData.popText, () => StartTutorial(null), () => StartCompleteProfilerTutorial());
#else
                WindowManager.Inst.PopupOpen(profiler, profilerTutorialTextData.popText, () => StartTutorial(null), null);
#endif
        }
        else if (DataManager.Inst.GetProfilerTutorialState() == TutorialState.ClickIncidentInfo)
        {
            if (DataManager.Inst.IsProfilerInfoData(targetIncidentID))
            {
                ProfilerChattingSystem.OnChatEnd += IncidentTabGuide;
                StartChatting(TutorialState.ClickIncidentTab);
            }
            else
            {
                GuideUISystem.OnEndAllGuide?.Invoke();
                StartTutorialSetting();
                GetIncidentInfoEvent();
            }
        }
        else if (DataManager.Inst.GetProfilerTutorialState() == TutorialState.ClickIncidentTab)
        {
            IncidentTabGuide();
        }
        else if (DataManager.Inst.GetProfilerTutorialState() == TutorialState.ClickCharacterInfo)
        {
            if (DataManager.Inst.IsProfilerInfoData(targetCharID1_1)
                || DataManager.Inst.IsProfilerInfoData(targetCharID2_1)
                || DataManager.Inst.IsProfilerInfoData(targetCharID1_2))
            {
                ProfilerChattingSystem.OnChatEnd += CharacterTabGuide;
                StartChatting(TutorialState.ClickCharacterTab);
            }
            else
            {
                GuideUISystem.OnEndAllGuide?.Invoke();
                StartTutorialSetting();
                GetCharacterInfoEvent();
            }
        }
        else if (DataManager.Inst.GetProfilerTutorialState() == TutorialState.ClickCharacterTab)
        {
            CharacterTabGuide();
        }
    }

    private void StartTutorial(object[] ps)
    {
        if (ProfilerWindow.CurrentProfiler != null)
            WindowManager.Inst.SelectObject(ProfilerWindow.CurrentProfiler);
        StartCoroutine(StartProfilerTutorial());
    }

    public void StartChatting(TutorialState state)
    {
        ProfilerChattingSystem.OnChatEnd += () => DataManager.Inst.SetProfilerTutorialState(state);
        ProfilerChattingSystem.OnPlayChatList?.Invoke(profilerTutorialTextData.tutorialTexts[(int)state], 1.5f, true);
    }

    private IEnumerator StartProfilerTutorial()
    {
        yield return new WaitForSeconds(0.5f);
        GameManager.Inst.ChangeGameState(EGameState.Tutorial);
        ProfilerChattingSystem.OnChatEnd += StartTutorialSetting;

        GetIncidentInfoEvent();
        EventManager.StopListening(ETutorialEvent.SelectLibrary, OpenLibrary);
        EventManager.StartListening(ETutorialEvent.SelectLibrary, OpenLibrary);
        StartChatting(TutorialState.ClickIncidentInfo);
    }

    private void StartTutorialSetting()
    {
        LibraryRect(null);
    }
    private void OpenLibrary(object[] ps)
    {
        //if (ps[0] is Library)
        //{
        //    library = ps[0] as Library;
        //    library.SetLibrary();
        //}

        //EventManager.TriggerEvent(ETutorialEvent.LibraryEventTrigger);
        //라이브러리가 오픈될 때 가 아니라 select가 될때
    }

    private void GetCharacterInfoEvent()
    {
        EventManager.StopListening(EProfilerEvent.FindInfoText, GetCharacterInfo);
        EventManager.StartListening(EProfilerEvent.FindInfoText, GetCharacterInfo);
    }

    public void GetCharacterInfo(object[] ps)
    {
        string id = (string)ps[0];
        if (id == targetCharID1_1 || id == targetCharID1_2 || id == targetCharID2_1)
        {
            EventManager.StopListening(EProfilerEvent.FindInfoText, GetCharacterInfo);
            ProfilerChattingSystem.OnChatEnd += CharacterTabGuide;
            StartChatting(TutorialState.ClickCharacterTab);
        }
    }

    private void GetIncidentInfoEvent()
    {
        EventManager.StopListening(EProfilerEvent.FindInfoText, GetIncidentInfo);
        EventManager.StartListening(EProfilerEvent.FindInfoText, GetIncidentInfo);
    }

    public void GetIncidentInfo(object[] ps)
    {
        string id = (string)ps[0];
        if (id == targetIncidentID)
        {
            EventManager.StopListening(EProfilerEvent.FindInfoText, GetIncidentInfo);
            ProfilerChattingSystem.OnChatEnd += IncidentTabGuide;
            StartChatting(TutorialState.ClickIncidentTab);
        }
    }


    private void IncidentTabGuide()
    {
        EventManager.StopListening(EProfilerEvent.ClickIncidentTab, ClickedIncidentTab);
        EventManager.StartListening(EProfilerEvent.ClickIncidentTab, ClickedIncidentTab);
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookIncidentTab, 2f);
        guideObjectName = EGuideObject.IncidentTab;
        GuideUISystem.OnEndAllGuide?.Invoke();
        EventManager.TriggerEvent(ETutorialEvent.GuideObject, new object[] { EGuideObject.IncidentTab });

        // 사건 탭이 클릭되는 이벤트 듣고
    }

    private void ClickedIncidentTab(object[] obj)
    {
        EventManager.StopListening(EProfilerEvent.ClickIncidentTab, ClickedIncidentTab);
        guideObjectName = EGuideObject.None;
        ProfilerChattingSystem.OnChatEnd += StartTutorialSetting;
        GuideUISystem.OnEndAllGuide?.Invoke();
        GetCharacterInfoEvent();
        DataManager.Inst.SetProfilerTutorialState(TutorialState.ClickCharacterInfo);
        StartChatting(TutorialState.ClickCharacterInfo);
    }

    private void CharacterTabGuide()
    {
        EventManager.StopListening(EProfilerEvent.ClickCharacterTab, ClickedCharacterTab);
        EventManager.StartListening(EProfilerEvent.ClickCharacterTab, ClickedCharacterTab);
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.LookCharacterTab, 2f);
        guideObjectName = EGuideObject.CharacterTab;
        GuideUISystem.OnEndAllGuide?.Invoke();
        EventManager.TriggerEvent(ETutorialEvent.GuideObject, new object[] { EGuideObject.CharacterTab });

        // 인물 탭이 클릭되는 이벤트 듣고
    }

    private void ClickedCharacterTab(object[] obj)
    {
        EventManager.StopListening(EProfilerEvent.ClickCharacterTab, ClickedCharacterTab);
        ProfilerTutorial.guideObjectName = EGuideObject.None;
        GuideUISystem.OnEndAllGuide?.Invoke();
        ProfilerChattingSystem.OnChatEnd += OverlayTutorial;
        StartChatting(TutorialState.Overlay);
    }

    private void OverlayTutorial() //모든 정보 획득
    {
        GuideUISystem.OnEndAllGuide?.Invoke();
        //라이브러리 가이드
        EventManager.StopListening(EProfilerEvent.GetAllInfo, GetAllInfo);
        EventManager.StartListening(EProfilerEvent.GetAllInfo, GetAllInfo);
    }
    
    private void GetAllInfo(object[] obj)
    {
        //모든 정보를 얻으면
        EventManager.StopListening(EProfilerEvent.GetAllInfo, GetAllInfo);
        GuideUISystem.OnEndAllGuide?.Invoke();
        StartChatting(TutorialState.FileLock);
        ProfilerChattingSystem.OnChatEnd += FileAdd;

        EventManager.StopListening(ELibraryEvent.IconClickOpenFile, FileOpenCheck);
        EventManager.StartListening(ELibraryEvent.IconClickOpenFile, FileOpenCheck);
        //해당 폴더 라이브러리 열었을때 이벤트
    }

    private void FileAdd()
    {
        FileManager.Inst.AddFile(Constant.FileID.Test, Constant.FileID.USB);
        //테스트 폴더 추가
    }

    private void FileOpenCheck(object[] ps)
    {
        if((ps[0] is FileSO) == true)
        {
            FileSO file = ps[0] as FileSO;
            if(file.ID == Constant.FileID.Test)
            {
                EventManager.StopListening(ELibraryEvent.IconClickOpenFile, FileOpenCheck);
                FileOpen();
            }
        }
    }

    private void FileOpen()
    {
        CallSystemUI.isCanClick = false;
        TutorialEnd();
        StartChatting(TutorialState.EndTutorial);
    }

    private void TutorialEnd()
    {
        GameManager.Inst.ChangeGameState(EGameState.Game);
        GuideUISystem.OnEndAllGuide?.Invoke();
        if (library != null)
        {
            library.TutorialLibraryClickRemoveEvent();
        }
        EventManager.TriggerEvent(ECallEvent.AddAutoCompleteCallBtn, new object[1] { "01023459876" });
        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { "IC_C_10", "I_C_10_1" });
        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { "IC_C_11", "I_C_11_1" });
        EventManager.TriggerEvent(EProfilerEvent.AddGuideButton);
        ProfilerChattingSystem.OnChatEnd += CallIncoming;
        EventManager.StopListening(ETutorialEvent.SelectLibrary, OpenLibrary);

    }

    private void CallIncoming()
    {
        CallSystem.OnInComingCall?.Invoke("CD_AS", "C_A_I_1");
        MonologSystem.AddOnEndMonologEvent("T_C_A_9", (() => { CallSystemUI.isCanClick = true; }));
        MonologSystem.OnStartMonolog?.Invoke("T_C_A_9", false);
    }

#if UNITY_EDITOR
    public void StartCompleteProfilerTutorial(object[] ps = null) // For Debug
    {
        EventManager.TriggerEvent(ECallEvent.AddAutoCompleteCallBtn, new object[1] { "01023459876" });
        ProfilerChattingSystem.OnImmediatelyEndChat?.Invoke();
        ProfilerChattingSystem.OnChatEnd = null;
        EventManager.TriggerEvent(EProfilerEvent.AddGuideButton);
        GuideUISystem.OnEndAllGuide?.Invoke();
        FileManager.Inst.AddFile(Constant.FileID.INCIDENT_MAP, Constant.FileID.USB);
        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { "IC_C_10", "I_C_10_1" });
        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { "IC_C_11", "I_C_11_1" });
        DataManager.Inst.SetProfilerTutorialState(TutorialState.EndTutorial);
        StopAllCoroutines();

    }
#endif

    private void LibraryRect(object[] ps) 
    {
        GuideUISystem.OnGuide?.Invoke(libraryRect);
        Debug.Log(libraryRect);
    }
}


