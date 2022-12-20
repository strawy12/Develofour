using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomeSite : Site
{

    [SerializeField]
    private HomeFavoriteBar favoriteBar;
    [SerializeField]
    private HomeSearchRecord recordPanel;
    [SerializeField]
    private Button SeacrhPanel;
    [SerializeField]
    private List<HomeSearchRecordDataSO> searchRecordDatas = new List<HomeSearchRecordDataSO>();

    public override void Init()
    {
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
}
