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
        workListPanel.Init(workDataList, postListPanel);
        postListPanel.Init(postPanel);
        postPanel.Init(OnPostListPanel);

        EventManager.StartListening(EBranchEvent.HideAllPanel, HideAllPanel);
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
        postPanel.gameObject.SetActive(false);
        postListPanel.HidePanel();
        writerInfoPanel.gameObject.SetActive(false);
        workListPanel.Show();
        topPanel.gameObject.SetActive(true);
    }
    public void OnPostListPanel()
    {
        writerInfoPanel.gameObject.SetActive(false);
        workListPanel.Show();
        postListPanel.gameObject.SetActive(true);
        postPanel.gameObject.SetActive(false);
        topPanel.gameObject.SetActive(true);
    }

    public void HideAllPanel(object[] ps)
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

    }



}
