using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct NoticeData
{
    public string head;
    public string body;
}

public class NoticeSystem : MonoBehaviour
{
    public static Action<NoticeData> OnGeneratedNotice;

    [SerializeField]
    private NoticePanel noticePanelTemp;

    [SerializeField]
    private Transform noticePanelParant;

    [SerializeField]
    private int maxPanelCount = 6;

    private Queue<NoticePanel> noticePanelQueue;

    private Stack<NoticePanel> noticePanelPool;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;

    private NoticePanel noticePanel;

    private bool isOpen = false; 

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        noticePanelQueue = new Queue<NoticePanel>();
        noticePanelPool = new Stack<NoticePanel>();

        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        OnGeneratedNotice += ShowNoticePanel;
    }


    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        EventManager.TriggerEvent(EEvent.OpenNoticeSystem);

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        
        rectTransform.DOKill();
        rectTransform.DOAnchorPosX(0f, Constant.NOTICE_DURATION);
    }
    public void Close()
    {
        if (!isOpen) return;
        isOpen = false;
        rectTransform.DOKill();
        rectTransform.DOAnchorPosX(rectTransform.rect.width, Constant.NOTICE_DURATION).OnComplete(()=>
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        });
    }

    public void ShowNoticePanel(NoticeData data)
    {
        if (string.IsNullOrEmpty(data.head) || string.IsNullOrEmpty(data.body))
        {
            Debug.LogError("Head나 Body 의 데이터가 없습니다");
            return;
        }

        if(noticePanel != null)
        {
            noticePanel.ImmediatelyStop();
        }    

        NoticePanel panel = noticePanel = GetPanel();
        panel.Init();
        panel.OnCompeleted += EnqueuePanel;

        panel.Notice(data);
    }

    public void EnqueuePanel(NoticePanel panel)
    {
        if (noticePanelQueue.Count > maxPanelCount)
        {
            NoticePanel temp = noticePanelQueue.Dequeue();
            Destroy(temp);
        }

        panel.transform.SetParent(noticePanelParant);
        noticePanelQueue.Enqueue(panel);

        panel.OnCompeleted -= EnqueuePanel;
        noticePanel = null;
    }

    private NoticePanel GetPanel()
    {
        NoticePanel panel;
        if (noticePanelPool.Count <= 0)
        {
            panel = Instantiate(noticePanelTemp, Define.WindowCanvasTrm);
        }

        else
        {
            panel = noticePanelPool.Pop();
        }

        panel.gameObject.SetActive(true);

        return panel;
    }

   

}
