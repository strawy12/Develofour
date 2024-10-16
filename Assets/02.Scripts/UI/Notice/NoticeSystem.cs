﻿using DG.Tweening;
using JetBrains.Annotations;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic; 
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.U2D.IK;

public class NoticeSystem : MonoUI
{
    public static Action<ENoticeType, float> OnGeneratedNotice;
    public static Action OnTagReset;
    //public static Action<Decision, float> OnDecisionPanel;
    public static System.Action<string, string, float, bool, Sprite,Color, ENoticeTag, string> OnNotice;

    [SerializeField]
    private NoticePanel noticePanelTemp;
    [SerializeField]
    private NoticeOutline noticeOutline;
    [SerializeField]
    private Transform noticePanelParant;
    [SerializeField]
    private NoticePanel celebrationNotice;

    private Stack<NoticePanel> noticePanelPool;

    private RectTransform rectTransform;

    private NoticePanel noticePanel;

    private bool isOpen = false;
    private bool isPlaying = false; 


    private ENoticeTag currentTag;

    [SerializeField]
    private List<NoticeData> saveNoticeList = new List<NoticeData>();

    public int maxExtend = 3;
    private int extendCount = 0;

    [SerializeField]
    private ProfileOverlayOpenTrigger overlayTrigger;

    private void Awake()
    {
        Bind();
    }

    private void Start()
    {
        GameManager.Inst.OnGameStartCallback += Init;
        GameManager.Inst.OnReset += NoticeReset;
    }
    private void NoticeReset()
    {
        saveNoticeList.Clear();
        DataManager.Inst.SaveData.saveNoticeData.Clear();
        List<NoticePanel> noticePanels = noticePanelParant.GetComponentsInChildren<NoticePanel>().ToList();
        noticePanels.Remove(celebrationNotice);
        noticePanels.ForEach(x => Destroy(x.gameObject));
    }
    private void Bind()
    {
        noticePanelPool = new Stack<NoticePanel>();

        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        overlayTrigger ??= GetComponent<ProfileOverlayOpenTrigger>();
    }

    private void Init()
    {
        FixedNoticePanelInit();

        OnGeneratedNotice += ShowNoticePanel;
        OnNotice += Notice;
        OnTagReset += TagReset;
        EventManager.StartListening(ENoticeEvent.ClickNoticeBtn, ToggleNotice);
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckClose);
        currentTag = ENoticeTag.None;

        LoadSaveNotice();
    }

    private void TagReset()
    {
        currentTag = ENoticeTag.None;
    }

    private void FixedNoticePanelInit()
    {
        for (int i = 0; i < noticePanelParant.childCount; i++)
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
        if (isOpen || isPlaying) return;

        if(overlayTrigger != null)
        {
            overlayTrigger.fileID = Constant.OverlayID.NOTIFICATION_SYSTEM.ToString();
            overlayTrigger.Open();
        }

        isOpen = true;
        isPlaying = true;
        EventManager.TriggerEvent(ENoticeEvent.OpenNoticeSystem);

        SetActive(true);

        rectTransform.DOKill();
        rectTransform.DOAnchorPosX(0f, Constant.NOTICE_DURATION).OnComplete(() => isPlaying = false);
    }

    public void Close()
    {
        if (!isOpen || isPlaying) return;
        overlayTrigger.Close();
        isOpen = false;
        isPlaying = true;
        rectTransform.DOKill();
        rectTransform.DOAnchorPosX(rectTransform.rect.width, Constant.NOTICE_DURATION).OnComplete(() =>
        {
            SetActive(false);
            isPlaying = false;
        });
    }

    //public void OpenDecisionNotice(Decision decision, float delay)
    //{
    //    string head = decision.decisionName;
    //    string body = "작업을 완료했습니다.";
    //    Sprite sprite = ;

    //}

    public void ShowNoticePanel(ENoticeType eNoticeDataType, float delay)
    {
        NoticeDataSO data = ResourceManager.Inst.GetNoticeData(eNoticeDataType);

        if (data.noticeTag == currentTag && data.noticeTag != ENoticeTag.None && noticePanel != null)
        {
            extendCount++;
            if (extendCount == maxExtend)
            {
                noticePanel.ImmediatelyStop();
            }
            else
            {
                noticePanel.SameTagTextAdd(data.Body);
                return;
            }
        }

        extendCount = 0;

        currentTag = data.noticeTag;

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

        //var noticeList = noticePanelQueue.Where((x) => x.HeadText == data.Head);
        //if(noticeList.Count() >= 1) {
        //    Debug.Log("이미 있는 알람임");
        //    return;
        //}

        saveNoticeList.Add(data.noticeData);
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
        noticeOutline.StartOutline();
        panel.Notice(data, isOpen);
    }

    public void IncludePanel(NoticePanel panel)
    {
        panel.CanvasSortingSetting();
        panel.transform.SetParent(noticePanelParant);
        panel.OnCompeleted -= IncludePanel;
        panel.transform.localScale = Vector3.one;
        noticePanel = null;
    }

    private void PushPanel(NoticePanel panel)
    {
        panel.gameObject.SetActive(false);
        panel.OnClosed -= PushPanel;
        foreach (var temp in saveNoticeList)
        {
            if (panel.headText.text == temp.head)
            {
                saveNoticeList.Remove(temp);
                break;
            }
        }
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

    private void Notice(string head, string body, float delay, bool canDelete, Sprite icon, Color color, ENoticeTag noticeTag, string fileID = null)
    {
        if (noticeTag == currentTag && noticeTag != ENoticeTag.None && noticePanel != null)
        {
            extendCount++;
            if (extendCount == maxExtend)
            {
                noticePanel.ImmediatelyStop();
            }
            else
            {
                noticePanel.SameTagTextAdd(body);
                return;
            }
        }

        extendCount = 0;

        EventManager.TriggerEvent(ENoticeEvent.OpenNoticeSystem);
        NoticePanel panel = noticePanel = GetPanel(true);
        panel.OnCompeleted += IncludePanel;
        panel.OnClosed += PushPanel;
        noticeOutline.StartOutline();

        currentTag = noticeTag;

        if (!isOpen)
        {
            panel.Notice(head, body, icon, color, false, fileID);
        }
        else
        {
            panel.Notice(head, body, icon, color, true, fileID);
        }

        NoticeData data = new NoticeData();
        data.head = head;
        data.body = body;
        data.icon = icon;
        data.canDeleted = canDelete;
        data.delay = delay;
        data.tag = noticeTag;
        data.fileID = fileID;
        //so에 있는 노티스태그 데이타에도 넣어

        saveNoticeList.Add(data);
    }

    private void OnApplicationQuit()
    {
        SaveNoticeData();
    }

    private void SaveNoticeData()
    {
        if (!DataLoadingScreen.completedDataLoad) return;
        DataManager.Inst.SetNoticeDataSave(saveNoticeList);
    }

    private void LoadSaveNotice()
    {
        saveNoticeList = DataManager.Inst.SaveData.saveNoticeData;
        foreach (NoticeData data in saveNoticeList)
        {
            NoticePanel panel = GetPanel(data.canDeleted);
            panel.transform.SetParent(noticePanelParant);
            panel.OnClosed += PushPanel;
            panel.LoadNotice(data);
            panel.SetActive(true);
            panel.isEndNotice = true;
        }
    }
}
