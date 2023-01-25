using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Discord : Window
{
    [SerializeField]
    private List<DiscordChatDataListSO> chatDataList; // 대화 한 내역
    [SerializeField]
    private List<DiscordTalkDataListSO> talkDataList; // 대화 할 내역

    private DiscordChatDataListSO currentChatData;
    private DiscordTalkDataListSO currentTalkData;

    private string currentUserName; // 현재 대화 중인 상대 닉네임

    [SerializeField]
    private DiscordChattingPanel chattingPanel;

    public DiscordFriendList friendList;

    [SerializeField]
    private DiscordMessageImagePanel imagePanel;

    [SerializeField]
    private DiscordLogin discordLogin;

    private void Start()

    {
        Debug.Log("디스코드 디버그용 스타트. ");
        Init();
    }

    protected override void Init()
    {
        base.Init();

        EventManager.StartListening(EDiscordEvent.ShowChattingPanel, SettingChattingPanel);
        EventManager.StartListening(EDiscordEvent.StartTalk, StartTalkChat);
        friendList.Init();
        discordLogin.Init();
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
            Debug.LogWarning("userName을 찾을 수 없습니다.");
        }
        return newChatData;
    }


    public void SettingChattingPanel(object[] param)
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
            chattingPanel.StartTalk(currentTalkData);
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

    private void OnDestroy()
    {
        EventManager.StopListening(EDiscordEvent.ShowChattingPanel, SettingChattingPanel);
        EventManager.StopListening(EDiscordEvent.StartTalk, StartTalkChat);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EDiscordEvent.ShowChattingPanel, SettingChattingPanel);
        EventManager.StopListening(EDiscordEvent.StartTalk, StartTalkChat);
    }
#if UNITY_EDITOR
    private void OnApplicationQuit()
    {
        foreach (DiscordTalkDataListSO talkList in talkDataList)
        {
            talkList.Reset();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            object[] ps = new object[1] { "테스트" };

            StartTalkChat(ps);

        }
    }
#endif
}
