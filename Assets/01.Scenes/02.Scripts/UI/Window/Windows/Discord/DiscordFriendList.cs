using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiscordFriendList : MonoBehaviour
{
    [SerializeField]
    public List<DiscordProfileDataSO> FriendList;

    [SerializeField]
    private DiscordFriendLine friendLinePrefab;

    void Start()
    {
        foreach(var friend in FriendList)
        {
            if(friend.isFriend)
            {
                

            }
        }
    }

    private void CreateFriendLine(FacebookFriendDataSO data)
    {
        DiscordFriendLine friendLine = Instantiate(friendLinePrefab, this.transform);

    }
}
