using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatBoxPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject chatBoxTemp;

    [SerializeField]
    private Transform boxParent;

    private void Init()
    {
        boxParent = transform.Find("");
    }

    public void AddChatBox(string text)
    {
        
    }

    public void SetSize()
    {

    }
}
