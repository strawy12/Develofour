using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Discord/TalkDataList")]
public class DiscordTalkDataListSO : ScriptableObject
{
    public DiscordProfileDataSO myProfileData;
    public DiscordProfileDataSO opponentProfileData;

    public bool isCoimpleteTalk = false;
    public List<DiscordChatData> chatDataList;
}
