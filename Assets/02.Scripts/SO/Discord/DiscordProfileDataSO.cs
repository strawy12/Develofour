using UnityEngine;


[CreateAssetMenu(menuName = "SO/Discord/ProfileData")]
public class DiscordProfileDataSO : ScriptableObject
{
    public string userName;
    public Sprite userSprite;
    public string statusMsg;
    public string infoMsg;
    public bool isFriend;
    public int overlayID;
}

