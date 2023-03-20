public enum ECoreEvent
{
    None = -1,
    LeftButtonClick,
    ChangeBGM,
    OpenTextBox,
    EndLoadResources,
    EndDataLoading,
    CursorChange,
    CoverPanelSetting,
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

public enum EInputType
{
    None = -1,
    InputMouseDown,
    InputMouse,
    InputMouseUp,
    InputAnyKeyDown,
    InputAnyKey,
    InputAnyKeyUp,
}

public enum EDiscordEvent
{
    None = -1,
    ShowChattingPanel,
    ShowImagePanel,
    StartTalk,
}

public enum EAiChatData
{
    None = -1,
    FirstAiChat,
    LastAiChat,
    Email,
    Password,
}

public enum EProfileEvent 
{
    None = -1,
    FindInfoText,
    SendMessage,
    InstalledProfile,
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
    BackgroundSignStart, 
    BackgroundSignEnd, 
    LibraryRootCheck, 
    LibraryRequesterInfoStart, 
    LibraryRequesterInfoEnd, 
    LibraryUserButtonStart, 
    LibraryUserButtonEnd, 
    LibraryUSBStart,  
    LibraryUSBEnd, 
    ProfileInfoStart, 
    ProfileInfoEnd, 
    ProfileEventStop, 
    EndClickInfoTutorial,
}


public enum EDebugSkipEvent
{
    None = -1,
    TutorialSkip,
}