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
    private Dictionary<string, DiscordFriendLine> friendLineDic = new Dictionary<string, DiscordFriendLine>();

    [SerializeField]
    private DiscordFriendLine friendLinePrefab;

    private DiscordFriendLine beforeFriendLine = null; //과거 좌클릭한 라인
    private DiscordFriendLine currentFriendLine = null; //현재 좌클릭한 라인
    private DiscordFriendLine AttributeFriendLine = null; //우클릭 한 라인

    [SerializeField]    
    private GameObject attribuePanel;

    [SerializeField]
    private DiscordArea discordArea;

    public void Init()
    {
        Debug.Log("실행됨");
        foreach (var friend in friendList)
        {
            if (friend.isFriend)
            {
                friendLineDic[friend.userName] = CreateFriendLine(friend);
            }
        }

        discordArea.OnAttributePanelOff += delegate { attribuePanel.SetActive(false); };
    }

    private DiscordFriendLine CreateFriendLine(DiscordProfileDataSO data)
    {
        DiscordFriendLine friendLine = Instantiate(friendLinePrefab, this.transform);

        friendLine.OnLeftClickPanel += LeftClickLine;
        friendLine.OnRightClickPanel += RightClickLine;
        friendLine.myData = data;
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

        friendLine.gameObject.SetActive(true);

        return friendLine;
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
        EventManager.TriggerEvent(EDiscordEvent.ShowChattingPanel, new object[] { line.myData.userName }) ;
    }

    public void RightClickLine(Vector2 pos, DiscordFriendLine line)
    {
        //속성 열어줌
        DiscordAttributePanel attributeData = attribuePanel.GetComponentInChildren<DiscordAttributePanel>();
        attributeData.data = line.myData;
        AttributeFriendLine = line;
        attribuePanel.transform.position = pos;
        attribuePanel.gameObject.SetActive(true);
    }

    //세로운 메세지 왔을때 노티스패널 키고 맨 위로 올리는 함수 
    public void NewMessage(DiscordProfileDataSO data)
    {
        foreach(var friend in friendList)
        {
            if(friend == data)
            {
                //이미 프렌드 리스트에 받은 사람이 있음
                friendLineDic[data.userName].transform.SetAsFirstSibling();
                friendLineDic[data.userName].NoticePanel.SetActive(true);
                return;
            }
        }

        //프렌드 리스트에 없으니 새로 생성해줘야함
        DiscordFriendLine line = CreateFriendLine(data);
        line.transform.SetAsFirstSibling();
        line.NoticePanel.SetActive(true);
        return;
    }

    //
    //              예시용
    //
    //void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.P))
    //    {
    //        NewMessage(debugData);
    //    }
    //}

}
