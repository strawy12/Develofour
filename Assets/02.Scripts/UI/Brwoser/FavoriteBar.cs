using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FavoriteBar : MonoBehaviour
{ 
    [SerializeField] private Transform favoritesParent;
    [SerializeField] private FavoriteButton favoriteBtnPrefab;
    [SerializeField] private Transform siteParent;
    private List<FavoriteButton> favoritesList = new List<FavoriteButton>();

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
        FavoriteButton button = Instantiate(favoriteBtnPrefab, favoritesParent);
        button.SiteLink = siteLink;
        button.Init(site.SiteData.siteIconSprite, site.SiteData.siteTitle);
        favoritesList.Add(button);
        button.gameObject.SetActive(false);
    }

    public void AddFavoritesButton(object[] param)
    {
        if (param == null || !(param[0] is ESiteLink)) return;
        ESiteLink siteLink = (ESiteLink)param[0];
        foreach (FavoriteButton favoriteButton in favoritesList)
        {
            if (favoriteButton.SiteLink == siteLink)
            {
                favoriteButton.gameObject.SetActive(true);
                return;
            }
        }

        FavoriteButton button = Instantiate(favoriteBtnPrefab, favoritesParent);
        button.SiteLink = siteLink;
        button.OnClick.AddListener(() => Browser.OnOpenSite.Invoke(siteLink, Constant.LOADING_DELAY));
        favoritesList.Add(button);
    }
}
