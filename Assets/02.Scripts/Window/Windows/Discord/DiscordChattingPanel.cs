using System;
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
    private Transform poolParent;
    [SerializeField]
    private TMP_Text stateText;
    [SerializeField]
    private TMP_Text inputChatingText;
    private List<DiscordMessagePanel> messagePoolList;
    private List<DiscordMessagePanel> messageList;

    private Coroutine currentTalkCoroutine;
    private bool isInputed = false;
    [HideInInspector]
    public DiscordProfileDataSO playerProfileData;
    [HideInInspector]
    public DiscordProfileDataSO opponentProfileData;
    [SerializeField]
    private DiscordFriendList friendList;

    [SerializeField]
    private DiscordImagePanel clickImagePanel;

    public void Init()
    {
        messagePoolList = new List<DiscordMessagePanel>();
        messageList = new List<DiscordMessagePanel>();
        CreatePool();

        clickImagePanel.Init();
    }

    #region Pooling
    public void PushAllPanel()
    {
        while (messageList.Count != 0)
        {
            Push(messageList[0]);
        }
    }
    private void CreatePool()
    {
        for (int i = 0; i < 50; i++)
        {
            DiscordMessagePanel poolObj = Instantiate(messagePrefab, poolParent);
            poolObj.transform.SetParent(poolParent);
            messagePoolList.Add(poolObj);
            poolObj.Init();
            poolObj.gameObject.SetActive(false);
        }
    }
    public void Push(DiscordMessagePanel pushObj)
    {
        if (messageList.Contains(pushObj))
        {
            messageList.Remove(pushObj);
        }
        pushObj.gameObject.SetActive(false);
        pushObj.transform.SetParent(poolParent);
        pushObj.Release();
        messagePoolList.Add(pushObj);
    }
    private DiscordMessagePanel Pop()
    {
        if (messagePoolList.Count <= 0)
        {
            CreatePool();
        }

        DiscordMessagePanel popObj = messagePoolList[0];
        popObj.transform.SetParent(MessageParent);

        messagePoolList.Remove(popObj);
        messageList.Add(popObj);

        return popObj;
    }
    #endregion
    #region ä�� ����
    public void CreatePanel(DiscordChatData data, DiscordProfileDataSO opponentProfile)
    {
        DiscordMessagePanel messagePanel = Pop();
        opponentProfileData = opponentProfile;
        if (data.isMine)
        {
            messagePanel.SettingChatData(data, playerProfileData, CheckShowMsgPanelProfile(data));
        }
        else
        {
            if (opponentProfileData == null)
            {
                Debug.Log("opponentProfileData is null");
            }
            messagePanel.SettingChatData(data, opponentProfileData, CheckShowMsgPanelProfile(data));
        }
        messagePanel.gameObject.SetActive(true);
    }

    public void StartTalk(DiscordTalkDataListSO talkList)
    {
        opponentProfileData = talkList.opponentProfileData;
        StartCoroutine(TalkCoroutine(talkList));

    }
    public IEnumerator TalkCoroutine(DiscordTalkDataListSO talkList)
    {
        foreach (DiscordChatData chatData in talkList.chatDataList)
        {
            if (chatData.isTalked) continue;

            isInputed = true;
            WaitingTyping(chatData);
            yield return new WaitForSeconds(chatData.typingDelay);
            TalkChat(chatData);
            yield return new WaitUntil(() => isInputed == false);
        }
        talkList.isCoimpleteTalk = true;
        yield break;
    }
    public void StopTalk()
    {
        StopCoroutine(currentTalkCoroutine);

        stateText.text = "";
        isInputed = false;
    }
    private void TalkChat(DiscordChatData data)
    {

        if (friendList.CurrentFriendLine == null || friendList.CurrentFriendLine.myData != opponentProfileData)
        {
            friendList.NewMessage(opponentProfileData);

            NoticeSystem.OnNotice.Invoke(opponentProfileData.userName, data.message, 0, true, opponentProfileData.userSprite, Color.white, ENoticeTag.Discord);
        }
        else
        {
            DiscordMessagePanel messagePanel = Pop();
            if (data.isMine)
            {
                messagePanel.SettingChatData(data, playerProfileData, CheckShowMsgPanelProfile(data));
            }
            else
            {
                messagePanel.SettingChatData(data, opponentProfileData, CheckShowMsgPanelProfile(data));
            }
            messagePanel.gameObject.SetActive(true);
        }
        //end
        data.isTalked = true;
        isInputed = false;
        inputChatingText.text = "#ä�ÿ� �޼��� ������";

        stateText.text = "";
    }
    public void WaitingTyping(DiscordChatData data)
    {
        if (friendList.CurrentFriendLine == null) return;
        if (friendList.CurrentFriendLine.myData != opponentProfileData) return;
        if (data.isMine)
        {
            inputChatingText.text = "...";
        }
        else
        {
            stateText.text = $"{opponentProfileData.userName}���� �Է��ϰ� �־��...";
        }

    }
    private bool CheckShowMsgPanelProfile(DiscordChatData data)
    {
        if (messageList.Count <= 1)
        {
            return true;
        }

        DiscordMessagePanel lastMessage = messageList[messageList.Count - 2]; // ���޼���;
        if (lastMessage.ChatData.isMine != data.isMine)
        {
            lastMessage.AutoSettingMessagePanelSize(true);
            return true;
        }

        return false;
    }
    #endregion
}
