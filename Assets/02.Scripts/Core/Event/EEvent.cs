public enum ECoreEvent
{ 
    None = -1,
    LeftButtonClick,
    ChangeBGM,
    EndDataLoading,
    CursorChange,
    SendCallNumber,
    OpenVolume,
    MainSceneStart,
}

public enum EOverlayEvent
{
    None = -1,
    OpenOverlay,
    SettingOverlay,

}

public enum EBranchEvent
{
    None=-1,
    HideAllPanel,
    ShowTopPanel,
    ShowWorkPanel,
    ShowPostList,
    ShowPost,
}

public enum EQuestEvent
{
    None = -1,
    GetOwnerInfo,
}
public enum EDecisionEvent
{
    None = -1,
    ClickOwnerNameText,
    ClickOwnerBirthdayText,
    ClickOwnerEMailText,
}

public enum EWindowEvent
{
    None = -1,
    ClickNoticeBtn,
    ActivePowerPanel,
    ExpendMenu,
    WindowsSuccessLogin,
    CreateWindow,
    OpenWindowPin,
    AlarmSend,
    AlarmRecieve,
    AlarmCheck,
    CloseAttribute,
}

public enum ENoticeEvent
{
    None = -1,
    GeneratedNotice,
    OpenNoticeSystem,
    ClickNoticeBtn,
    GeneratedProfilerFindNotice,
}

public enum ELibraryEvent
{
    None = -1,
    IconClickOpenFile,
    AddFile,
    SelectIcon,
    SelectNull,
    AddUndoStack,
    ResetRedoStack,
    AddLeftIcon,
    CreateLeftPanel,
}

public enum EBrowserEvent
{
    None = -1,
    OnOpenSite,
    OnUndoSite,
}

#region Site
public enum ESiteEvent
{
    None = -1,
    ResetSite,
}

public enum EYoutubeSiteEvent
{
    None = -1,
    ClickHateBtn,
}

public enum ELoginSiteEvent
{
    None = -1,
    LoginSuccess,
    RequestSite,
    EmailLoginSuccess,
    EmailRequestSite,
    FacebookLoignSuccess,
    FacebookRequestSite,
    FacebookNewPassword,
}

public enum EMailSiteEvent
{
    None = -1,
    PoliceGameClear,
    VisiableMail,
    RefreshPavoriteMail,
}
#endregion

public enum ECutSceneEvent
{
    None = -1,
    ShowCutScene,
    SkipCutScene,
    EndNewsCutScene,
    EndStartCutScene,
    StartTest1CutScene,
}

public enum EDiscordEvent
{
    None = -1,
    ShowChattingPanel,
    ShowImagePanel,
    StartTalk,
    OpenHarmony,
}

public enum EAiChatData
{
    None = -1,
    FirstAiChat,
    LastAiChat,
    Email,
    Password,
    ChattingEnd,
}

public enum EProfilerEvent 
{
    None = -1,
    FindInfoText,
    InstalledProfiler,
    ProfilerSendMessage,
    ShowInfoPanel,
    HideInfoPanel,
    RegisterInfo,
    ClickIncidentTab,
    ClickCharacterTab,
    AddGuideButton,
    EndGuide,
    Maximum,
    Minimum,
    ClickGuideButton,
    ClickGuideToggleButton,
}
public enum EGuideEventType
{
    ClearGuideType,
    GuideConditionCheck,
}

public enum ETutorialType
{
    None = -1,
    Profiler
}

public enum ETutorialEvent
{
    None = -1,
    TutorialStart, // 튜토리얼 시작 이벤트
    USBTutorial,
    ReportTutorial,
    GuideObject,
    SelectLibrary,
    LibraryEventTrigger,
    LibraryGuide,
    EndTutorial,
    IncidentReportOpen,
    GetCharacterInfo,
    GetIncidentInfo,
    CheckTutorialState,
    ClickIncidentCategory,
    ClickCharacterCategory,
    CompleteEvent,
    GetAllInfo,
    OutGoingCall,
}


public enum EDebugSkipEvent
{
    None = -1,
    TutorialSkip,
}

public enum ETextboxEvent
{
    None = -1,
    Shake,
    Delay,
}

public enum EGuideButtonTutorialEvent
{
    None = -1,
    OpenPopup,
    TutorialStart,
    ClickAnyBtn,
    GuideMoveBtn,
}
public enum EMonologEvent
{
    None = -1,
    MonologException,
    MonologEnd,
}
public enum ECallEvent
{
    None = -1,
    AddAutoCompleteCallBtn,
    // CallSystem
    ClickSelectBtn,
    // 상대가 전화를 받은 경우
    RecivivedCall,
}
public enum ETimeEvent
{
    None = -1,
    ChangeTime,
}

public enum EOutStarEvent
{
    None = -1,
    ClickFriendPanel,
}
