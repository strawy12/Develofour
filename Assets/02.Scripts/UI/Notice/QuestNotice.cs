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
        data.head = "���� ����� ���� Ȯ���ϱ�";
        data.body = "���ã�⿡ �ִ� ���� �ٷΰ��� ��ư�� �̿��� �� �� �ֽ��ϴ�";
        NoticeSystem.OnGeneratedNotice?.Invoke(data);

        EventManager.TriggerEvent(EEvent.AddFavoriteSite, ESiteLink.Email_Received);
        EventManager.StopListening(EQuestEvent.HateBtnClicked, HateBtnClicked);
    }
}
