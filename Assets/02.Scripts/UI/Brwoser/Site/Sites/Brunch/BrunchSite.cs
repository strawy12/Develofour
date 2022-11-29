using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrunchSite : Site
{
    [SerializeField]
    private GameObject writerInfoPanel;
    [SerializeField]
    private PostPanelParent postListPanel;
    [SerializeField]
    private GameObject workListPanel;
    [SerializeField]
    private Button writerInfoBtn;
    [SerializeField]
    private Button postListBtn;
    [SerializeField]
    private Button workListBtn;
    [SerializeField]
    private BrunchPost brunchPostPrefab;
    [SerializeField]
    private Transform postParent;

    [SerializeField]
    private List<BrunchPostDataSO> postDatas = new List<BrunchPostDataSO>();

    public void Awake()
    {
        workListBtn.onClick.AddListener(OnWorkListPanel);
        postListBtn.onClick.AddListener(OnPostListPanel);
        writerInfoBtn.onClick.AddListener(OnWriterInfoPanel);

        CreatePost();
    }

    public override void Init()
    {
        base.Init();
    }
    #region OnOffPanel
    public void OnWriterInfoPanel()
    {
        postListPanel.gameObject.SetActive(false);
        workListPanel.gameObject.SetActive(false);
        writerInfoPanel.gameObject.SetActive(true);
    }
    public void OnPostListPanel()
    {
        workListPanel.gameObject.SetActive(false);
        writerInfoPanel.gameObject.SetActive(false);
        postListPanel.gameObject.SetActive(true);
    }
    public void OnWorkListPanel()
    {
        writerInfoPanel.gameObject.SetActive(false);
        postListPanel.gameObject.SetActive(false);
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
    
    private void CreatePost()
    {
        postListPanel.Init(postDatas.Count);
        foreach (BrunchPostDataSO postData in postDatas)
        {
            BrunchPost post = Instantiate(brunchPostPrefab, postParent);
            post.Init(postData);
            post.gameObject.SetActive(true);
        }

    }
}
