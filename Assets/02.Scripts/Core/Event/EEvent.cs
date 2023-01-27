public enum ECoreEvent
{
    None = -1,
    LeftButtonClick,
    ChangeBGM,
    OpenTextBox,
}
    
public enum EQuestEvent
{
    None = -1,
    HateBtnClicked,
    LoginGoogle,
    PoliceMiniGameClear,
    ShowBrunchGmail,
    EndBrunchPostCleanUp,
    WriterWindowsLoginSuccess,
    GetOwnerInfo,
}
public enum EDecisionEvent
{
    None = -1,
    ClickOwnerNameText,

}
public enum EWindowEvent
{
    None = -1,
    ClickNoticeBtn,
    ActivePowerPanel,
    ExpendMenu,
    WindowsSuccessLogin,
    CreateWindow,
}

public enum ENoticeEvent
{
    None = -1,
    GeneratedNotice,
    OpenNoticeSystem,
    ClickNoticeBtn,
    DiscordNotice,
}
public enum ELibraryEvent
{
    None = -1,
    IconClickOpenFile,
    ButtonOpenFile,
}
public enum EBrowserEvent
{
    None = -1,
    OnOpenSite,
    OnUndoSite,
    AddFavoriteSite,
    RemoveFavoriteSite,
    AddFavoriteSiteAll,
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
    ShowBlogGmail,
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
}

public enum EInputType
{
    None = -1,
    InputMouseDown,
    InputMouse,
    InputMouseUp,
}

public enum EDiscordEvent
{
    None = -1,
    ShowChattingPanel,
    ShowImagePanel,
    StartTalk,
}