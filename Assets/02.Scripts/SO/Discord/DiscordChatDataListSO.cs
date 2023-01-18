using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class DiscordChatData
{
    [Multiline]
    public string message;
    [Header("보낸 날짜")]
    public int year;
    public int month;
    public int day;

    [Header("")]
    public bool isMine;
    public Sprite msgSprite;
    public float typingDelay;

    public DateTime sendDateTime
    {
        get
        {
            return new DateTime(year, month, day);
        }
    }
    public bool isTalked = false;

    public void Reset()
    {
        isTalked = false;
    }

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
