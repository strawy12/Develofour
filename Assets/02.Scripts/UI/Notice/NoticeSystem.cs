using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.IK;
using TMPro.EditorUtilities;

public class NoticeSystem : MonoUI
{
    public static Action<ENoticeType, float> OnGeneratedNotice;
    //public static Action<Decision, float> OnDecisionPanel;

    [SerializeField]
    private NoticePanel noticePanelTemp;

    [SerializeField]
    private Transform noticePanelParant;

    private Queue<NoticePanel> noticePanelQueue;

    private Stack<NoticePanel> noticePanelPool;

    private RectTransform rectTransform;

    private NoticePanel noticePanel;

    private bool isOpen = false;
    private void Awake()
    {
        Bind();
    }

    private void Start()
    {
        Init();
    }

    private void Bind()
    {
        noticePanelQueue = new Queue<NoticePanel>();
        noticePanelPool = new Stack<NoticePanel>();

        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Init()
    {
        FixedNoticePanelInit();

        OnGeneratedNotice += ShowNoticePanel;

        EventManager.StartListening(ENoticeEvent.ClickNoticeBtn, ToggleNotice);
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
        EventManager.StartListening(ENoticeEvent.DiscordNotice, DiscordNotice);
    }

    private void FixedNoticePanelInit()
    {
        for(int i = 0; i < noticePanelParant.childCount; i++)
        {
            NoticePanel panel = noticePanelParant.GetChild(i).GetComponent<NoticePanel>();
            panel.Init(false);
        }
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
        if (isOpen)
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
        rectTransform.DOAnchorPosX(rectTransform.rect.width, Constant.NOTICE_DURATION).OnComplete(() =>
        {
            SetActive(false);
        });
    }

    //public void OpenDecisionNotice(Decision decision, float delay)
    //{
    //    string head = decision.decisionName;
    //    string body = "작업을 완료했습니다.";
    //    Sprite sprite = ;

    //}

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
        if (data == null)
        {
            Debug.LogError("Head나 Body 의 데이터가 없습니다");
            return;
        }

        if (string.IsNullOrEmpty(data.Head) || string.IsNullOrEmpty(data.Body))
        {
            Debug.LogError("Head나 Body 의 데이터가 없습니다");
            return;
        }

        var noticeList = noticePanelQueue.Where((x) => x.HeadText == data.Head);
        if(noticeList.Count() >= 1) {
            Debug.Log("이미 있는 알람임");
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

        NoticePanel panel = noticePanel = GetPanel(data.CanDeleted);
        panel.OnCompeleted += IncludePanel;
        panel.OnClosed += PushPanel;
        panel.Notice(data);
    }

    public void IncludePanel(NoticePanel panel)
    {
        panel.transform.SetParent(noticePanelParant);
        panel.OnCompeleted -= IncludePanel;
        noticePanel = null;
    }

    private void PushPanel(NoticePanel panel)
    {
        panel.gameObject.SetActive(false);
        panel.OnClosed -= PushPanel;
        noticePanelPool.Push(panel);
    }

    private NoticePanel GetPanel(bool canDelete)
    {
        NoticePanel panel;
        if (noticePanelPool.Count <= 0)
        {
            panel = Instantiate(noticePanelTemp, Define.WindowCanvasTrm);
            panel.Init(canDelete);
        }
        else
        {
            panel = noticePanelPool.Pop();
            panel.transform.SetParent(Define.WindowCanvasTrm);
        }

        panel.gameObject.SetActive(true);

        return panel;
    }

    private void DiscordNotice(object[] param)
    {
        if (!(param[0] is string) || !(param[1] is string) || !(param[2] is Sprite)) return;

        NoticePanel panel = noticePanel = GetPanel(true);

        string head = param[0] as string;
        string body = param[1] as string;
        Sprite icon = param[2] as Sprite;

        //panel.Notice(head, body, icon);
    }
}
