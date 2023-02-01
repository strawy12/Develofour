using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
public class FacebookFriendPanel : MonoBehaviour
{

    [Header("Friend")]
    [SerializeField]
    private List<FacebookProfileDataSO> friendDataList;
    [SerializeField]
    private FacebookProfilePanel profilePanel;
    [SerializeField]
    private FacebookFriendLine friendLinePrefab;
    [SerializeField]
    private Transform friendLineParent;

    private List<FacebookFriendLine> friendLineList = new List<FacebookFriendLine>();

    public void Init()
    {
        CreateFriend();
    }


    private void CreateFriend()
    {
        for (int i = 0; i < friendDataList.Count; i++)
        {
            FacebookFriendLine line = Instantiate(friendLinePrefab, friendLineParent);
            line.OnSelect += (x) => LineSelectPanelSetActive();
            line.OnSelect += SetProfilePanel;
            line.Init(friendDataList[i]);
            friendLineList.Add(line);
            line.gameObject.SetActive(true);
        }
    }
    private void LineSelectPanelSetActive()
    {
        for (int i = 0; i < friendLineList.Count; i++)
        {
            Debug.Log("lineSetActiveFalse");
            friendLineList[i].SetSelectPanel(false);
        }
    }

    private void SetProfilePanel(FacebookProfileDataSO data)
    {
        Debug.Log("와");

        profilePanel.gameObject.SetActive(true);
        //profilePanel.Setting(data);
    }

    private void ShowAllFriend()
    {
        friendLineParent.gameObject.SetActive(true);
    }
}
