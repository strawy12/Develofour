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

        NoticeData data = new NoticeData();
        data.head = "경찰 출두장 메일 확인하기";
        data.body = "즐겨찾기에 있는 메일 바로가기 버튼을 이용해 갈 수 있습니다";
        NoticeSystem.OnGeneratedNotice?.Invoke(data);

        EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSite, new object[] { ESiteLink.Email });
        EventManager.TriggerEvent(EBrowserEvent.RemoveFavoriteSite, new object[] { ESiteLink.Youtube_News });

        EventManager.StopListening(EQuestEvent.HateBtnClicked, HateBtnClicked);
    }

    private void GoogleLoginSuccess(object[] emptyParam)
    {

        NoticeData data = new NoticeData();
        data.head = "경찰 출두장 메일 확인하기";
        data.body = "메일 창에서 경찰 출두장 메일이 존재합니다";  
        NoticeSystem.OnGeneratedNotice?.Invoke(data);

        EventManager.StopListening(EQuestEvent.LoginGoogle, GoogleLoginSuccess);
    }

    private void ClearPoliceMiniGame(object[] emptyParam)
    {

        NoticeData data = new NoticeData();
        data.head = "블로그 삭제하기";
        data.body = "블로그에 올린 소설들을 모두 삭제하세요.";
        NoticeSystem.OnGeneratedNotice?.Invoke(data);

        EventManager.StopListening(EQuestEvent.PoliceMiniGameClear, ClearPoliceMiniGame);
    }

    private void ShowBlogDeleteGmail(object[] emptyParam)
    {
        EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSite, new object[] { ESiteLink.Brunch });

        EventManager.StopListening(EQuestEvent.BlogCleanUp, ShowBlogDeleteGmail);
    }
}
