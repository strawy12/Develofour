

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
        EventManager.StartListening(EBrowserEvent.AddFavoriteSiteAll, ShowAllFavoritesButton);
        EventManager.StartListening(EBrowserEvent.RemoveFavoriteSite, RemoveFavoritesButton);
        ReadSiteListsData();
    }

    private void CreatePool()
    {
        for (int i = 0; i < siteParent.childCount; i++)
        {
            Site site = siteParent.GetChild(i).GetComponent<Site>();

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

    public void ShowAllFavoritesButton(object[] param)
    {
        foreach(var btn in favoritesList)
        {
            btn.Value.gameObject.SetActive(true);
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
            favoritesList.Add(siteLink, button);
        }
    }

    public void RemoveFavoritesButton(object[] param)
    {
        if (param == null || !(param[0] is ESiteLink)) return;
        ESiteLink siteLink = (ESiteLink)param[0];

        SiteLinkData.Remove(siteLink);

        if (favoritesList.ContainsKey(siteLink))
        {
            favoritesList[siteLink].gameObject.SetActive(false);
        }
    }
}
