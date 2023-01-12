using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class DiscordChatData
{
    public string message;
    [Header("���� ��¥")]
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
 
}

[CreateAssetMenu(menuName = "SO/Discord/ChatDataList")]
public class DiscordChatDataListSO : ScriptableObject
{
    public DiscordProfileDataSO myProfileData;
    public DiscordProfileDataSO opponentProfileData;

    public List<DiscordChatData> chatDataList;
}
