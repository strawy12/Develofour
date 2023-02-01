using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FacebookSite : Site
{
    
    [Header("Pid")]
    [SerializeField]
    private List<FacebookPidPanelDataSO> pidDataList;
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
    private Button myProfileBtn;
    [SerializeField]
    private FacebookProfileDataSO myProfileData;
    
    private List<FacebookPidPanel> pidList = new List<FacebookPidPanel>();

    public FacebookProfilePanel myProfile;

    //Pid부분은 나중에 다시 만들어야함

    private void CreatePid()
    {
        //use Pooling!
        for (int i = 0; i < pidDataList.Count; i++)
        {
            FacebookPidPanel pid = Instantiate(pidPrefab, pidParent);
            pid.gameObject.SetActive(true);
            pidList.Add(pid);
        }
        myProfile.Setting(myProfileData);
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)pidParent);
    }

    private void SettingPid()
    {
        for (int i = 0; i < pidDataList.Count; i++)
        {
            pidList[i].Setting(pidDataList[i], true);
        }
    }
    public override void Init()
    {
        base.Init();
        CreatePid();
        facebookFriendPanel.Init();
        friendPanelBtn.onClick.AddListener(ShowFriendPanel);
        homePanelBtn.onClick.AddListener(ShowHomePanel);
        myProfileBtn.onClick.AddListener(ShowMyProfile);


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
        base.ShowSite();
        EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSite, new object[] { ESiteLink.Facebook, Constant.LOADING_DELAY });
       
        if(!DataManager.Inst.CurrentPlayer.CurrentChapterData.isLoginSNSSite)
        {
            EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { ESiteLink.FacebookLoginSite, 0f, false});
            return;
        }
        SettingPid();

    }

    private void ShowHomePanel()
    {
        facebookFriendPanel.gameObject.SetActive(false);
        myProfile.gameObject.SetActive(false);
        homePanel.SetActive(true);
    }

    private void ShowFriendPanel()
    {
        homePanel.SetActive(false);
        myProfile.gameObject.SetActive(false);

        facebookFriendPanel.gameObject.SetActive(true);
    }
    private void ShowMyProfile()
    {
        homePanel.SetActive(false);
        facebookFriendPanel.gameObject.SetActive(false);
        myProfile.gameObject.SetActive(true);

    }
}
