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

    protected override void Init()
    {
        base.Init();
    }

    public void Update()
    {
        Debug.LogWarning("디버그 코드");
        if (Input.GetKeyDown(KeyCode.J))
        {
            SettingChattingPanel(chatDataList[0].opponentProfileData.userName);
        }
    }

    public DiscordChatDataListSO GetChatDataList(string userName)
    {
        DiscordChatDataListSO newChatData = null;
        foreach (DiscordChatDataListSO chatDataList in chatDataList)
        {
            if(userName == chatDataList.opponentProfileData.userName)
            {
                newChatData = chatDataList;
            }
        }
        if(newChatData == null)
        {
            Debug.LogWarning("userName을 찾을 수 없습니다.");
        }
        return newChatData;
    }

    public void SettingChattingPanel(string userName)
        // 채팅을 하고 있는 대상을 바꿈
    {
        currentUserName = userName;
        currentChatData = GetChatDataList(currentUserName);

        foreach(DiscordChatData chatData in currentChatData.chatDataList)
        {
            chattingPanel.CreatePanel(chatData, currentChatData.opponentProfileData);
        }
    }
}
