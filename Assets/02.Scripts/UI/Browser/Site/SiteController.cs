using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Browser : Window
{
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
            case ESiteLink.Branch:
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
                    if(!DataManager.Inst.SaveData.isSuccessLoginStarbook)
                    {
                        requestSite = siteLink;
                        siteDictionary.TryGetValue(ESiteLink.StarbookLoginSite, out site);
                    }
                    break;
                }
        }

        return site != null;
    }
}
