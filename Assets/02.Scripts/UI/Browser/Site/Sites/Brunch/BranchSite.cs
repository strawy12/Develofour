using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
public class BranchSite : Site
{

    [Header("WriterInfo")]
    [SerializeField]
    private GameObject writerInfoPanel;
    [SerializeField]
    private Button writerInfoBtn;
    [Header("WorkList")]
    [SerializeField]
    private GameObject workListPanel;
    [SerializeField]
    private Button workListBtn;
    [SerializeField]
    private List<BranchWorkDataSO> workDataList;
    [SerializeField]
    private BranchSiteWorkPanel workPanelPrefab;
    [SerializeField]
    private Transform workPanelParent;
    [SerializeField]
    private Dictionary<EBranchWorkCategory ,BranchSiteWorkPanel> workPanels = new Dictionary<EBranchWorkCategory, BranchSiteWorkPanel>();
    public void Awake()
    {
        workListBtn.onClick.AddListener(OnWorkListPanel);
        writerInfoBtn.onClick.AddListener(OnWriterInfoPanel);

        CreateWork();
    }

    public override void Init()
    {
        base.Init();
    }
    #region OnOffPanel
    public void OnWriterInfoPanel()
    {
        workListPanel.gameObject.SetActive(false);
        writerInfoPanel.gameObject.SetActive(true);
    }
    public void OnWorkListPanel()
    {
        writerInfoPanel.gameObject.SetActive(false);
        workListPanel.gameObject.SetActive(true);
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
    
    private void CreateWork()
    {
        foreach(BranchWorkDataSO workData in workDataList)
        {
            BranchSiteWorkPanel panel = Instantiate(workPanelPrefab, workPanelParent);
            panel.Init(workData);
            panel.gameObject.SetActive(true);
            workPanels.Add(workData.workKey, panel);
        }
    }

}
