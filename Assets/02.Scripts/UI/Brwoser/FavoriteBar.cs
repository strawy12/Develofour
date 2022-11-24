

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FavoriteBar : MonoBehaviour
{
    [SerializeField] private Transform favoritesParent;
    [SerializeField] private BrowserFavoriteButton favoriteBtnPrefab;
    [SerializeField] private Transform siteParent;
    private Dictionary<ESiteLink, BrowserFavoriteButton> favoritesList = new Dictionary<ESiteLink, BrowserFavoriteButton>();
    private List<ESiteLink> SiteLinkData { get { return DataManager.Inst.CurrentPlayer.CurrentChapterData.siteLinks; } }

    public void Init()
    {
        CreatePool();
        EventManager.StartListening(EBrowserEvent.AddFavoriteSite, AddNewFavoritesButton);
        ReadSiteListsData();
    }

    private void CreatePool()
    {
        Site[] sites = siteParent.GetComponentsInChildren<Site>();

        foreach (Site site in sites)
        {
            MakeFavoriteButton(site);
            site.gameObject.SetActive(false);
        }
    }

    private void MakeFavoriteButton(Site site)
    {
        ESiteLink siteLink = site.SiteLink;
        if (!favoritesList.ContainsKey(siteLink))
        {
            BrowserFavoriteButton button = Instantiate(favoriteBtnPrefab, favoritesParent);
            button.SiteLink = siteLink;
            button.Init(site.SiteData.siteIconSprite, site.SiteData.siteTitle);
            button.OnClick.AddListener(() => FavoriteEvent(siteLink));
            favoritesList.Add(siteLink, button);
            button.gameObject.SetActive(false);
        }
    }

    private void ReadSiteListsData()
    {
        foreach (ESiteLink sitelink in SiteLinkData)
        {
            favoritesList[sitelink].gameObject.SetActive(true);
            return;
        }
    }

    public void AddNewFavoritesButton(object[] param)
    {
        if (param == null || !(param[0] is ESiteLink)) return;
        ESiteLink siteLink = (ESiteLink)param[0];

        SiteLinkData.Add(siteLink);

        if (favoritesList.ContainsKey(siteLink))
        {
            favoritesList[siteLink].gameObject.SetActive(true);
        }
        else
        {
            BrowserFavoriteButton button = Instantiate(favoriteBtnPrefab, favoritesParent);
            button.SiteLink = siteLink;
            button.OnClick.AddListener(() => FavoriteEvent(siteLink));
            favoritesList.Add(siteLink, button);
        }
    }

    public void FavoriteEvent(ESiteLink siteLink)
    {
        object[] ps = new object[2] { siteLink, Constant.LOADING_DELAY };

        EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, ps);
    }
}
