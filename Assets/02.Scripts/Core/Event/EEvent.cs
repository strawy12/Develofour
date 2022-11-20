public enum EEvent 
{
    None = -1,
    LeftButtonClick = 1,
    GeneratedNotice,
    OpenNoticeSystem,
    ClickNoticeBtn,
    ChangeBGM,
    ActivePowerPanel,
    ExpendMenu,
    OpenTextBox,
    ShowCutScene,
    AddFavoriteSite,
    ResetSite,
    ClickHateBtn,
    LoginGoogle,
    End
}

public enum EQuestEvent
{
    None = -1,
    HateBtnClicked = EEvent.End,
    LoginGoogle,
}
