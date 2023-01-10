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

    public void CreatePanel(DiscordChatData data)
    {
        DiscordMessagePanel messagePanel = Pop();
        if (data.isMine)
        {
            messagePanel.SettingChatData(data, playerProfileData);
        }
        else
        {
            messagePanel.SettingChatData(data, opponentProfileData);
        }
        messagePanel.transform.SetParent(MessageParent);
        messagePanel.gameObject.SetActive(true);
    }

    public IEnumerator WaitingTypingCoroutine(DiscordChatData data)
    {
        DiscordMessagePanel messagePanel = Pop();

        if (data.isMine)
        {
            inputChatingText.text = "...";
            yield return new WaitForSeconds(data.typingDelay);
            messagePanel.SettingChatData(data, playerProfileData);
            inputChatingText.text = "";
        }
        else
        {
            stateText.text = $"{opponentProfileData.userName}님이 입력하고 있어요...";
            yield return new WaitForSeconds(data.typingDelay);
            messagePanel.SettingChatData(data, opponentProfileData);
            stateText.text = "";
        }
        messagePanel.transform.SetParent(MessageParent);
        messagePanel.gameObject.SetActive(true);

    }
}
