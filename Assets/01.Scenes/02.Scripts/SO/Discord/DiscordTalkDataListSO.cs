using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordTalkDataListSO : ScriptableObject
{
    public DiscordProfileDataSO myProfileData;
    public DiscordProfileDataSO opponentProfileData;

    public List<DiscordChatData> chatDataList;
}
