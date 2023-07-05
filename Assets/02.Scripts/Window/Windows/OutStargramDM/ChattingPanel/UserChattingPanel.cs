using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserChattingPanel : MonoBehaviour
{
    [SerializeField]
    private UserChatBoxPanel myChatBoxPanelTemp;

    [SerializeField]
    private UserChatBoxPanel otherChatBoxPanelTemp;

    [SerializeField]
    private Transform boxPoolParent;
    [SerializeField]
    private Transform panelPoolParent;
    [SerializeField]
    private int poolCnt;
    private OutStarCharacterDataSO userData;
    
    private List<UserChatBoxPanel> myChatBoxPanelList;
    private List<UserChatBoxPanel> otherChatBoxPanelList;
    private Queue<UserChatBoxPanel> myChatBoxPanelQueue;
    private Queue<UserChatBoxPanel> otherChatBoxPanelQueue;

    [SerializeField]
    private TMP_Text timeTextTemp;
    private List<TMP_Text> timeTextList;


    public void Init()
    {
        myChatBoxPanelQueue = new Queue<UserChatBoxPanel>();
        otherChatBoxPanelQueue = new Queue<UserChatBoxPanel>();
        myChatBoxPanelList = new List<UserChatBoxPanel>();
        otherChatBoxPanelList = new List<UserChatBoxPanel>();
        timeTextList = new List<TMP_Text>();

        CreatePool();
    }

    public void ChangeUserData(OutStarCharacterDataSO data)
    {
        userData = data;
        Setting();
    }

    private void Setting()
    {
        PushAll();
        while (timeTextList.Count != 0)
        {
            TMP_Text timeText = timeTextList[0];
            timeTextList.Remove(timeText);
            Destroy(timeText.gameObject);
        }
        string lastMine = "";

        foreach (var id in userData.timeChatIDList)
        {
            OutStarTimeChatDataSO timeChat = ResourceManager.Inst.GetOutStarTimeChatResourceManager(id);
            string timeText = Define.GetOutStarTimeText(timeChat.time);
            TMP_Text timeTextObj = Instantiate(timeTextTemp, transform);
            timeTextObj.SetText(timeText);
            timeTextObj.gameObject.SetActive(true);
            timeTextList.Add(timeTextObj);

            UserChatBoxPanel currentPanel = null;

            foreach (var chatId in timeChat.chatDataIDList)
            {
                OutStarChatDataSO chatData = ResourceManager.Inst.GetOutStarChatResourceManager(chatId); // Resource
                if (lastMine == "" || lastMine != chatData.isMine.ToString())
                {
                    currentPanel = Pop(chatData.isMine);
                }
                currentPanel.AddChatBox(chatData.chatText);
            }
        }
    }

    #region Pool
    private void CreatePool()
    {
        for (int i = 0; i < poolCnt; i++)
        {
            UserChatBoxPanel myChatBoxPanel = Instantiate(myChatBoxPanelTemp, panelPoolParent);
            myChatBoxPanel.transform.SetParent(panelPoolParent);
            myChatBoxPanel.Init(boxPoolParent);
            myChatBoxPanel.gameObject.SetActive(false);
            myChatBoxPanelQueue.Enqueue(myChatBoxPanel);

            UserChatBoxPanel otherChatBoxPanel = Instantiate(otherChatBoxPanelTemp, panelPoolParent);
            otherChatBoxPanel.transform.SetParent(panelPoolParent);
            otherChatBoxPanel.Init(boxPoolParent);
            otherChatBoxPanel.gameObject.SetActive(false);
            otherChatBoxPanelQueue.Enqueue(otherChatBoxPanel);
        } 
    }
    private UserChatBoxPanel Pop(bool isMine)
    {
        UserChatBoxPanel chatBoxPanel = null;
        if(isMine)
        {
            if(myChatBoxPanelQueue.Count <= 0)
            {
                CreatePool();
            }
            chatBoxPanel = myChatBoxPanelQueue.Dequeue();
            myChatBoxPanelList.Add(chatBoxPanel);
        }
        else
        {
            if (otherChatBoxPanelQueue.Count <= 0)
            {
                CreatePool();
            }
            chatBoxPanel = otherChatBoxPanelQueue.Dequeue();
            otherChatBoxPanelList.Add(chatBoxPanel);
        }
        chatBoxPanel.transform.SetParent(transform);
        chatBoxPanel.gameObject.SetActive(true);
        return chatBoxPanel;
    }

    private void Push(UserChatBoxPanel chatBoxPanel)
    {
        if(chatBoxPanel.isMine)
        {
            myChatBoxPanelList.Remove(chatBoxPanel);
            chatBoxPanel.Release();
            chatBoxPanel.gameObject.SetActive(false);
            myChatBoxPanelQueue.Enqueue(chatBoxPanel);
        }
        else
        {
            otherChatBoxPanelList.Remove(chatBoxPanel);
            chatBoxPanel.Release();
            chatBoxPanel.gameObject.SetActive(false);
            otherChatBoxPanelQueue.Enqueue(chatBoxPanel);
        }
        chatBoxPanel.transform.SetParent(panelPoolParent);
    }

    private void PushAll()
    {
        while(myChatBoxPanelList.Count != 0)
        {
            Push(myChatBoxPanelList[0]);
        }
        while(otherChatBoxPanelList.Count != 0)
        {
            Push(otherChatBoxPanelList[0]);
        }

    }
    #endregion

}
