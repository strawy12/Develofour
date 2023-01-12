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

        chattingPanel.PushAllPanel();
        foreach(DiscordChatData chatData in currentChatData.chatDataList)
        {
            chattingPanel.CreatePanel(chatData, currentChatData.opponentProfileData);
        }
    }
}
