using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="SO/DiscordProfileDataSO")]
public class DiscordProfileDataSO : ScriptableObject
{
    public string userName;
    public Sprite userSprite;
    public string statusMsg;
    public string infoMsg;
    public bool isFriend;
}
