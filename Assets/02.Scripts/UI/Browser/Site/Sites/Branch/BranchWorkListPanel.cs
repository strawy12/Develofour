using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BranchWorkListPanel : MonoBehaviour
{
    [SerializeField]
    private BranchSiteWorkPanel workPanelPrefab;
    [SerializeField]
    private Transform workPanelParent;

    [SerializeField]
    private Dictionary<EBranchWorkCategory, BranchSiteWorkPanel> workPanels = new Dictionary<EBranchWorkCategory, BranchSiteWorkPanel>();

    private List<BranchWorkDataSO> workDataList;
    private RectTransform rectTransform;

    private BranchPostListPanel postListPanel;
    public void Init(List<BranchWorkDataSO> dataList, BranchPostListPanel postListPanel)
    {
        Bind();
        workDataList = dataList;
        this.postListPanel = postListPanel; 
        CreateWorkPanel();
    }

    private void Bind()
    {
        rectTransform ??= GetComponent<RectTransform>();
    }

    private void CreateWorkPanel()
    {
        foreach (BranchWorkDataSO workData in workDataList)
        {
            BranchSiteWorkPanel panel = Instantiate(workPanelPrefab, workPanelParent);
            panel.Init(workData, postListPanel);
            panel.gameObject.SetActive(true);
            workPanels.Add(workData.workKey, panel);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);

        RectTransform workParentRect = (RectTransform)workPanelParent;
        LayoutRebuilder.ForceRebuildLayoutImmediate(workParentRect);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, workParentRect.sizeDelta.y);
    }

    private IEnumerator ShowCoroutine(RectTransform workParentRect)
    {
        yield return new WaitForSeconds(0.5f);


    }

}
