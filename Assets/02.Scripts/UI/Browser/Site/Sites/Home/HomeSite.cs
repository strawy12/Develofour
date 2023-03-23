using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AddressableAssets;

public class HomeSite : Site , IPointerClickHandler
{
    private static HomeSearchRecordDataSO searchRecordData;

    [SerializeField]
    private HomeFavoriteBar favoriteBar;
    [SerializeField]
    private HomeSearchRecord recordPanel;
    [SerializeField]
    private Button SeacrhPanel;
    [SerializeField]
    private HomeProfile profilePanel;

    public override void Init()
    {
        if(searchRecordData == null)
        {
            var handle = Addressables.LoadAssetAsync<HomeSearchRecordDataSO>("HomeSearchData");
            searchRecordData = handle.WaitForCompletion();
        }

        favoriteBar.Init();

        profilePanel.Init();
        recordPanel.Init(searchRecordData);
        SeacrhPanel.onClick.AddListener(ShowRecordPanel);
        recordPanel.OnCloseRecord += ShowSearchPanel;

        base.Init();
    }

    protected override void ShowSite()
    {
        base.ShowSite();
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
            profilePanel.loginPanel.gameObject.SetActive(false);
        }
    }
}
