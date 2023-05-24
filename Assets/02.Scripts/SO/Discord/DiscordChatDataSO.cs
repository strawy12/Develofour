using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DiscordSendTime
{
    public int year = 2023;
    public int month;
    public int day;
}

[System.Serializable]
public class DiscordChatData
{
    [Multiline]
    public string message;

    public bool isMine;
    public GameObject msgSpritePrefab = null;

    [Header("정보 찾기")]
    public List<int> infoIDs;
    public int monologID;
    public List<int> needInformaitonList;
}
[CreateAssetMenu(menuName = "SO/Discord/ChatData")]
public class DiscordChatDataSO : ScriptableObject
{
    public DiscordSendTime chatDay;
    public List<DiscordChatData> chatDatas;
}
