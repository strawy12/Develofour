using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ProfileChattingSystem : TextSystem
{
    public static Action<TextData, bool> OnPlayChat;
    public static Action<List<TextData>, float, bool> OnPlayChatList;
    
    public static Action OnChatEnd;

    public Sprite aiChattingSprite;

    private List<TextData> textDataList;
    private TextData currentTextData;

    private void Awake()
    {
        OnPlayChatList += StartChatting;
        OnPlayChat += StartChatting;
    }

    public void StartChatting(TextData data, bool isSave)
    {
        currentTextData = data;

        PrintText();
        EndChatting();
    }

    // delay = ä�� ���� �ð�
    public void StartChatting(List<TextData> list, float delay, bool isSave)
    {
        textDataList = list;

        StartCoroutine(ChattingCoroutine(delay));
    }

    private IEnumerator ChattingCoroutine(float delay)
    {
        foreach(TextData data in textDataList)
        {
            currentTextData = data;
            PrintText();

            yield return new WaitForSeconds(delay);
        }

        EndChatting();
    }

    private void EndChatting()
    {
        OnChatEnd?.Invoke();
    }

    private void PrintText()
    {
        // �̺�Ʈ�Ŵ����� ���ְ� 
        // ������ ����

        currentTextData.text = RemoveCommandText(currentTextData.text, true);
        foreach (Action trigger in triggerDictionary.Values)
        {
            trigger?.Invoke();
        }

        EventManager.TriggerEvent(EProfileEvent.SendMessage, new object[] { currentTextData });
        DataManager.Inst.AddAiChattingList(currentTextData);

        SendNotice(currentTextData.text);
    }

    public void SendNotice(string body)
    {
        NoticeSystem.OnNotice.Invoke("AI���Լ� �޼����� �����߽��ϴ�!", body, 0, true, null, Color.white, ENoticeTag.AIAlarm);
    }
}
