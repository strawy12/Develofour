﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Discord : Window
{
    public static Discord currentDiscord;
    [SerializeField]
    private List<DiscordChatDataListSO> chatDataList; // 대화 한 내역
    [SerializeField]
    private List<DiscordTalkDataListSO> talkDataList; // 대화 할 내역

    private DiscordChatDataListSO currentChatData;
    private DiscordTalkDataListSO currentTalkData;

    private string currentUserName; // 현재 대화 중인 상대 닉네임

    [SerializeField]
    private TMP_Text chatRoomNameText;

    [SerializeField]
    private DiscordChattingPanel chattingPanel;

    public DiscordFriendList friendList;

    [SerializeField]
    private DiscordMessageImagePanel imagePanel;

    [SerializeField]
    private DiscordLogin discordLogin;

    private HarmonyShortcutDataSO chatData;

    protected override void Init()
    {
        base.Init();
        EventManager.StartListening(EDiscordEvent.ShowChattingPanel, SettingChattingPanel);
        EventManager.StartListening(EDiscordEvent.StartTalk, StartTalkChat);

        OnSelected += SelectedDiscord;

        OnClosed += (a) => ResetDiscord();

        friendList.Init();
        chattingPanel.Init();
        if (!DataManager.Inst.GetIsLogin(ELoginType.Harmony))
        {
            discordLogin.Init();
        }
        else
        {
            discordLogin.gameObject.SetActive(false);
        }
        currentDiscord = this;
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
        chatRoomNameText.text = "@ " + currentUserName;
        currentChatData = GetChatDataList(currentUserName);
        currentTalkData = GetTalkDataList(currentUserName);
        chattingPanel.PushAllPanel();
        chattingPanel.playerProfileData = currentChatData.myProfileData;
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

    public void OpenChattingRoom(string name)
    {
        friendList.friendLineDic[name].OnLeftClickPanel?.Invoke(friendList.friendLineDic[name]);
    }

    public void ResetDiscord()
    {
        currentDiscord = null;
    }

    public void SelectedDiscord()
    {
        currentDiscord = this;
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EDiscordEvent.ShowChattingPanel, SettingChattingPanel);
        EventManager.StopListening(EDiscordEvent.StartTalk, StartTalkChat);
    }

    protected override void OnDisable()
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
