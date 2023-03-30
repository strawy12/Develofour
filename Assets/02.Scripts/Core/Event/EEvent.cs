public enum ECoreEvent
{ 
    None = -1,
    LeftButtonClick,
    ChangeBGM,
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
    RemoveGuideButton,
    EndGuide,
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
    ProfileMidiumStart,
    ProfileMidiumEnd,
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

public enum ETextboxEvent
{
    None = -1,
    Shake,
    Delay,
}

public enum EProfileSearchTutorialEvent
{
    None = -1,
    TutorialMonologStart,
    TutorialStart,
    GuideSearchButton,
    ClickSearchButton,
    GuideSearchInputPanel,
    SearchNameText,
    EndTutorial,
}

public enum EMonologEvent
{
    None = -1,
    MonologException,

}