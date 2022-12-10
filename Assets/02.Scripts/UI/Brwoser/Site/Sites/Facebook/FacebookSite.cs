using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FacebookSite : Site
{
    [Header("Pid")]
    [SerializeField]
    private List<FacebookPidPanelDataSO> pidList;
    [SerializeField]
    private Transform pidParent;

    [SerializeField]
    private FacebookPidPanel pidPrefab;
    [Header("TopPanel")]
    [SerializeField]
    private Button friendPanelBtn;
    [SerializeField]
    private Button homePanelBtn;
    [SerializeField]
    private FacebookFriendPanel facebookFriendPanel;
    [SerializeField]
    private GameObject homePanel;
    [SerializeField]
    private GameObject leftPanel;

    [Header("Friend")]
    [SerializeField]
    private List<FacebookFriendDataSO> friendList;
    [SerializeField]
    private FacebookProfilePanel profilePanelPrefab;
    [SerializeField]
    private Transform friendTransform;

    //Maybe it has to change Dictionary
    [SerializeField]
    private List<FacebookProfilePanel> profilePanelList;
    //facebookfrienddataso의 값이 이 프리팹에 적용되서 생성될꺼임

    //Pid부분은 나중에 다시 만들어야함

    private void CreatePid()
    {
        //use Pooling!
        for (int i = 0; i < pidList.Count; i++)
        {
            FacebookPidPanel pid = Instantiate(pidPrefab, pidParent);
            pid.Init(pidList[i]);
            pid.gameObject.SetActive(true);
        }
    }

    private void CreateFriend()
    {
        for(int i = 0; i < friendList.Count; i++)
        {
            FacebookProfilePanel profile = Instantiate(profilePanelPrefab, friendTransform);
            profile.Init(friendList[i]);
            profilePanelList.Add(profile);
        }
        profilePanelList[0].gameObject.SetActive(true);
    }

    public override void Init()
    {
        CreatePid();
        CreateFriend();
        base.Init();
        friendPanelBtn.onClick.AddListener(ShowFriendPanel);
        homePanelBtn.onClick.AddListener(ShowHomePanel);
    }

    protected override void HideSite()
    {
        base.HideSite();
    }

    protected override void ResetSite()
    {
        base.ResetSite();
    }

    protected override void ShowSite()
    {
        EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSite, new object[] { ESiteLink.Facebook, Constant.LOADING_DELAY });
        base.ShowSite();
    }

    private void ShowHomePanel()
    {
        facebookFriendPanel.gameObject.SetActive(false);
        homePanel.SetActive(true);
        leftPanel.SetActive(true);
    }

    private void ShowFriendPanel()
    {
        homePanel.SetActive(false);
        leftPanel.SetActive(false);
        facebookFriendPanel.gameObject.SetActive(true);
    }
}
