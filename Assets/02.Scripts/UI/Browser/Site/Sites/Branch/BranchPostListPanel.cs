using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BranchPostListPanel : MonoBehaviour
{
    [SerializeField]
    private Button backBtn;
    [Header("Pool")]
    [SerializeField]
    private BranchPostLine postLinePrefab;
    [SerializeField]
    private Transform postParent;
   
    private Queue<BranchPostLine> poolQueue;
    private int poolCnt = 15;
    private List<BranchPostLine> usePostLine;

    private List<BranchPostDataSO> currentPostList;

    private RectTransform rectTransform;
    public void Init()
    {
        Bind();

        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(() => EventManager.TriggerEvent(EBranchEvent.ShowWorkPanel));
        CreatePool();
    }

    private void Bind()
    {
        rectTransform = GetComponent<RectTransform>();
        poolQueue = new Queue<BranchPostLine>();
        usePostLine = new List<BranchPostLine>();
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

    public void Show(List<BranchPostDataSO> postList = null)
    {
        if(postList != null)
        {
            currentPostList = postList;
        }

        foreach (var postData in currentPostList)
        {
            BranchPostLine postLine = Pop();
            postLine.Init(postData);
            postLine.gameObject.SetActive(true);
            usePostLine.Add(postLine);
        }

        gameObject.SetActive(true);

        RectTransform workParentRect = (RectTransform)postParent;
        LayoutRebuilder.ForceRebuildLayoutImmediate(workParentRect);
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, workParentRect.sizeDelta.y);

    }

    public void HidePanel()
    {
        PushAll();
        gameObject.SetActive(false);
    }
}
