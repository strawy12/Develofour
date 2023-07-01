using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserChatBoxPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject chatBoxTemp;

    [SerializeField]
    private Transform boxParent;

    public void Init()
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
