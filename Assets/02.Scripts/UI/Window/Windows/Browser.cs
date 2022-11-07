using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


// 임시 EsiteLink
public enum ESiteLink
{
    None,
    Chrome,
    Youtube,
    Youtube_News
}

public class Browser : Window
{
    private Dictionary<ESiteLink, Site> siteDictionary;

    private Site usingSite;

    private Stack<Site> undoSite;
    private Stack<Site> redoSite;

    private Transform siteParent;

    private BrowserBar browserBar;

    public static Action<ESiteLink> OnOpenSite;

    void Awake()
    {
        Init();
    }

    private void Start()
    {
        BindingStart();
    }

    private void Init()
    {
        undoSite = new Stack<Site>();
        redoSite = new Stack<Site>();

        OnOpenSite += ChangeSite;
        OnClosed += (a) => ResetBrowser();
        //browserBar.OnClose?.AddListener(WindowClose);
        //browserBar.OnUndo?.AddListener(UndoSite);
        //browserBar.OnRedo?.AddListener(RedoSite);
    }



    private void BindingStart()
    {
        for (int i = 0; i < siteParent.childCount; i++)
        {
            Site site = siteParent.GetChild(i).GetComponent<Site>();
            siteDictionary.Add(site.Link, site);
            //site.Init();
        }
    }

    public void ChangeSite(ESiteLink eSiteLink)
    {
        Site site = null;

        if(siteDictionary.TryGetValue(eSiteLink, out site))
        {
            ChangeSite(site);
        }

        else
        {
            Debug.LogError($"SiteDictionary의 값 중에 {eSiteLink}을 키로 갖는 값이 존재하지 않습니다.");
        }
        // 이전 사이트를 저장하고
        // 사이트 바꿈
    }

    public Site ChangeSite(Site site)
    {
        if(siteDictionary.ContainsValue(site) == false)
        {
            Debug.LogError($"Dictonary에 존재하지 않는 Site가 있습니다. {site.gameObject.name}");
            return null;
        }

        Site beforeSite = usingSite;
        //usingSite?.OnUnused?.Invoke();
        undoSite.Push(usingSite);
        usingSite = site;
        //usingSite?.OnUsed?.Invoke();

        return beforeSite;
    }

    public void UndoSite()
    {
        Site beforeSite = ChangeSite(undoSite.Pop());
        redoSite.Push(beforeSite); // 앞으로 갈 사이트는 사용하던 사이트 
         // 뒤로 갈 사이트는 undosite의 top
    }

    public void RedoSite()
    {
        Site beforeSite = ChangeSite(redoSite.Pop());
        undoSite.Push(beforeSite);
        // 작동은 UndoSite함수의 정 반대로 
    }

    public void ResetBrowser()
    {
       // EventManager.TriggerEvent(EEvent.ResetBrowser);
        usingSite = null;

        undoSite.Clear();
        redoSite.Clear();
    }
}
