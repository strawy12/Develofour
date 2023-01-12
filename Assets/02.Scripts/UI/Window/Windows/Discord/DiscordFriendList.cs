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

    private DiscordFriendLine beforeFriendLine = null; //���� ��Ŭ���� ����
    private DiscordFriendLine currentFriendLine = null; //���� ��Ŭ���� ����
    private DiscordFriendLine AttributeFriendLine = null; //��Ŭ�� �� ����

    [SerializeField]    
    private GameObject attribuePanel;

    [SerializeField]
    private DiscordArea discordArea;

    public void Init()
    {
        Debug.Log("�����");
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
        //Ŭ�������� �ش� �޼���â �����ָ� ��
        EventManager.TriggerEvent(EDiscordEvent.ShowChattingPanel, new object[] { line.myData.userName }) ;
    }

    public void RightClickLine(Vector2 pos, DiscordFriendLine line)
    {
        //�Ӽ� ������
        DiscordAttributePanel attributeData = attribuePanel.GetComponentInChildren<DiscordAttributePanel>();
        attributeData.data = line.myData;
        AttributeFriendLine = line;
        attribuePanel.transform.position = pos;
        attribuePanel.gameObject.SetActive(true);
    }

    //���ο� �޼��� ������ ��Ƽ���г� Ű�� �� ���� �ø��� �Լ� 
    public void NewMessage(DiscordProfileDataSO data)
    {
        foreach(var friend in friendList)
        {
            if(friend == data)
            {
                //�̹� ������ ����Ʈ�� ���� ����� ����
                friendLineDic[data.userName].transform.SetAsFirstSibling();
                friendLineDic[data.userName].NoticePanel.SetActive(true);
                return;
            }
        }

        //������ ����Ʈ�� ������ ���� �����������
        DiscordFriendLine line = CreateFriendLine(data);
        line.transform.SetAsFirstSibling();
        line.NoticePanel.SetActive(true);
        return;
    }

    //
    //              ���ÿ�
    //
    //void Update()
    //{
    //    if(Input.GetKeyDown(KeyCode.P))
    //    {
    //        NewMessage(debugData);
    //    }
    //}

}
