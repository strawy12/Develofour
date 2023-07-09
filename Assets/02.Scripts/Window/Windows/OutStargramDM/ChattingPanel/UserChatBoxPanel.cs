using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserChatBoxPanel : MonoBehaviour
{
    [SerializeField]
    private ChatBox chatBoxTemp;

    [SerializeField]
    private RectTransform boxParent;
    [SerializeField]
    private ClickInfoTrigger clickInfoTriggerTemp;
    private Transform boxPoolParent;
    [SerializeField]
    private float offset = 5f;
    [SerializeField]
    private float spacing;
    [SerializeField]
    private int poolCnt;
    private Queue<ChatBox> chatBoxQueue;
    private List<ChatBox> chatBoxList;

    private OutStarChatDataSO chatData;

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

    public void AddChatBox(OutStarChatDataSO data)
    {
        chatData = data;
        ChatBox chatBox = Pop();
        chatBox.gameObject.SetActive(true);
        List<TextTriggerData> triggerDataList = new List<TextTriggerData>();

        if(data.outStarTriggerList != null && data.outStarTriggerList.Count != 0)
        {
            foreach (var outStarTrigger in data.outStarTriggerList)
            {
                TriggerDataSO triggerData = ResourceManager.Inst.GetTriggerDataSOResources(outStarTrigger.triggerID);
                if (triggerData != null)
                {
                    ClickInfoTrigger clickInfoTrigger = Instantiate(clickInfoTriggerTemp, chatBox.ChatText.transform);
                    RectTransform triggerRect = (RectTransform)clickInfoTrigger.transform;
                    triggerRect.anchorMin = new Vector2(0, 0.5f);
                    triggerRect.anchorMax = new Vector2(0, 0.5f);
                    triggerRect.pivot = new Vector2(0, 1f);
                    TextTriggerData textTriggerData = new TextTriggerData() { startIdx = outStarTrigger.startIdx, trigger = clickInfoTrigger, endIdx = outStarTrigger.endIdx };
                    clickInfoTrigger.Setting(triggerData);
                    clickInfoTrigger.gameObject.SetActive(true);
                    triggerDataList.Add(textTriggerData);
                }
            }
        }
        
        chatBox.Setting(data.chatText);
        
        Define.SetTriggerPosition(chatBox.ChatText, triggerDataList);
        Define.SetTiggerSize(chatBox.ChatText, triggerDataList);

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
