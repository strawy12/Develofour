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
}

public enum EWindowEvent
{
    None = -1,
    ClickNoticeBtn,
    ActivePowerPanel,
}

public enum ENoticeEvent
{
    None = -1,
    GeneratedNotice,
    OpenNoticeSystem,
    ClickNoticeBtn,
}

public enum EBrowserEvent
{
    None = -1,
    OnOpenSite,
    OnUndoSite,
    AddFavoriteSite,
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
}

#endregion

public enum ECutSceneEvent
{
    None = -1,
    ShowCutScene ,
    SkipCutScene,
    EndNewsCutScene,
}


