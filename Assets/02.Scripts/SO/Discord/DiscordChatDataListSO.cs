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
            //windowManager�� ��¥ ���ؼ� ���� ������ �׳� ���� ������ ������Ѵ�.
            if(hour > 12)
            {
                str += $"���� {hour - 12}:{month}";
            }
            else if(hour == 12)
            {
                str += $"���� {hour}:{month}";
            }
            else if(hour == 24)
            {
                str += $"���� {hour - 12}:{month}";
            }
            else
            {
                str += $"���� {hour}:{month}";
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
