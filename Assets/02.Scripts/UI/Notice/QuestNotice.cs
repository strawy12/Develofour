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

        EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSite, new object[] { ESiteLink.Email });
        EventManager.TriggerEvent(EBrowserEvent.RemoveFavoriteSite, new object[] { ESiteLink.Youtube_News });
        EventManager.StopListening(EQuestEvent.HateBtnClicked, HateBtnClicked);
    }
}
