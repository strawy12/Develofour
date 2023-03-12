

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class FavoriteBar : MonoBehaviour
{
    [SerializeField] protected Transform favoritesParent;
    [SerializeField] protected BrowserFavoriteButton favoriteBtnPrefab;
    [SerializeField] protected Transform siteParent;

    protected Dictionary<ESiteLink, BrowserFavoriteButton> favoritesList = new Dictionary<ESiteLink, BrowserFavoriteButton>();

    public virtual void Init()
    {
        CreateButton();
    }

    protected void CreateButton()
    {
        for (int i = 0; i < siteParent.childCount; i++)
        {
            Site site = siteParent.GetChild(i).GetComponent<Site>();

            if (!site.SiteData.isHaveFavorite) continue;

            MakeFavoriteButton(site);
            site.gameObject.SetActive(false);
        }
    }

    protected void MakeFavoriteButton(Site site)
    {
        ESiteLink siteLink = site.SiteLink;
        if (!favoritesList.ContainsKey(siteLink))
        {
            BrowserFavoriteButton button = Instantiate(favoriteBtnPrefab, favoritesParent);
            button.SiteLink = siteLink;
            button.Init(site.SiteData.siteIconSprite, site.SiteData.siteTitle);

            favoritesList.Add(siteLink, button);

            button.gameObject.SetActive(true);
        }
    }
}

