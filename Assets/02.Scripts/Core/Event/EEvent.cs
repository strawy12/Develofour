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

}

public enum EWindowEvent
{
    None = -1,
    ClickNoticeBtn,
    ActivePowerPanel,
    ExpendMenu,
}

public enum ENoticeEvent
{
    None = -1,
    GeneratedNotice,
    OpenNoticeSystem,
    ClickNoticeBtn,
    DiscordNotice,
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