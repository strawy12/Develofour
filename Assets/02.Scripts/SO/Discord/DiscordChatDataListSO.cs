using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class DiscordChatData
{
    public string message;
    public int year;
    public int month;
    public int day;
    public int hour;
    public int minute;
    public bool isMine;
    public Sprite msgSprite;
    public float typingDelay;

    public string sendTimeText
    {
        get
        {
            string str = "";
            //windowManager랑 날짜 비교해서 오늘 어제는 그냥 오늘 어제로 적어야한다.
            if(hour > 12)
            {
                str += $"오후 {hour - 12}:{month}";
            }
            else if(hour == 12)
            {
                str += $"오후 {hour}:{month}";
            }
            else if(hour == 24)
            {
                str += $"오전 {hour - 12}:{month}";
            }
            else
            {
                str += $"오전 {hour}:{month}";
            }
            return str;
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
