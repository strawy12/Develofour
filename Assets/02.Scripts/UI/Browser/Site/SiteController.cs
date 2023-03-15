using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Browser : Window
{
    [SerializeField]
    private List<Site> siteObjectList = new List<Site>();

    public Site CreateSite(Site selectSite)
    {
        Site createSite = Instantiate(selectSite, selectSite.transform.parent);
        createSite.Init();
        createSite.gameObject.SetActive(true);

        return createSite;
    }

    public bool CheckZoogleSiteLogin() 
    {
        if (!DataManager.Inst.SaveData.isSuccessLoginZoogle)
        {
            return false;
        }
            
        return true;
    }

    public bool TryGetSitePrefab(ESiteLink siteLink, out Site site)
    {
        siteDictionary.TryGetValue(siteLink, out site);

        switch (siteLink)
        {
            case ESiteLink.Email:
            case ESiteLink.Brunch:
                {
                    if (!CheckZoogleSiteLogin())
                    {
                        requestSite = siteLink;
                        siteDictionary.TryGetValue(ESiteLink.GoogleLogin, out site);
                    }

                    break;
                }
            case ESiteLink.Starbook:
                {
                    // 
                    break;
                }
        }

        return site != null;
    }
}
