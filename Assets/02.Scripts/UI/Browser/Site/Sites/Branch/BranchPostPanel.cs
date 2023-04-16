using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;




public class BranchPostPanel : MonoBehaviour
{
    private BranchPostDataSO postData;

    [SerializeField]
    private BranchPostLockPanel lockPanel;

    [SerializeField]
    private Button backBtn;
    
    [SerializeField]
    private RectTransform contentRect;

    private RectTransform rectTransform;

    [SerializeField]
    private List<BranchPostPrefab> postPrefabList;


    private Dictionary<BranchPostDataSO, BranchPostPrefab> postPrefabs;


    public void Init()
    {
        Bind();

        backBtn.onClick.RemoveAllListeners();
        backBtn.onClick.AddListener(() => EventManager.TriggerEvent(EBranchEvent.ShowPostList));
        postPrefabs.Clear();
        foreach (var post in postPrefabList)
        {
            postPrefabs.Add(post.postData, post);
        }
    }

    private void Bind()
    {
        rectTransform ??= GetComponent<RectTransform>();
        postPrefabs = new Dictionary<BranchPostDataSO, BranchPostPrefab>();
    }

    public void Show(BranchPostDataSO newPostData)
    {
        postData = newPostData;

        gameObject.SetActive(true);

        if (!DataManager.Inst.GetBranchUnLockData(postData) && postData.postPassword != "")
        {
            lockPanel.Show(postData);
            return;
        }else
        {
            lockPanel.Hide();
        }

        foreach (var prefabData in postPrefabs)
        {
            if(prefabData.Key == postData )
            {
                prefabData.Value.Show();
            }
            else
            {
                prefabData.Value.Hide();
            }
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(contentRect);
        rectTransform.sizeDelta = new Vector3(rectTransform.sizeDelta.x, contentRect.sizeDelta.y);
        
    }
}
