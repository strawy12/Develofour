using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/Site/Facebook/FriendDataSO")]
public class FacebookFriendDataSO : ScriptableObject
{
    public bool isFriend = false;
    public Sprite backgroundImage;
    public Sprite profileImage;
    public string nameText;
    public string infoText;
    public List<FacebookPidPanelDataSO> pidList;    
}
