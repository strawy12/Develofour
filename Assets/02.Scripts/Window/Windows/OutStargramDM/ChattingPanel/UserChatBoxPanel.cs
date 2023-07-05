using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserChatBoxPanel : MonoBehaviour
{
    [SerializeField]
    private ChatBox chatBoxTemp;

    [SerializeField]
    private RectTransform boxParent;

    private Transform boxPoolParent;
    [SerializeField]
    private float offset = 5f;
    [SerializeField]
    private float spacing;
    [SerializeField]
    private int poolCnt;
    private Queue<ChatBox> chatBoxQueue;
    private List<ChatBox> chatBoxList;

    public bool isMine;
    #region Pool
    private void CreateChatBox()
    {
        for(int i = 0; i < poolCnt; i++)
        {
            ChatBox chatBox = Instantiate(chatBoxTemp, boxPoolParent);
            chatBox.gameObject.SetActive(false);
            chatBox.transform.SetParent(boxPoolParent);
            chatBox.Release();
            chatBoxQueue.Enqueue(chatBox);
        }
    }

    private ChatBox Pop()
    {
        if(chatBoxQueue.Count == 0)
        {
            CreateChatBox();
        }
        ChatBox chatBox = chatBoxQueue.Dequeue();
        chatBox.transform.SetParent(boxParent);
        chatBoxList.Add(chatBox);
        return chatBox;
    }
    
    private void Push(ChatBox chatBox)
    {
        chatBoxList.Remove(chatBox);
        chatBox.gameObject.SetActive(false);
        chatBox.Release();
        chatBoxQueue.Enqueue(chatBox);
    }

    private void PushAll()
    {
        while(chatBoxList.Count != 0)
        {
            Push(chatBoxList[0]);
        }
    }
    #endregion
    public void Init(Transform poolParent)
    {
        chatBoxList = new List<ChatBox>();
        chatBoxQueue = new Queue<ChatBox>();
        boxPoolParent = poolParent;
        CreateChatBox();
    }
    public void Setting()
    {
        SetSize();
    }

    public void AddChatBox(string text)
    {
        ChatBox chatBox = Pop();
        chatBox.gameObject.SetActive(true);
        chatBox.Setting(text);
        SetSize();
    }

    public void SetSize()
    {
        float totalY = offset - spacing;
        foreach (var box in chatBoxList)
        {
            RectTransform boxRectTrm = (RectTransform)box.transform;
            totalY += boxRectTrm.sizeDelta.y + spacing;
        }
        boxParent.sizeDelta = new Vector2(boxParent.sizeDelta.x, totalY);
        RectTransform rectTransform = (RectTransform)transform;
        rectTransform.sizeDelta = boxParent.sizeDelta;
    }

    public void Release()
    {
        PushAll();

    }
}
