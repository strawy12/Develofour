using System;
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
    public Dictionary<string, DiscordFriendLine> friendLineDic = new Dictionary<string, DiscordFriendLine>();

    [SerializeField]
    private DiscordFriendLine friendLinePrefab;

    private DiscordFriendLine beforeFriendLine = null; //���� ��Ŭ���� ����
    public static DiscordFriendLine currentFriendLine = null; //���� ��Ŭ���� ����
    private DiscordFriendLine AttributeFriendLine = null; //��Ŭ�� �� ����

    public DiscordFriendLine CurrentFriendLine { get { return currentFriendLine; } }

    [SerializeField]
    private GameObject attribuePanel;

    [SerializeField]
    private DiscordArea discordArea;

    public static Action<DiscordFriendLine> OnOverlay;

    public void Init()
    {
        foreach (var friend in friendList)
        {
            if (friend.isFriend)
            {
                friendLineDic[friend.userName] = CreateFriendLine(friend);
            }
        }

        discordArea.OnAttributePanelOff += delegate { attribuePanel.SetActive(false); };
        OnOverlay += OpenOverlay;
    }

    private DiscordFriendLine CreateFriendLine(DiscordProfileDataSO data)
    {
        DiscordFriendLine friendLine = Instantiate(friendLinePrefab, this.transform);


        friendLine.OnLeftClickPanel += LeftClickLine;
        friendLine.OnLeftClickPanel += (x) => { discordArea.SettingInputText(data.userName); };
        friendLine.OnRightClickPanel += RightClickLine;
        friendLine.myData = data;
        friendLine.ProfileImage.sprite = data.userSprite;
        if (data.statusMsg == string.Empty)
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

        if (data.overlayID < 0) //constant�� �ִ� ����
        {
            friendLine.overlayTrigger = friendLine.GetComponent<ProfileOverlayOpenTrigger>();
            friendLine.overlayTrigger.fileID = data.overlayID;
            friendLine.OnLeftClickPanel += OpenOverlay;
        }

        friendLine.gameObject.SetActive(true);

        return friendLine;
    }

    public void OpenOverlay(DiscordFriendLine line)
    {
        currentFriendLine.overlayTrigger.OpenByIntList(line.myData.overlayID, Discord.OnGetInfoID?.Invoke(line.myData));
    }

    public void LeftClickLine(DiscordFriendLine line)
    {
        if (line == currentFriendLine)
        {
            return;
        }
        beforeFriendLine = currentFriendLine;
        if (beforeFriendLine != null && beforeFriendLine != line)
        {
            beforeFriendLine.QuitSelectPanel();
        }
        currentFriendLine = line;
        //Ŭ�������� �ش� �޼���â �����ָ� ��
        EventManager.TriggerEvent(EDiscordEvent.ShowChattingPanel, new object[] { line.myData.userName });

        currentFriendLine.overlayTrigger.Close();
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
        foreach (var friend in friendList)
        {
            if (friend == data)
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