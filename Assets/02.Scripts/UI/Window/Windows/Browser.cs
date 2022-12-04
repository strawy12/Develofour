using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.EventSystems;
using UnityEngine;
using System.Net.NetworkInformation;

public enum ESiteLink
{
    None,
    Chrome,
    Youtube,
    Youtube_News,
    GoogleLogin,
    Email,
    Brunch,
    Facebook,
}

public class Browser : Window
{
    public static Browser currentBrowser;
    private Dictionary<ESiteLink, Site> siteDictionary = new Dictionary<ESiteLink, Site>();

    private Site usingSite;

    private Stack<Site> undoSite;
    private Stack<Site> redoSite;

    [SerializeField] private Transform siteParent;

    [SerializeField] private BrowserBar browserBar;
    [SerializeField] private LoadingBar loadingBar;

    private bool isLoading = false;

    protected override void Init()
    {
        base.Init();
        undoSite = new Stack<Site>();
        redoSite = new Stack<Site>();
        siteDictionary = new Dictionary<ESiteLink, Site>();

        BindingStart();
        //OnUndoSite += UndoSite;
        //OnOpenSite += ChangeSite;
        OnSelected += SelectedBrowser;

        OnClosed += (a) => ResetBrowser();

        browserBar.Init();
        browserBar.OnClose?.AddListener(WindowClose);
        browserBar.OnUndo?.AddListener(UndoSite);
        browserBar.OnRedo?.AddListener(RedoSite);

        EventManager.StartListening(EBrowserEvent.OnUndoSite, UndoSite);
    }

    private void BindingStart()
    {
        for (int i = 0; i < siteParent.childCount; i++)
        {
            Site site = siteParent.GetChild(i).GetComponent<Site>();
            siteDictionary.Add(site.SiteLink, site);
            site.Init();
        }
    }

    public void ChangeSite(ESiteLink eSiteLink, float loadDelay)
    {
        Site site = null;

        if (siteDictionary.TryGetValue(eSiteLink, out site))
        {
            ChangeSite(site, loadDelay);
        }
        else
        {
            Debug.LogError($"SiteDictionary�� �� �߿� {eSiteLink}�� Ű�� ���� ���� �������� �ʽ��ϴ�.");
        }
    }

    public Site ChangeSite(Site site, float loadDelay, bool addUndo = true)
    {
        if (siteDictionary.ContainsValue(site) == false)
        {
            Debug.LogError($"Dictonary�� �������� �ʴ� Site�� �ֽ��ϴ�. {site.gameObject.name}");
            return null;
        }

        WindowOpen();
        Site beforeSite = usingSite;
        usingSite?.OnUnused?.Invoke();

        StartCoroutine(LoadingSite(loadDelay, () =>
        {
            if (addUndo && usingSite != null)
            {
                undoSite.Push(usingSite);
            }

            usingSite = site;
            usingSite?.OnUsed?.Invoke();
        }));

        return beforeSite;
    }

    private IEnumerator LoadingSite(float loadDelay, Action Callback)
    {
        if(loadDelay != 0)
        {
            loadingBar.StartLoading();
            yield return new WaitForSeconds(loadDelay);
        }

        Callback?.Invoke();
        loadingBar.StopLoading();
    }

    public void UndoSite(object[] emptyParam) => UndoSite();
    public void UndoSite()
    {
        if (isLoading) return;
        if (undoSite.Count == 0) return;
        Site currentSite = undoSite.Pop();

        Site beforeSite = ChangeSite(currentSite, Constant.LOADING_DELAY, false);
        redoSite.Push(beforeSite); // ������ �� ����Ʈ�� ����ϴ� ����Ʈ 
                                   // �ڷ� �� ����Ʈ�� undosite�� top
    }

    public void RedoSite()
    {
        if (isLoading) return;
        if (redoSite.Count == 0) return;

        Site beforeSite = ChangeSite(redoSite.Pop(), Constant.LOADING_DELAY, false);
        undoSite.Push(beforeSite);
        // �۵��� UndoSite�Լ��� �� �ݴ�� 
    }

    public void ResetBrowser()
    {
        // EventManager.TriggerEvent(EEvent.ResetBrowser);
        usingSite = null;

        undoSite.Clear();
        redoSite.Clear();
    }

    public void SelectedBrowser()
    {
        currentBrowser = this;
    }
}
