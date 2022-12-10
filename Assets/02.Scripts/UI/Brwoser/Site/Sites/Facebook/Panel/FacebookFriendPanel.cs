using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
public class FacebookFriendPanel : MonoBehaviour
{
    [Header("Catecory")]
    [SerializeField]
    private TMP_Text categoryTitleText;
    [SerializeField]
    private Button backBtn;
    [SerializeField]
    private GameObject categoryPanel;
    [SerializeField]
    private Button requestBtn;
    [SerializeField]
    private Button allFriendBtn;
    [SerializeField]
    private Button birthdayBtn;

    [Header("Birthday")]
    [SerializeField]
    private FacebookBirthdayPanel birthdayPanel;

    [Header("Friend")]
    [SerializeField]
    private List<FacebookFriendDataSO> friendDataList;
    [SerializeField]
    private FacebookProfilePanel profilePanel;
    [SerializeField]
    private FacebookFriendLine friendLinePrefab;
    [SerializeField]
    private Transform friendLineParent;

    private List<FacebookFriendLine> friendLineList = new List<FacebookFriendLine>();

    public void Init()
    {
        allFriendBtn.onClick.AddListener(ShowAllFriend);
        birthdayBtn.onClick.AddListener(ShowBirthdayPanel);
        backBtn.onClick.AddListener(ShowCategory);
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

    private void SetProfilePanel(FacebookFriendDataSO data)
    {
        Debug.Log("客");

        profilePanel.gameObject.SetActive(true);
        profilePanel.Setting(data);
    }

    private void ShowAllFriend()
    {
        categoryTitleText.text = "葛电 模备";
        categoryPanel.SetActive(false);
        profilePanel.gameObject.SetActive(false);
        birthdayPanel.gameObject.SetActive(false);
        backBtn.gameObject.SetActive(true);
        friendLineParent.gameObject.SetActive(true);
    }

    private void ShowCategory()
    {
        categoryTitleText.text = "模备";
        backBtn.gameObject.SetActive(false);
        friendLineParent.gameObject.SetActive(false);
        profilePanel.gameObject.SetActive(false);
        birthdayPanel.gameObject.SetActive(false);
        categoryPanel.SetActive(true);
    }

    private void ShowBirthdayPanel()
    {
        profilePanel.gameObject.SetActive(false);
        birthdayPanel.gameObject.SetActive(true);
    }
}
