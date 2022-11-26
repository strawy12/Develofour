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
    }

    private void HateBtnClicked(object[] emptyParam)
    {
        NoticeData data = new NoticeData();
        data.head = "경찰 출두장 메일 확인하기";
        data.body = "즐겨찾기에 있는 메일 바로가기 버튼을 이용해 갈 수 있습니다";
        NoticeSystem.OnGeneratedNotice?.Invoke(data);

        object[] ps = new object[1] { ESiteLink.Email };

        EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSite, ps);
        EventManager.StopListening(EQuestEvent.HateBtnClicked, HateBtnClicked);
    }
}
