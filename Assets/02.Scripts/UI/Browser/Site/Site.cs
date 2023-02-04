using System;
using System.Collections;
using System.Collections.Generic;
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
        if(!CheckGoogleLogin())
        {
            if (siteLink != ESiteLink.GoogleLogin && siteLink != ESiteLink.Home) //로그인 사이트가 아니라면 혹은 홈 사이트가 아니라면
            {
                EventManager.TriggerEvent(EBrowserEvent.OnOpenSite, new object[] { ESiteLink.GoogleLogin, Constant.LOADING_DELAY, false });
                return;
            }
        }

        SetActive(true);
    }

    protected virtual void HideSite()
    {
        SetActive(false);
    }

    private bool CheckGoogleLogin()
    {
        if (!DataManager.Inst.CurrentPlayer.CurrentChapterData.isEnterLoginGoogleSite)
        {
            return false;
        }

        return true;
    }
}
