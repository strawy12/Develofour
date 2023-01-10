using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DiscordFriendList : MonoBehaviour
{
    //[SerializeField]
    //public Dictionary<DiscordProfileDataSO, DiscordFriendLine> FriendDictionary;

    [SerializeField]
    private List<DiscordProfileDataSO> friendList;

    [SerializeField]
    private DiscordFriendLine friendLinePrefab;

    private DiscordFriendLine beforeFriendLine = null; //과거 좌클릭한 라인
    private DiscordFriendLine currentFriendLine = null; //현재 좌클릭한 라인
    private DiscordFriendLine AttributeFriendLine = null; //우클릭 한 라인

    [SerializeField]    
    private GameObject attribuePanel;

    void Start()
    {
        foreach(var friend in friendList)
        {
            if(friend.isFriend)
            {
                CreateFriendLine(friend);
            }
        }
    }

    private void CreateFriendLine(DiscordProfileDataSO data)
    {
        DiscordFriendLine friendLine = Instantiate(friendLinePrefab, this.transform);

        friendLine.OnLeftClickPanel += LeftClickLine;
        friendLine.OnRightClickPanel += RightClickLine;

        friendLine.ProfileImage.sprite = data.userSprite;
        if(data.statusMsg == string.Empty)
        {
            friendLine.NameText.text = data.userName; 
        }
        else
        {
            friendLine.NameText.gameObject.SetActive(false);
            friendLine.StatusNameText.gameObject.SetActive(true);
            friendLine.StatusText.gameObject.SetActive(true);
            friendLine.StatusNameText.text = data.userName;
            friendLine.StatusText.text = data.statusMsg;
        }

        //FriendDictionary[data] = friendLine;
        friendLine.gameObject.SetActive(true);
    }

    public void LeftClickLine(DiscordFriendLine line)
    {
        beforeFriendLine = currentFriendLine;
        if (beforeFriendLine != null && beforeFriendLine != line)
        {
            beforeFriendLine.QuitSelectPanel();
        }
        currentFriendLine = line;
        //클릭됬을때 해당 메세지창 보여주면 됨
    }

    public void RightClickLine(Vector2 pos, DiscordFriendLine line)
    {
        AttributeFriendLine = line;
        attribuePanel.transform.position = pos;
        attribuePanel.gameObject.SetActive(true);
    }

    public void NewMessage()
    {

        //세로운 메세지 왔을때 노티스패널 키고 맨 위로 올리는 함수 
    }

}
