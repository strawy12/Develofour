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

    [SerializeField]
    public static EGuideObject guideObjectName;

    [SerializeField]
    private int targetIncidentID = Constant.ProfilerInfoKey.INCIDENTREPORT_TITLE;

    private int targetCharID1_1 = Constant.ProfilerInfoKey.PARKJUYOUNG_NAME;
    private int targetCharID1_2 = Constant.ProfilerInfoKey.PARKJUYOUNG_INCIDENT;
    private int targetCharID2_1 = Constant.ProfilerInfoKey.KIMYUJIN_NAME;
    private int targetCharID2_2 = Constant.ProfilerInfoKey.KIMYUJIN_INCIDENT;

    private Library library;

    void Start()
    {
        EventManager.StartListening(ETutorialEvent.TutorialStart, CheckState);
        EventManager.StartListening(ETutorialEvent.LibraryGuide, LibraryRect);
    }

    private void CheckState(object[] ps)
    {
        if(DataManager.Inst.GetProfilerTutorialState() == TutorialState.NotStart) // 맨 처음 시작
        {
        #if UNITY_EDITOR
            WindowManager.Inst.PopupOpen(profiler, profilerTutorialTextData.popText, () => StartTutorial(null), () => StartCompleteProfilerTutorial());
        #else
            WindowManager.Inst.PopupOpen(profiler, profilerTutorialTextData.popText, () => StartTutorial(null), null);
        #endif
        }
        else if (DataManager.Inst.GetProfilerTutorialState() == TutorialState.ClickIncidentInfo)
        {
            if(DataManager.Inst.IsProfilerInfoData(targetIncidentID))
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
                || DataManager.Inst.IsProfilerInfoData(targetCharID1_2)
                || DataManager.Inst.IsProfilerInfoData(targetCharID2_2))
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
        StartCoroutine(StartProfilerTutorial());
    }

    public void StartChatting(TutorialState state)
    {
        ProfilerChattingSystem.OnChatEnd += () => DataManager.Inst.SetProfilerTutorialState(state);
        ProfilerChattingSystem.OnPlayChatList?.Invoke(profilerTutorialTextData.tutorialTexts[(int)state].data, 1.5f, true);
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
        int id = (int)ps[0];
        if (id == targetCharID1_1 || id == targetCharID1_2 || id == targetCharID2_1 || id == targetCharID2_2)
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
        int id = (int)ps[0];
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

        GuideUISystem.OnEndAllGuide?.Invoke();
        TutorialEnd();
        StartChatting(TutorialState.EndTutorial);
    }

    private void TutorialEnd()
    {
        EventManager.StopListening(EProfilerEvent.ClickCharacterTab, ClickedCharacterTab);
        GameManager.Inst.ChangeGameState(EGameState.Game);
        GuideUISystem.OnEndAllGuide?.Invoke();
        if (library != null)
        {
            library.TutorialLibraryClickRemoveEvent();
        }

        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { EProfilerCategory.AssistantProfile, 115 });
        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { EProfilerCategory.PoliceProfile, 116 });
        EventManager.TriggerEvent(EProfilerEvent.AddGuideButton);
        MonologSystem.OnEndMonologEvent = () => FileManager.Inst.AddFile(164, 6);
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
        GuideUISystem.OnEndAllGuide?.Invoke();
        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { EProfilerCategory.AssistantProfile, 115 });
        EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { EProfilerCategory.PoliceProfile, 116 });
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


