using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserChatBoxPanel : MonoBehaviour
{
    [SerializeField]
    private ChatBox chatBoxTemp;

    [SerializeField]
    private Transform boxParent;

    private List<ChatBox> chatBoxList;
    private List<string> chatTextList;
    public void Init(List<string> chatDataList)
    {
        boxParent = transform.Find("");
        chatBoxList = new List<ChatBox>();
        this.chatTextList = chatDataList;

        CreateChatBox();
    }

    private void CreateChatBox()
    {
        foreach(var text in chatTextList)
        {
            AddChatBox(text);
        }

        SetSize();
    }

    private void AddChatBox(string text)
    {
        ChatBox chatBox = Instantiate(chatBoxTemp, boxParent);
        chatBox.Init(text);
        chatBoxList.Add(chatBox);
    }

    public void SetSize()
    {
        foreach (var text in chatTextList)
        {
            
        }
    }
}
