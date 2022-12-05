using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.IK;



public class NoticeSystem : MonoUI
{
    public static Action<ENoticeType, float> OnGeneratedNotice;

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

        EventManager.StartListening(ENoticeEvent.ClickNoticeBtn, ToggleNotice);
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
    }
 
    private void CheckClose(object[] hits)
    {
        if (isOpen == false) return;
        if (Define.ExistInHits(gameObject, hits[0]) == false)
        {
            Close();
        }
    }

    private void ToggleNotice(object[] obj)
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

        EventManager.TriggerEvent(ENoticeEvent.OpenNoticeSystem);

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

    public NoticeDataSO GetTextData(ENoticeType noticeDataType)
    {
        NoticeDataSO noticeDataSO = null;

        try
        {
            noticeDataSO = Resources.Load<NoticeDataSO>($"NoticeData/NoticeData_{noticeDataType.ToString()}");
        }
        catch (System.NullReferenceException e)
        {
            Debug.Log($"NoticeData {noticeDataType} is null\n{e}");
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }

        return noticeDataSO;
    }


    public void ShowNoticePanel(ENoticeType eNoticeDataType, float delay)
    {
        NoticeDataSO data = GetTextData(eNoticeDataType);

        if(data == null)
        {
            Debug.LogError("Head�� Body �� �����Ͱ� �����ϴ�");
            return;
        }

        if (string.IsNullOrEmpty(data.Head) || string.IsNullOrEmpty(data.Body))
        {
            Debug.LogError("Head�� Body �� �����Ͱ� �����ϴ�");
            return;
        }

        StartCoroutine(NoticeCoroutine(data, delay));
    }

    private IEnumerator NoticeCoroutine(NoticeDataSO data, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (noticePanel != null)
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
