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
        base.Init();
        favoriteBar.Init();
        foreach(var data  in searchRecordDatas)
        {
            if(data.characterDataType == DataManager.Inst.CurrentPlayer.currentChapterType)
            {
                recordPanel.Init(data);
                break;
            }
        }
        SeacrhPanel.onClick.AddListener(ShowRecordPanel);
    }

    protected override void ShowSite()
    {
        base.ShowSite();
        favoriteBar.ReadSiteListsData();
    }

    private void ShowRecordPanel()
    {
        SeacrhPanel.gameObject.SetActive(false);
        recordPanel.gameObject.SetActive(true);
    }

}
