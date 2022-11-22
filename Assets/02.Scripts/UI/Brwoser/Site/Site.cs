using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Profiling.Memory.Experimental;
using UnityEngine;

public abstract class Site : MonoUI
{
    [SerializeField]
    protected ESiteLink siteLink;

    public ESiteLink SiteLink
    {
        get
        {
            return siteLink;
        }
    }

    [SerializeField]
    protected SiteData siteData;

    public SiteData SiteData
    {
        get 
        {
            return siteData;
        }
    }

    public Action OnUsed;
    public Action OnUnused;
    private bool isSubscribe;

    public virtual void Init()
    {
        OnUsed += Subscribe;
        OnUsed += ShowSite;
        OnUnused += HideSite;
    }

    protected virtual void ResetSite()
    {
        EventManager.StopListening(ESiteEvent.ResetSite, SiteStopEvent);
        isSubscribe = false;    
    }

    private void SiteStopEvent(object[] ps)
    {
        ResetSite();
    }

    private void Subscribe()
    {
        if(!isSubscribe)
        {
            EventManager.StartListening(ESiteEvent.ResetSite, (x) => ResetSite());
            isSubscribe = true;
        }
    }

    protected virtual void ShowSite()
    {
        SetActive(true);
    }

    protected virtual void HideSite()
    {
        Debug.Log(name);
        SetActive(false);
    }
}
