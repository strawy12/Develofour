using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.IK;

public struct NoticeData
{
    public string head;
    public string body;
}

public class NoticeSystem : MonoUI
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

        EventManager.StartListening(EEvent.ClickNoticeBtn, ToggleNotice);
        EventManager.StartListening(EEvent.LeftButtonClick, CheckClose);

        EventManager.StartListening(EEvent.EndTutorialScene, EndTutorialScene);

        EventManager.StartListening(EEvent.HateBtnClicked, HateBtnClicked);
    }

    private void EndTutorialScene(object emptyObj)
    {
        NoticeData data = new NoticeData();
        data.head = "AI ���� ���� �ݴ��ϱ�";
        data.body = "������ �Ⱦ�� ��ư�� �������ν� AI ���� ���� ���� ������ ��Ÿ������";
        ShowNoticePanel(data);
    }

    private void HateBtnClicked(object emptyParam)
    {
        NoticeData data = new NoticeData();
        data.head = "���� ����� ���� Ȯ���ϱ�";
        data.body = "���ã�⿡ �ִ� ���� �ٷΰ��� ��ư�� �̿��� �� �� �ֽ��ϴ�";
        ShowNoticePanel(data);
    }
    private void CheckClose(object hits)
    {
        if (isOpen == false) return;
        if (Define.ExistInHits(gameObject, hits) == false)
        {
            Close();
        }
    }

    private void ToggleNotice(object obj)
    {
        if(isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Open()
    {
        if (isOpen) return;
        isOpen = true;

        EventManager.TriggerEvent(EEvent.OpenNoticeSystem);

        SetActive(true);
        
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
            SetActive(false);
        });
    }

    public void ShowNoticePanel(NoticeData data)
    {
        if (string.IsNullOrEmpty(data.head) || string.IsNullOrEmpty(data.body))
        {
            Debug.LogError("Head�� Body �� �����Ͱ� �����ϴ�");
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

        panel.SetActive(true);

        return panel;
    }

   

}
