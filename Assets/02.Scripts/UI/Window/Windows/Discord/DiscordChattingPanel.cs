using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DiscordChattingPanel : MonoBehaviour
{
    [SerializeField]
    private DiscordMessagePanel messagePrefab;

    [SerializeField]
    private Transform MessageParent;
    [SerializeField]
    private Transform poolParents;
    [SerializeField]
    private TMP_Text stateText;
    [SerializeField]
    private TMP_Text inputChatingText;
    private List<DiscordMessagePanel> messagePoolList;
    private List<DiscordMessagePanel> messageList;

    private Coroutine currentTalkCoroutine; 

    [HideInInspector]
    public DiscordProfileDataSO playerProfileData;
    [HideInInspector]
    public DiscordProfileDataSO opponentProfileData;

    private void Awake()
    {
        messagePoolList = new List<DiscordMessagePanel>();
        messageList = new List<DiscordMessagePanel>();
        CreatePool();

    }
    public void PushAllPanel()
    {
        while (messageList.Count != 0) { 
            Push(messageList[0]);
        }
    }
    private void CreatePool()
    {
        for(int i = 0; i < 50; i++)
        {
            DiscordMessagePanel poolObj = Instantiate(messagePrefab, poolParents);
            messagePoolList.Add(poolObj);
            poolObj.Init();
            poolObj.gameObject.SetActive(false);
        }
    }

    public void Push(DiscordMessagePanel pushObj)
    {
        if(messageList.Contains(pushObj))
        {
            messageList.Remove(pushObj);
        }
        pushObj.gameObject.SetActive(false);
        pushObj.Release();
        messagePoolList.Add(pushObj);
    }

    private DiscordMessagePanel Pop()
    {
        if(messagePoolList.Count <= 0)
        {
            CreatePool();
        }

        DiscordMessagePanel popObj = messagePoolList[0];
        
        messagePoolList.Remove(popObj);
        messageList.Add(popObj);
        
        return popObj;
    }

    public void CreatePanel(DiscordChatData data, DiscordProfileDataSO opponentProfile)
    {
        DiscordMessagePanel messagePanel = Pop();
        opponentProfileData = opponentProfile;
        if (data.isMine)
        {
            messagePanel.SettingChatData(data, playerProfileData, CheckShowMsgPanelProfile(data));
        }
        else
        {
            if (opponentProfileData == null)
            {
                Debug.Log("opponentProfileData is null");
            }
            messagePanel.SettingChatData(data, opponentProfileData, CheckShowMsgPanelProfile(data));
        }
        messagePanel.transform.SetParent(MessageParent);
        messagePanel.gameObject.SetActive(true);
    }
    //talk 데이터 함수
    public IEnumerator WaitingTypingCoroutine(DiscordChatData data)
    {
        DiscordMessagePanel messagePanel = Pop();

        if (data.isMine)
        {
            inputChatingText.text = "...";
            yield return new WaitForSeconds(data.typingDelay);
            messagePanel.SettingChatData(data, playerProfileData, CheckShowMsgPanelProfile(data));
            inputChatingText.text = "";
        }
        else
        {
            stateText.text = $"{opponentProfileData.userName}님이 입력하고 있어요...";
            yield return new WaitForSeconds(data.typingDelay);
            messagePanel.SettingChatData(data, opponentProfileData, CheckShowMsgPanelProfile(data));
            stateText.text = "";
        }

        messagePanel.transform.SetParent(MessageParent);
        messagePanel.gameObject.SetActive(true);
        inputChatingText.text = "#채팅에 메세지 보내기";

    }

    private bool CheckShowMsgPanelProfile(DiscordChatData data)
    {
        if (messageList.Count <= 1)
        {
            return true;
        }

        DiscordMessagePanel lastMessage = messageList[messageList.Count- 2]; // 전메세지;
        if (lastMessage.ChatData.isMine != data.isMine)
        {
            return true;
        }
        //TimeSpan timeSpan = new TimeSpan(0, minutes: 5, 0);
        //if (lastMessage.ChatData.sendDateTime.Subtract(lastMessage.ChatData.sendDateTime) > timeSpan)
        //{
        //    return true;
        //}

        return false;
    }
}
