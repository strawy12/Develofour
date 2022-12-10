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
    //[SerializeField]
    //private GameObject homePanel;
    //[SerializeField]
    //private GameObject leftPanel;

    [Header("Friend")]
    [SerializeField]
    private List<FacebookFriendDataSO> friendList;
    [SerializeField]
    private FacebookProfilePanel profilePanelPrefab;
    [SerializeField]
    private FacebookFriendLine friendLinePrefab;
    [SerializeField]
    private Transform friendLineParent;
    private List<FacebookFriendLine> friendLineList = new List<FacebookFriendLine>();

    //Pid부분은 나중에 다시 만들어야함

    private void CreatePid()
    {
        //use Pooling!
        for (int i = 0; i < pidList.Count; i++)
        {
            FacebookPidPanel pid = Instantiate(pidPrefab, pidParent);
            pid.Setting(pidList[i]);
            pid.gameObject.SetActive(true);
        }
    }

    private void CreateFriend()
    {
        for(int i = 0; i < friendList.Count; i++)
        {
            FacebookFriendLine line = Instantiate(friendLinePrefab, friendLineParent);
            line.OnSelect += (x) => LineSelectPanelSetActive();
            line.OnSelect += SetProfilePanel;
            line.Init(friendList[i]);
            friendLineList.Add(line);
            line.gameObject.SetActive(true);
        }
    }

    private void LineSelectPanelSetActive()
    {
        for(int i = 0; i < friendLineList.Count; i++)
        {
            Debug.Log("lineSetActiveFalse");
            friendLineList[i].SetSelectPanel(false);
        }
    }
        
    private void SetProfilePanel(FacebookFriendDataSO data)
    {
        Debug.Log("와");
        
        profilePanelPrefab.gameObject.SetActive(true);
        profilePanelPrefab.Setting(data);
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
        //homePanel.SetActive(true);
        //leftPanel.SetActive(true);
    }

    private void ShowFriendPanel()
    {
        //homePanel.SetActive(false);
        //leftPanel.SetActive(false);
        facebookFriendPanel.gameObject.SetActive(true);
    }
}
