using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class ProfilerChattingSystem : TextSystem
{
    //public static Action<string, bool, bool> OnPlayChat;
    public static Action<AIChattingTextDataSO, float, bool> OnPlayChatList;

    public static Action OnChatEnd;

    public static Action OnImmediatelyEndChat;

    public Sprite aiChattingSprite;

    private AIChattingTextDataSO currentChatData;

    private float currentDelay = 0f;

    private float currentDataIndex;

    private string RECIEVE_IMAGE = "이미지가 전송되었습니다.";

    public static bool isChatting;

    private void Awake()
    {
        OnPlayChatList += StartChatting;
        //OnPlayChat += StartChatting;

        OnImmediatelyEndChat += ImmediateEndChat;
    }

    //public void StartChatting(string data, bool isSave, bool isEnd)
    //{
    //    currentTextData = data;

    //    PrintText(isSave);

    //    if (isEnd)
    //    {
    //        EndChatting();
    //    }
    //}

    // delay = 채팅 간격 시간
    public void StartChatting(AIChattingTextDataSO list, float delay, bool isSave)
    {
        Debug.Log(list);
        currentChatData = list;

        StartCoroutine(ChattingCoroutine(delay, isSave));
    }

    private void ImmediateEndChat()
    {
        StopAllCoroutines();
    }

    private IEnumerator ChattingCoroutine(float delay, bool isSave)
    {
        isChatting = true;
        foreach (AIChat data in currentChatData.AIChatList)
        {
            if(data.sprite == null && data.text != null) // 텍스트
            {
                PrintText(data.text, isSave);
            }
            else if(data.sprite != null)
            {
                PrintImage(data.sprite, isSave);
            }
            else
            {
                Debug.Log("ChatData Error");
            }

            yield return new WaitForSeconds(delay);

            if (currentDelay != 0f)
            {
                yield return new WaitForSeconds(currentDelay);
                currentDelay = 0f;
            }
        }

        EndChatting();
    }

    private void EndChatting()
    {
        isChatting = false;
        OnChatEnd?.Invoke();

        OnChatEnd = null;
    }

    private void PrintText(string currentStr, bool isSave)
    {
        // 이벤트매니저로 쏴주고 
        // 데이터 저장

        currentStr = RemoveCommandText(currentStr, true);
        foreach (Action trigger in triggerDictionary.Values)
        {
            trigger?.Invoke();
        }

        EventManager.TriggerEvent(EProfilerEvent.ProfilerSendMessage, new object[] { currentStr });

        if (isSave)
        {
            DataManager.Inst.AddTextAiChattingList(currentStr);
        }

        SendNotice(currentStr);
    }

    private void PrintImage(Sprite sprite, bool isSave)
    {
        EventManager.TriggerEvent(EProfilerEvent.ProfilerSendMessage, new object[] { sprite });

        if (isSave)
        {
            DataManager.Inst.AddImageAiChattingList(sprite);
        }

        SendNotice(RECIEVE_IMAGE);
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
