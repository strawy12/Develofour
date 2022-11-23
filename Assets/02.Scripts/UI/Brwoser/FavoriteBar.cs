using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoriteBar : MonoBehaviour
{ 
    [SerializeField] private Transform favoritesParent;
    [SerializeField] private BrowserFavoriteButton favoriteBtnPrefab;
    [SerializeField] private Transform siteParent;
    private List<BrowserFavoriteButton> favoritesList = new List<BrowserFavoriteButton>();

    public void Init()
    {
        EventManager.StartListening(EEvent.AddFavoriteSite, AddFavoritesButton);
        CreatePool();
    }

    private void CreatePool()
    {
        Site[] sites = siteParent.GetComponentsInChildren<Site>();

        foreach(Site site in sites)
        {
            MakeFavoriteButton(site);
            site.gameObject.SetActive(false);
        }
    }
    
    private void MakeFavoriteButton(Site site)
    {
        ESiteLink siteLink = site.SiteLink;
        BrowserFavoriteButton button = Instantiate(favoriteBtnPrefab, favoritesParent);
        button.SiteLink = siteLink;
        button.Init(site.SiteData.siteIconSprite, site.SiteData.siteTitle);
        button.OnClick.AddListener(() => Browser.OnOpenSite.Invoke(siteLink));
        favoritesList.Add(button);
        button.gameObject.SetActive(false);
    }

    public void AddFavoritesButton(object param)
    {
        if (param == null || !(param is ESiteLink)) return;
        ESiteLink siteLink = (ESiteLink)param;
        foreach (BrowserFavoriteButton favoriteButton in favoritesList)
        {
            if (favoriteButton.SiteLink == siteLink)
            {
                favoriteButton.gameObject.SetActive(true);
                return;
            }
        }

        BrowserFavoriteButton button = Instantiate(favoriteBtnPrefab, favoritesParent);
        button.SiteLink = siteLink;
        button.OnClick.AddListener(() => Browser.OnOpenSite.Invoke(siteLink));
        favoritesList.Add(button);
    }
}
