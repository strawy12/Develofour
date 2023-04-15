using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
public class BranchSite : Site
{
    [SerializeField]
    private GameObject topPanel;
    [Header("WriterInfo")]
    [SerializeField]
    private GameObject writerInfoPanel;
    [SerializeField]
    private Button writerInfoBtn;
    [Header("WorkList")]
    [SerializeField]
    private Button workListBtn;
    [SerializeField]
    private List<BranchWorkDataSO> workDataList;
    [SerializeField]
    private BranchWorkListPanel workListPanel;
    [Header("PostList")]
    [SerializeField]
    private BranchPostListPanel postListPanel;
    [Header("Post")]
    [SerializeField]
    private BranchPostPanel postPanel;
    public void Awake()
    {
        workListBtn.onClick.AddListener(OnWorkListPanel);
        writerInfoBtn.onClick.AddListener(OnWriterInfoPanel);

    }

    public override void Init()
    {
        base.Init();
        workListPanel.Init(workDataList);
        postListPanel.Init();
        postPanel.Init();

        EventManager.StartListening(EBranchEvent.HideAllPanel, HideAllPanel);
        EventManager.StartListening(EBranchEvent.ShowWorkPanel, OnWorkListPanel);
        EventManager.StartListening(EBranchEvent.ShowPostList, OnPostListPanel);
        EventManager.StartListening(EBranchEvent.ShowPost, OnPostPanel);
    }
    #region OnOffPanel
    public void OnWriterInfoPanel()
    {
        workListPanel.gameObject.SetActive(false);
        postPanel.gameObject.SetActive(false);
        postListPanel.HidePanel();
        writerInfoPanel.gameObject.SetActive(true);
        topPanel.gameObject.SetActive(true);
    }
    public void OnWorkListPanel()
    {
        OnWorkListPanel(null);
    }
    public void OnWorkListPanel(object[] ps = null)
    {
        postPanel.gameObject.SetActive(false);
        postListPanel.HidePanel();
        writerInfoPanel.gameObject.SetActive(false);
        workListPanel.Show();
        topPanel.gameObject.SetActive(true);
    }
    public void OnPostListPanel(object[] ps = null)
    {
        if(ps == null)
        {
            postListPanel.Show();
        }
        else if((ps[0] is BranchWorkDataSO))
        {
            postListPanel.Show((ps[0] as BranchWorkDataSO).postDataList);
        }
        writerInfoPanel.gameObject.SetActive(false);
        workListPanel.gameObject.SetActive(false);
        postPanel.gameObject.SetActive(false);
        topPanel.gameObject.SetActive(false);
    }
    public void OnPostPanel(object[] ps)
    {
        if (ps == null || !(ps[0] is BranchPostDataSO))
        {
            return;
        }
        BranchPostDataSO postData = ps[0] as BranchPostDataSO;

        HideAllPanel();


        postPanel.Show(postData);
    }
    public void HideAllPanel(object[] ps = null)
    {
        workListPanel.gameObject.SetActive(false);
        postPanel.gameObject.SetActive(false);
        postListPanel.HidePanel();
        writerInfoPanel.gameObject.SetActive(false);
        topPanel.gameObject.SetActive(false);
    }
    #endregion
    protected override void ResetSite()
    {
        base.ResetSite();
    }
    protected override void ShowSite()
    {
        base.ShowSite();
    }

    private void OnDestroy()
    {
        EventManager.StopAllListening(EBranchEvent.HideAllPanel);
        EventManager.StopAllListening(EBranchEvent.ShowWorkPanel);
        EventManager.StopAllListening(EBranchEvent.ShowPostList);
        EventManager.StopAllListening(EBranchEvent.ShowPost);

    }



}
