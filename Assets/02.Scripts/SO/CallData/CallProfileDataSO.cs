using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallProfileDataSO : ScriptableObject
{
    [SerializeField]
    private string chatacterID = "";

    public string monologID;

    // 전화를 받지 않을 경우의 통화 ID
    public string notExistCallID;

    public float delay;

    public List<string> outGoingCallIDList;
    public List<string> inCommingCallIDList;
    public List<string> returnCallIDList;

    public string CharacterID
    {
        get { return chatacterID; }
        set
        {
            if (!string.IsNullOrEmpty(chatacterID))
                return;

            chatacterID = value;
        }
    }
}
