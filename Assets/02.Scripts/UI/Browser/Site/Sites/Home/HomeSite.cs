using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.AddressableAssets;

public class HomeSite : Site , IPointerClickHandler
{
    [SerializeField]
    private HomeSearchRecord recordPanel;
    [SerializeField]
    private HomeFavoriteBar favoriteBar;
    [SerializeField]
    private Button SeacrhPanel;
    [SerializeField]
    private HomeProfile profilePanel;

    public override void Init()
    {
        favoriteBar.Init();

        profilePanel.Init();
        //SeacrhPanel.onClick.AddListener(ShowRecordPanel);

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

    public void OnPointerClick(PointerEventData eventData)
    {
        if(profilePanel.isActiveAndEnabled)
        {
            profilePanel.loginPanel.gameObject.SetActive(false);

            SeacrhPanel.gameObject.SetActive(true);
            recordPanel.Close();
        }
    }
}
