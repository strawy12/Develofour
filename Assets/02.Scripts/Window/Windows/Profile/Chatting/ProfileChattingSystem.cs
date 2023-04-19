using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ProfileChattingSystem : TextSystem
{
    public static Action<TextData, bool, bool> OnPlayChat;
    public static Action<List<TextData>, float, bool> OnPlayChatList;
    
    public static Action OnChatEnd;

    public static Action OnImmediatelyEndChat;

    public Sprite aiChattingSprite;

    private List<TextData> textDataList;
    private TextData currentTextData;

    private float currentDelay = 0f;

    private float currentDataIndex;

    private void Awake()
    {
        OnPlayChatList += StartChatting;
        OnPlayChat += StartChatting;

        OnImmediatelyEndChat += ImmediateEndChat;
    }

    public void StartChatting(TextData data, bool isSave, bool isEnd)
    {
        currentTextData = data;

        PrintText(isSave);

        if (isEnd)
        {
            EndChatting();
        }
    }

    // delay = 채팅 간격 시간
    public void StartChatting(List<TextData> list, float delay, bool isSave)
    {
        textDataList = list;

        StartCoroutine(ChattingCoroutine(delay, isSave));
    }

    private void ImmediateEndChat()
    {
        StopAllCoroutines();
    }

    private IEnumerator ChattingCoroutine(float delay, bool isSave)
    {
        foreach(TextData data in textDataList)
        {
            currentTextData = data;
            PrintText(isSave);

            yield return new WaitForSeconds(delay);

            if(currentDelay != 0f)
            {
                yield return new WaitForSeconds(currentDelay);
                currentDelay = 0f;
            }
        }

        currentDataIndex = textDataList.IndexOf(currentTextData);

        EndChatting();
    }

    private void EndChatting()
    {
        OnChatEnd?.Invoke();
        
        OnChatEnd = null;
    }

    private void PrintText(bool isSave)
    {
        // 이벤트매니저로 쏴주고 
        // 데이터 저장
        currentTextData.text = RemoveCommandText(currentTextData.text, true);

        foreach (Action trigger in triggerDictionary.Values)
        {
            trigger?.Invoke();
        }

        if(isSave)
        {
            EventManager.TriggerEvent(EProfileEvent.ProfileSendMessage, new object[] { currentTextData });
        }

        DataManager.Inst.AddAiChattingList(currentTextData);

        SendNotice(currentTextData.text);
    }

    public void SendNotice(string body)
    {
        NoticeSystem.OnNotice.Invoke("AI에게서 메세지가 도착했습니다!", body, 0, true, null, Color.white, ENoticeTag.AIAlarm);
    }

    public override void SetDelay(float value)
    {
        currentDelay = value;
    }
}
