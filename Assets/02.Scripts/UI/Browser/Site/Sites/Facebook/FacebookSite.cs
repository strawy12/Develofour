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
        //myProfile.Setting(myProfileData);
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
        //EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSite, new object[] { ESiteLink.Facebook, Constant.LOADING_DELAY });
       
        if(!DataManager.Inst.SaveData.isSuccessLoginStarbook)
        {

            // 패스워드 초기화를 했냐?
            // 메일은 와있는데
            // 다시 요청을 하는건데
            // 메일이 하나 그냥 계속 있어도 될까
            // 새로 안 와도 될까?
            // 퍼즐로 보안코드는 고정을 시키긴 했어
            // 그럼 알림을 계속 띄워야할까?
            // 알림 계속 띄운다는건 다시 말하면 브라우저만 다시 껐다키든가
            // 아니면 뒤로가기 눌렀다가 다시 앞으로 가기 눌러도 온다는 거거든?
            // 비밀번호 재설정 메일 오는거랑 연결이 되어있어 



            if(DataManager.Inst.SaveData.isSuccessLoginStarbook)
            {
                EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { ESiteLink.StarbookPasswordResetSite, 0f, false });
            }
            else
            {
                EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { ESiteLink.StarbookLoginSite, 0f, false});
            }
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
        myProfile.Setting(myProfileData);
    }
}
