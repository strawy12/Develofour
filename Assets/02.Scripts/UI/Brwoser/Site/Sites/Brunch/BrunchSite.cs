using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
public class BrunchSite : Site
{

    [Header("WriterInfo")]
    [SerializeField]
    private GameObject writerInfoPanel;
    [SerializeField]
    private Button writerInfoBtn;
    [Header("PostList")]
    [SerializeField]
    private Transform postParent;
    [SerializeField]
    private Button postListBtn;
    [SerializeField]
    private BrunchPost brunchPostPrefab;
    [SerializeField]
    private PostPanelParent postListPanel;
    [SerializeField]
    private List<BrunchPostDataSO> postDatas = new List<BrunchPostDataSO>();
    [SerializeField]
    private TMP_Text postCntText;
    [Header("WorkList")]
    [SerializeField]
    private GameObject workListPanel;
    [SerializeField]
    private Button workListBtn;
    

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
        postListPanel.ChangeVerticalUICount(postDatas.Count);
        foreach (BrunchPostDataSO postData in postDatas)
        {
            BrunchPost post = Instantiate(brunchPostPrefab, postParent);
            post.Init(postData);
            post.OnRemove += RemovePost;
            post.gameObject.SetActive(true);
        }
        postCntText.text = $"글 {postDatas.Count}";
    }

    private void RemovePost(BrunchPost post)
    {
        postDatas.Remove(post.PostData);
        postListPanel.ChangeVerticalUICount(postDatas.Count);
        postCntText.text = $"글 {postDatas.Count}";
        
        if(postDatas.Count <= 0)
        {
            EventManager.TriggerEvent(EQuestEvent.EndBrunchPostCleanUp);
        }
    }
}
