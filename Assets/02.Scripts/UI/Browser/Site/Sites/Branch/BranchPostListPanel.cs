using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchPostListPanel : MonoBehaviour
{
    [SerializeField]
    private PanelSettingChildVerticalSize settingChildVerticalSize;

    [Header("Pool")]
    [SerializeField]
    private BranchPostLine postLinePrefab;
    [SerializeField]
    private Transform postParent;
    private Queue<BranchPostLine> poolQueue;
    private int poolCnt = 15;
    private List<BranchPostLine> usePostLine;

    private List<BranchPostDataSO> currentPostList;

    private BranchPostPanel postPanel;
    public void Init(BranchPostPanel postPanel)
    {
        poolQueue = new Queue<BranchPostLine>();
        usePostLine = new List<BranchPostLine>();
        this.postPanel = postPanel;
        CreatePool();
    }
    private BranchPostLine Pop()
    {
        BranchPostLine branchPostLine = poolQueue.Dequeue();
        usePostLine.Add(branchPostLine);
        return branchPostLine;
    }

    private void PushAll()
    {
        foreach (var postLine in usePostLine)
        {
            postLine.Release();
            postLine.gameObject.SetActive(false);
            poolQueue.Enqueue(postLine);
        }
        usePostLine.Clear();
    }
    private void CreatePool()
    {
        for(int i = 0; i< poolCnt; i++)
        {
            BranchPostLine branchPostLine = Instantiate(postLinePrefab, postParent);
            branchPostLine.gameObject.SetActive(false);
            poolQueue.Enqueue(branchPostLine);
        }
    }

    public void Setting(List<BranchPostDataSO> postList)
    {
        currentPostList = postList;

        foreach (var postData in currentPostList)
        {
            BranchPostLine postLine = Pop();
            postLine.Init(postData, postPanel);
            postLine.gameObject.SetActive(true);
            usePostLine.Add(postLine);
        }

        settingChildVerticalSize.ChangeVerticalUICount(usePostLine.Count);
        gameObject.SetActive(true);
    }

    public void HidePanel()
    {
        PushAll();
        gameObject.SetActive(false);
    }
}
