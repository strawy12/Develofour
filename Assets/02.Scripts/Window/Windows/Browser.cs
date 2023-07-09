using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public enum ESiteLink
{
    None,
    Chrome,
    GoogleLogin,
    Email,
    Branch,
    BranchLogin,
    NullSite,
    Map,
    BranchPasswordSite,
}
/// <summary>
///  블로그 사이트 이름
/// </summary>
public partial class Browser : Window
{
    public static Browser currentBrowser;
    private Dictionary<ESiteLink, Site> siteDictionary = new Dictionary<ESiteLink, Site>();

    [SerializeField]
    private Site usingSite;
    public Site UsingSite => usingSite;

    [SerializeField] private Transform siteParent;

    [SerializeField] private BrowserBar browserBar;
    [SerializeField] private LoadingIcon loadingBar;

    [SerializeField] private TMP_InputField addressInputField;

    public TMP_InputField AddressInputField => addressInputField;

    private ESiteLink requestSite;
    private List<Site> usedSiteList;

    private bool isLoading = false;

    private Coroutine loadingCoroutine;

    private string saveAddressString = "";
    private bool isNotFirstOpen = false;
    protected override void Init()
    {
        base.Init();
        //한글 입력 막기/
        addressInputField.onSelect.AddListener(ResetAddressInputField);
        addressInputField.onDeselect.AddListener(SettingAddressInputField);
        addressInputField.onSubmit.AddListener(AddressChangeSite);
        siteDictionary = new Dictionary<ESiteLink, Site>();
        usedSiteList = new List<Site>();

        BindingStart();
        OnUnSelected += OverlayClose;
        OnSelected += SelectedBrowser;

        OnClosed += (a) => ResetBrowser();
        browserBar.Init();
        browserBar.OnClose?.AddListener(WindowClose);

        browserBar.OnUndo?.AddListener(UndoSite);
        browserBar.OnRedo?.AddListener(RedoSite);

        EventManager.StartListening(EBrowserEvent.OnUndoSite, UndoSite);
        EventManager.StartListening(ELoginSiteEvent.LoginSuccess, LoginSiteOpen);

        ChangeSite(ESiteLink.Chrome, 0f, false);
    }

    private void OverlayClose()
    {
        ProfileOverlaySystem.OnClose?.Invoke();
    }

    private void ResetAddressInputField(string str)
    {
        if (usingSite.SiteLink == ESiteLink.Chrome)
        {
            saveAddressString = addressInputField.text;
            addressInputField.text = "";
        }
    }

    private void SettingAddressInputField(string str)
    {
        if (usingSite.SiteLink == ESiteLink.Chrome)
        {
            addressInputField.text = saveAddressString;
        }

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

    public void AddressChangeSite(string address)
    {
        switch (address)
        {
            case Constant.BranchSite:
                ChangeSite(ESiteLink.Branch, Constant.LOADING_DELAY);
                break;
            case Constant.ZMailSite:
                ChangeSite(ESiteLink.Email, Constant.LOADING_DELAY);
                break;
            case Constant.BranchNewPasswordSite:
                ChangeSite(ESiteLink.BranchPasswordSite, Constant.LOADING_DELAY);
                break;
            case Constant.LoginSite:
                ChangeSite(ESiteLink.Chrome, Constant.LOADING_DELAY);
                break;
            default:
                ChangeSite(ESiteLink.NullSite, Constant.LOADING_DELAY);
                break;
        }
    }

    public void ChangeSite(ESiteLink eSiteLink, float loadDelay, bool addUndo = true)
    {
        Site sitePrefab = null;

        if (eSiteLink == ESiteLink.None)
        {
            eSiteLink = ESiteLink.Chrome;
        }

        if (TryGetSitePrefab(eSiteLink, out sitePrefab))
        {
            Debug.Log(addUndo);
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

        if (beforeSite != null)
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

        if (isPrefab)
        {
            currentSite = CreateSite(currentSite);
        }

        usingSite = currentSite;
        // before 사이트는 위에서 넣고 using을 currentSite로 여기서 갱신

        if (addUndo && beforeSite != null)
        {
            Debug.Log("트리거");
            usingSite.SetUndoSite(beforeSite);
        }
        //else if (usingSite.SiteLink != ESiteLink.Chrome &&beforeSite.undoSite != null)
        //{
        //    usingSite.SetUndoSite(beforeSite.undoSite);
        //}
        if (addUndo && beforeSite != null)
        {
            beforeSite.SetRedoSite(null);
        }

        usingSite.gameObject.SetActive(true);
        usingSite?.OnUsed?.Invoke();

        browserBar.SettingButtons();
        browserBar.ChangeSiteData(usingSite.SiteData); // 로딩이 다 되고 나서 바뀌게 해놈
    }

    private void DeleteRedoSite(Site site)
    {
        if (site == null)
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

    public void LoginSiteOpen(object[] ps)
    {
        ChangeSite(requestSite, Constant.LOADING_DELAY, true);
    }

    public void UndoSite(object[] emptyParam) => UndoSite();

    public bool IsExistUndoSite()
    {
        if (usingSite.undoSite == null)
        {
            return false;
        }
        return true;
    }

    public bool IsExistRedoSite()
    {
        if (usingSite.redoSite == null)
        {
            return false;
        }
        return true;
    }


    public void UndoSite()
    {

        if (isLoading) return;
        if (usingSite.undoSite == null)
        {
            return;
        }
        Debug.Log("언도");
        Debug.Log(usingSite);
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
        Debug.Log("리도");
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

    public override void SizeDoTween()
    {
        if (isNotFirstOpen)
        {
            SetActive(true);
        }
        else
        {
            base.SizeDoTween();
        }
        isNotFirstOpen = true;
    }

    private void OnDestroy()
    {
        EventManager.StopListening(EBrowserEvent.OnUndoSite, UndoSite);
    }

}
