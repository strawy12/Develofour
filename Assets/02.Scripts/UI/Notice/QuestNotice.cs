using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constant;

public class QuestNotice : MonoBehaviour
{
    private void Awake()
    {
        Init();
    }

    private void Init()
    { 
        EventManager.StartListening(EQuestEvent.HateBtnClicked, HateBtnClicked);
        EventManager.StartListening(EQuestEvent.LoginGoogle, GoogleLoginSuccess);
        EventManager.StartListening(EQuestEvent.PoliceMiniGameClear, ClearPoliceMiniGame);
        EventManager.StartListening(EQuestEvent.ShowBrunchGmail, ShowBrunchPostCleanUpMail);
        EventManager.StartListening(EQuestEvent.EndBrunchPostCleanUp, EndClearBrunchPost);
    }
    /// <summary>
    /// object[] = {Decision}
    /// </summary>
    /// <param name="ps"></param>
    private void OnDecisionNotice(object[] ps) 
    {

    }

    private void HateBtnClicked(object[] emptyParam)
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.PressHateButton, 0f);

        EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSite, new object[] { ESiteLink.Email });
        EventManager.TriggerEvent(EBrowserEvent.RemoveFavoriteSite, new object[] { ESiteLink.Youtube_News });

        EventManager.StopListening(EQuestEvent.HateBtnClicked, HateBtnClicked);
    }

    private void GoogleLoginSuccess(object[] emptyParam)
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.SuccessLoginMailSite, 0f);

        EventManager.StopListening(EQuestEvent.LoginGoogle, GoogleLoginSuccess);
    }

    private void ClearPoliceMiniGame(object[] emptyParam)
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.ClearPoliceMinigame, POLICE_REPLY_DELAY);
        EventManager.TriggerEvent(EMailSiteEvent.VisiableMail, new object[] {EMailType.PoliceReply, POLICE_REPLY_DELAY });

        EventManager.StopListening(EQuestEvent.PoliceMiniGameClear, ClearPoliceMiniGame);
    }

    private void ShowBrunchPostCleanUpMail(object[] emptyParam)
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.OpenBrunchDeleteMail, 0f);
        EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSite, new object[] { ESiteLink.Brunch });

        EventManager.StopListening(EQuestEvent.ShowBrunchGmail, ShowBrunchPostCleanUpMail);
    }

    private void EndClearBrunchPost(object[] emptyParam)
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.EndBrunchPostDelete, 0f);
        EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSite, new object[] { ESiteLink.Facebook });

        EventManager.StopListening(EQuestEvent.EndBrunchPostCleanUp, EndClearBrunchPost);
    }
}
