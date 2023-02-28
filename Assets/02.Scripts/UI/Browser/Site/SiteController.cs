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
}
