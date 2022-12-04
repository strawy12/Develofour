using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        EventManager.StartListening(EQuestEvent.BlogCleanUp, ShowBlogDeleteGmail);
    }

    private void HateBtnClicked(object[] emptyParam)
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.PressHateButton);

        EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSite, new object[] { ESiteLink.Email });
        EventManager.TriggerEvent(EBrowserEvent.RemoveFavoriteSite, new object[] { ESiteLink.Youtube_News });

        EventManager.StopListening(EQuestEvent.HateBtnClicked, HateBtnClicked);
    }

    private void GoogleLoginSuccess(object[] emptyParam)
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.SuccessLoginMail);

        EventManager.StopListening(EQuestEvent.LoginGoogle, GoogleLoginSuccess);
    }

    private void ClearPoliceMiniGame(object[] emptyParam)
    {
        NoticeSystem.OnGeneratedNotice?.Invoke(ENoticeType.PoliceReplyMail);

        EventManager.StopListening(EQuestEvent.PoliceMiniGameClear, ClearPoliceMiniGame);
    }

    private void ShowBlogDeleteGmail(object[] emptyParam)
    {
        EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSite, new object[] { ESiteLink.Brunch });

        EventManager.StopListening(EQuestEvent.BlogCleanUp, ShowBlogDeleteGmail);
    }
}
