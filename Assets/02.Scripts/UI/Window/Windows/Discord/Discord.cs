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

    protected override void Init()
    {
        base.Init();
    }

    public void Update()
    {
        Debug.LogWarning("����� �ڵ�");
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
            Debug.LogWarning("userName�� ã�� �� �����ϴ�.");
        }
        return newChatData;
    }

    public void SettingChattingPanel(string userName)
        // ä���� �ϰ� �ִ� ����� �ٲ�
    {
        currentUserName = userName;
        currentChatData = GetChatDataList(currentUserName);

        foreach(DiscordChatData chatData in currentChatData.chatDataList)
        {
            chattingPanel.CreatePanel(chatData, currentChatData.opponentProfileData);
        }
    }
}
