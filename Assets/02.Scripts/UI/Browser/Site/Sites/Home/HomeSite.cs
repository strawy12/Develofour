using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HomeSite : Site , IPointerClickHandler
{

    [SerializeField]
    private HomeFavoriteBar favoriteBar;
    [SerializeField]
    private HomeSearchRecord recordPanel;
    [SerializeField]
    private Button SeacrhPanel;
    [SerializeField]
    private List<HomeSearchRecordDataSO> searchRecordDatas = new List<HomeSearchRecordDataSO>();
    [SerializeField]
    private HomeProfile profilePanel;

    public override void Init()
    {
        profilePanel.Init();
        favoriteBar.Init();
        CheckData();
        SeacrhPanel.onClick.AddListener(ShowRecordPanel);
        recordPanel.OnCloseRecord += ShowSearchPanel;

        base.Init();
    }
    private void CheckData()
    {
        foreach (var data in searchRecordDatas)
        {
            if (data.characterDataType == DataManager.Inst.CurrentPlayer.currentChapterType)
            {
                recordPanel.Init(data);
                break;
            }
        }
    }
    protected override void ShowSite()
    {
        base.ShowSite();
        CheckData();
        favoriteBar.ReadSiteListsData();
    }

    protected override void HideSite()
    {
        base.HideSite();
        profilePanel.HidePanel();
    }

    private void ShowRecordPanel()
    {
        SeacrhPanel.gameObject.SetActive(false);
        recordPanel.OpenPanel();
    }

    private void ShowSearchPanel()
    {
        SeacrhPanel.gameObject.SetActive(true);
        recordPanel.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(profilePanel.isActiveAndEnabled)
        {
            profilePanel.LoginPanel.gameObject.SetActive(false);
        }
    }
}
