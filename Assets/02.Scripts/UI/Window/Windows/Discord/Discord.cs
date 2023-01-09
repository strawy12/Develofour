using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Discord : Window
{
    List<DiscordChatDataListSO> chatDataList; // ��ȭ �� ����
    List<DiscordTalkDataListSO> talkDataList; // ��ȭ �� ����

    private DiscordChatDataListSO currentChatData;
    private DiscordTalkDataListSO currentTalkData;
    
    private string currentUserName; // ���� ��ȭ ���� ��� �г���

    [SerializeField]
    private DiscordChattingPanel chattingPanel;

    DiscordChatDataListSO GetChatDataList(string userName) 
    {
        foreach(DiscordChatDataListSO chatData in chatDataList)
        {
            if(userName == chatData.opponentProfileData.userName)
            {
                currentChatData = chatData;
            }
        }
        return currentChatData;
    }

    void SettingChattingPanel(string userName)
    {
        currentUserName = userName;

    }
}
