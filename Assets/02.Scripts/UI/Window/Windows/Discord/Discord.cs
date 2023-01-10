using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Discord : Window
{
    List<DiscordChatDataListSO> chatDataList; // 대화 한 내역
    List<DiscordTalkDataListSO> talkDataList; // 대화 할 내역

    private DiscordChatDataListSO currentChatData;
    private DiscordTalkDataListSO currentTalkData;
    
    private string currentUserName; // 현재 대화 중인 상대 닉네임

    [SerializeField]
    private DiscordChattingPanel chattingPanel;

    DiscordChatDataListSO GetChatDataList(string userName)
        // 채팅 데이터를 얻어옴
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
        // 채팅을 하고 있는 대상을 바꿈
    {
        currentUserName = userName;

    }
}
