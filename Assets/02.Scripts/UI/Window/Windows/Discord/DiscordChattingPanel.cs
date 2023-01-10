using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DiscordChattingPanel : MonoBehaviour
{
    [SerializeField]
    private DiscordMessagePanel messagePrefab; 

    [SerializeField]
    private Transform poolParents;

    private List<DiscordMessagePanel> messagePoolList;
    private List<DiscordMessagePanel> messageList;

    public DiscordProfileDataSO playerProfileData;
    public DiscordProfileDataSO opponentProfileData;

    private void CreatePool()
    {
        for(int i = 0; i < 50; i++)
        {
            DiscordMessagePanel poolObj = Instantiate(messagePrefab, poolParents);
            messagePoolList.Add(poolObj);
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
        messagePoolList.Add(pushObj);
    }

    private DiscordMessagePanel Pop()
    {
        if(messagePoolList.Count <= 0)
        {
            for (int i = 0; i < 50; i++)
            {
                DiscordMessagePanel poolObj = Instantiate(messagePrefab, poolParents);
                messagePoolList.Add(poolObj);
                poolObj.gameObject.SetActive(false);
            }
        }

        DiscordMessagePanel popObj = messagePoolList[0];
        
        messagePoolList.Remove(popObj);
        messageList.Add(popObj);
        
        return popObj;
    }

    public DiscordMessagePanel CreatePanel(DiscordChatData data)
    // 얻어온 데이터를 쓸 Panel을 만들음
    {
        DiscordMessagePanel messagePanel = Pop();

        if(data.isMine)
        {
            messagePanel.SettingChatData(data, playerProfileData);
        }
        else
        {
            messagePanel.SettingChatData(data, opponentProfileData);

        }

        return messagePanel;
    }
}
