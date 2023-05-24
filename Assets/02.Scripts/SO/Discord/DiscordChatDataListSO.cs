using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class DiscordSendTime
{
    public int year = 2023;
    public int month = 10;
    public int day = 23;
    public int time;
    public int minute;
}
    
[System.Serializable]
public class DiscordChatData
{
    [Multiline]
    public string message;
    [Header("보낸 날짜")]
    public DiscordSendTime sendTime;

    public bool isMine;
    public GameObject msgSpritePrefab = null;
    public float typingDelay;

    public bool isTalked = false;

    public void Reset()
    {
        isTalked = false;
    }

    [Header("정보 찾기")]
    public int infoID;
    public int monologID;
    public List<int> needInformaitonList;

}

[CreateAssetMenu(menuName = "SO/Discord/ChatDataList")]
public class DiscordChatDataListSO : ScriptableObject
{
    public DiscordProfileDataSO myProfileData;
    public DiscordProfileDataSO opponentProfileData;

    public List<DiscordChatData> chatDataList;
    public void Reset()
    {
        foreach (DiscordChatData chatData in chatDataList)
        {
            chatData.Reset();
        }
    }
}
