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
    FacebookLoginSite,
    FacebookPasswordResetSite,
}
/// <summary>
///  블로그 사이트 이름
/// </summary>
public partial class Browser : Window
{
    public static Browser currentBrowser;
    private Dictionary<ESiteLink, Site> siteDictionary = new Dictionary<ESiteLink, Site>();

    private Site usingSite;

    [SerializeField] private Transform siteParent;

    [SerializeField] private BrowserBar browserBar;
    [SerializeField] private LoadingBar loadingBar;

    private List<Site> usedSiteList;

    private bool isLoading = false;

    private Coroutine loadingCoroutine;

    protected override void Init()
    {
        base.Init();

        siteDictionary = new Dictionary<ESiteLink, Site>();
        usedSiteList = new List<Site>();

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
        ChangeSite(ESiteLink.Chrome, 0f, false);
        EventManager.TriggerEvent(EBrowserEvent.AddFavoriteSiteAll);
    }

    private void BindingStart()
    {
        for (int i = 0; i < siteParent.childCount; i++)
        {
            Site site = siteParent.GetChild(i).GetComponent<Site>();
            siteDictionary.Add(site.SiteLink, site);
            //site.Init();
        }
    }

    public void ChangeSite(ESiteLink eSiteLink, float loadDelay, bool addUndo = true)
    {
        Site sitePrefab = null;

        if (TryGetSitePrefab(eSiteLink, out sitePrefab))
        {
            ChangeSite(sitePrefab, loadDelay, addUndo, true);
        }
        else
        {
            Debug.LogError($"SiteDictionary의 값 중에 {eSiteLink}을 키로 갖는 값이 존재하지 않습니다.");
        }
    }

    public Site ChangeSite(Site site, float loadDelay, bool addUndo = true, bool isPrefab = false)
    {
        if (siteDictionary.ContainsKey(site.SiteLink) == false)
        {
            Debug.LogError($"Dictonary에 존재하지 않는 Site가 있습니다. {site.gameObject.name}");
            return null;
        }

        if (loadingCoroutine != null)
        {
            StopCoroutine(loadingCoroutine);
            loadingBar.StopLoading();
        }

        foreach (Site usedSite in usedSiteList)
        {
            if (usedSite.gameObject.activeSelf)
            {
                usedSite.OnUnused?.Invoke();
            }
        }

        WindowOpen();
        
        Site beforeSite = usingSite;

        if(beforeSite != null)
        {
            beforeSite?.OnUnused?.Invoke();
            beforeSite.gameObject.SetActive(false);
        }

        loadingCoroutine = StartCoroutine(LoadingSite(loadDelay, () =>
        {
            OpenSite(site, beforeSite, addUndo, isPrefab);
        }));

        return beforeSite;
    }

    private void OpenSite(Site currentSite, Site beforeSite, bool addUndo, bool isPrefab) 
    {
        // addUndo == false라면 undo에서 넘어 옴

        if(isPrefab)
        {
            currentSite = CreateSite(currentSite); 
        }

        usingSite = currentSite;
       
        if (addUndo && beforeSite != null)
        {
            usingSite.SetUndoSite(beforeSite);
        }

        if(addUndo && beforeSite != null) 
        {
            DeleteRedoSite(beforeSite.redoSite);
            beforeSite.SetRedoSite(null);
        }

        usingSite.gameObject.SetActive(true);
        usingSite?.OnUsed?.Invoke();
        
        browserBar.ChangeSiteData(usingSite.SiteData); // 로딩이 다 되고 나서 바뀌게 해놈
    }

    private void DeleteRedoSite(Site site)
    {
        if(site == null)
        {
            return;
        }

        DeleteRedoSite(site.redoSite);
        Destroy(site.gameObject);
    }

    private IEnumerator LoadingSite(float loadDelay, Action Callback)
    {
        isLoading = true;
        if (loadDelay != 0)
        {
            loadingBar.StartLoading(loadDelay);
            yield return new WaitForSeconds(loadDelay);
        }
        loadingBar.StopLoading();

        Callback?.Invoke();

        isLoading = false;
        loadingCoroutine = null;
    }

    public void UndoSite(object[] emptyParam) => UndoSite();

    public void UndoSite()
    {
        if (isLoading) return;
        if (usingSite.undoSite == null) return;

        Site currentSite = usingSite.undoSite;
        Site beforeSite = ChangeSite(currentSite, Constant.LOADING_DELAY, false);

        currentSite.SetRedoSite(beforeSite);

        // 앞으로 갈 사이트는 사용하던 사이트 
        // 뒤로 갈 사이트는 undosite의 top
    }

    public void RedoSite()
    {
        if (isLoading) return;
        if (usingSite.redoSite == null) return;

        Site currentSite = usingSite.redoSite;
        usingSite.SetRedoSite(null);

        ChangeSite(currentSite, Constant.LOADING_DELAY, true);

        // 작동은 UndoSite함수의 정 반대로 
    }

    public void ResetBrowser()
    {
        // EventManager.TriggerEvent(EEvent.ResetBrowser);
        currentBrowser = null;
        usingSite = null;
    }

    public void SelectedBrowser()
    {
        currentBrowser = this;
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EBrowserEvent.OnUndoSite, UndoSite);
    }

}
