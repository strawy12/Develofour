using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Discord : Window
{
    [SerializeField]
    private List<DiscordChatDataListSO> chatDataList; // ��ȭ �� ����
    [SerializeField]
    private List<DiscordTalkDataListSO> talkDataList; // ��ȭ �� ����

    private DiscordChatDataListSO currentChatData;
    private DiscordTalkDataListSO currentTalkData;

    private string currentUserName; // ���� ��ȭ ���� ��� �г���

    [SerializeField]
    private DiscordChattingPanel chattingPanel;

    public DiscordFriendList friendList;

    void Start()
    {
        Init();
    }

    protected override void Init()
    {
        base.Init();
        EventManager.StartListening(EDiscordEvent.ShowChattingPanel, SettingChattingPanel);
        friendList.Init();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {

            currentTalkData = GetTalkDataList("�׽�Ʈ");

            chattingPanel.StartTalk(currentTalkData);
        }
    }
    public DiscordChatDataListSO GetChatDataList(string userName)
    {
        DiscordChatDataListSO newChatData = null;
        foreach (DiscordChatDataListSO chatDataList in chatDataList)
        {
            if (userName == chatDataList.opponentProfileData.userName)
            {
                newChatData = chatDataList;
            }
        }
        if (newChatData == null)
        {
            Debug.LogWarning("userName�� ã�� �� �����ϴ�.");
        }
        return newChatData;
    }

    public void SettingChattingPanel(object[] param)
    // ä���� �ϰ� �ִ� ����� �ٲ�
    {

        if (!(param[0] is string) || param[0] == null) return;
        string userName = param[0] as string;

        currentUserName = userName;
        currentChatData = GetChatDataList(currentUserName);
        currentTalkData = GetTalkDataList(currentUserName);
        chattingPanel.PushAllPanel();

        if (currentChatData != null)
        {
            foreach (DiscordChatData chatData in currentChatData.chatDataList)
            {
                chattingPanel.CreatePanel(chatData, currentChatData.opponentProfileData);
            }
        }
        if (currentTalkData != null)
        {
            foreach (DiscordChatData talkData in currentTalkData.chatDataList)
            {
                if (talkData.isTalked)
                    chattingPanel.CreatePanel(talkData, currentChatData.opponentProfileData);
            }
        }
    }

    public void StartTalkChat(object[] param)
    {
        if (!(param[0] is string) || param[0] == null) return;

        string userName = param[0] as string;



        currentTalkData = GetTalkDataList(userName);

        if (!currentTalkData.isCoimpleteTalk)
        {
            if (userName == currentUserName)
            {
                chattingPanel.StartTalk(currentTalkData);
            }
            else
            {
                
            }
        }
    }

    private DiscordTalkDataListSO GetTalkDataList(string userName)
    {
        foreach (DiscordTalkDataListSO talkList in talkDataList)
        {
            if (talkList.opponentProfileData.userName == userName)
            {
                return talkList;
            }
        }
        return null;
    }

    public void StopTalk(object[] param)
    {
        chattingPanel.StopTalk();
    }

#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        foreach (DiscordTalkDataListSO talkList in talkDataList)
        {
            talkList.Reset();
        }
    }
#endif
}
