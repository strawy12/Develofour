public enum ECoreEvent
{ 
    None = -1,
    LeftButtonClick,
    ChangeBGM,
    EndDataLoading,
    CursorChange,
    CoverPanelSetting,
    SendCallNumber,
    OpenVolume,
    MainSceneStart,
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
    GeneratedProfileFindNotice,
}

public enum ELibraryEvent
{
    None = -1,
    IconClickOpenFile,
    ButtonOpenFile,
    AddFile,
    SelectIcon,
    SelectNull,
    AddUndoStack,
    ResetRedoStack,
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

public enum EProfileEvent 
{
    None = -1,
    FindInfoText,
    InstalledProfile,
    ProfileSendMessage,
    AddGuideButton,
    ClickGuideToggleButton,
    EndGuide,
    ShowInfoPanel,
    HideInfoPanel,
    FindInfoInProfile,
}
public enum EGuideEventType
{
    ClearGuideType,
    GuideConditionCheck,
}

public enum ETutorialEvent
{
    None = -1,
    TutorialStart, // 튜토리얼 시작 이벤트
    SearchBtnGuide,
    ClickSearchBtn,
    SearchNameText,
    EndClickInfoTutorial,
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
    GuideMoveBtn,
    ClickMoveBtn,
    ClickAnyBtn,
}
public enum EMonologEvent
{
    None = -1,
    MonologException,
}
public enum ECallEvent
{
    None = -1,
    AddAutoCompleteCallBtn,
    CallInit,
}
