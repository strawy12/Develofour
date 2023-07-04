using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum EWindowType // 확장자
{
    None,
    Notepad,
    Browser,
    ImageViewer,
    Discord,
    Directory,
    Installer,
    ProfilerWindow,
    WindowPinLock,
    MediaPlayer,
    IconProperty,
    Popup,
    Calculator,
    SiteShortCut,
    HarmonyShortCut,
    Dummy,
    VideoPlayer,
    SoundPlayer,
    BackgroundBGM,
    End 
}

[RequireComponent(typeof(GraphicRaycaster))]
public class Window : MonoUI, ISelectable
{
    public static int windowMaxCnt;
    public static Window currentWindow;

    public int openInt = 0;


    [Header("Window Data")]
    [SerializeField]
    public WindowAlterationSO originWindowAlteration; // 위도우 위치 크기 정보

    protected WindowAlterationSO windowAlteration;

    public Vector2 WindowSize
    {
        get
        {
            return windowAlteration.size;
        }
    }

    protected FileSO file;

    [SerializeField]
    public WindowBar windowBar;

    protected bool isSelected;

    protected bool canNotClosed;

    protected RectTransform rectTransform;

    public Action<string> OnClosed;
    public Func<bool> OnUnSelectIgnoreFlag;

    public Action OnSelected { get; set; }
    public Action OnUnSelected { get; set; }

    public FileSO File
    {
        get
        {
            return file;
        }
    }

    public int SortingOrder
    {
        get => currentCanvas.sortingOrder;
        set => currentCanvas.sortingOrder = value;
    }

    private Vector3 windowPos;
    protected Canvas currentCanvas;
    protected Image windowPanel;
    protected virtual void Init()
    {
        currentCanvas = GetComponent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
        windowPanel = GetComponent<Image>();

        windowAlteration = Instantiate(originWindowAlteration);

        if (file == null)
        {
            windowBar.Init(windowAlteration, rectTransform);
        }
        else
        {
            windowBar.Init(windowAlteration, file, rectTransform);
        }

        OnSelected += () => WindowSelected(true);
        OnUnSelected += () => WindowSelected(false);

        if (windowBar.OnClose != null)
        {
            windowBar.OnClose?.AddListener(WindowClose);
        }

        if (windowBar.OnMinimum != null)
        {
            windowBar.OnMinimum?.AddListener(WindowMinimum);
        }

        if (windowBar.OnMaximum != null)
        {
            windowBar.OnMaximum?.AddListener(WindowMaximum);
        }

        windowBar.OnSelected += SelectWindow;
    }

    // SelectableObject를 위한 함수
    public bool IsSelected(GameObject hitObject)
    {
        // 지금 현재 클릭한 오브젝트와 내 오브젝트가 같거나
        bool flag1 = hitObject == gameObject;

        // 선택 취소 무시 플래그가 true 이거나  
        bool flag2 = OnUnSelectIgnoreFlag != null && OnUnSelectIgnoreFlag.Invoke();

        // 선택되었다고한다면
        return (flag1 && isSelected) || flag2;
    }

    public bool IsSelected()
    {
        bool flag = OnUnSelectIgnoreFlag != null && OnUnSelectIgnoreFlag.Invoke();
        return isSelected || flag;
    }

    public void WindowSelected(bool windowSelected)
    {
        if (isSelected == windowSelected) return;

        isSelected = windowSelected;
    }

    public void WindowClose()
    {
        if (canNotClosed) return;
        CloseEventAdd();
        OnClosed?.Invoke(file.fileName);

        windowMaxCnt--;

        if (isSelected)
        {
            WindowManager.Inst.SelectedObjectNull();
        }
        EventManager.StopListening(EWindowEvent.AlarmSend, AlarmCheck);
        OnDestroyWindow();
        Destroy(gameObject);
    }

    public virtual void WindowMinimum()
    {
        WindowManager.Inst.SelectedObjectNull();
        SetActive(false);
    }

    public virtual void WindowMaximum()
    {
        if (!windowAlteration.isMaximum)
        {
            Vector2 size = Constant.MAX_CANVAS_SIZE;
            size.y -= 50;
            rectTransform.sizeDelta = size;
            windowAlteration.size = new Vector2(1920, 1030);
            windowPos = rectTransform.localPosition;
            rectTransform.localPosition = Constant.WINDOWMAXIMUMPOS;

            windowAlteration.isMaximum = true;
        }
        else
        {
            windowAlteration.isMaximum = false;

            windowAlteration.size = new Vector2(1280, 720);
            rectTransform.localPosition = windowAlteration.pos;
            rectTransform.sizeDelta = windowAlteration.size;
        }
    }

    public virtual void WindowOpen()
    {
        WindowManager.Inst.SelectObject(this);

        SetCurrentWindow(this);

        windowBar.OnClose?.AddListener(CloseEventAdd);

        if (!windowAlteration.isMaximum)
        {
            Vector2 pos = windowAlteration.pos + new Vector2(20 * (openInt), -20 * (openInt));
            rectTransform.localPosition = pos;
        }
        else
        {
            rectTransform.localPosition = Constant.WINDOWMAXIMUMPOS;
        }
        SizeDoTween();
    }
    public virtual void SizeDoTween()
    {
        float minDuration = 0.16f;
        rectTransform.localScale = new Vector2(0.9f, 0.9f);

        Sequence sequence = DOTween.Sequence();
        sequence.Join(rectTransform.DOScale(1, minDuration));
        sequence.AppendCallback(() => SetActive(true));
        DataManager.Inst.AddLastAccessDateData(file.id, TimeSystem.TimeCount());
    }
    public void CloseEventAdd()
    {
        if (WindowManager.Inst != null && file != null && this != null)
        {
            WindowManager.Inst.RemoveWindowDictionary(file.windowType, this);
        }
    }

    public void SetCurrentWindow(Window selecetedWindow)
    {
        currentWindow = selecetedWindow;
    }

    public virtual void CreatedWindow(FileSO file)
    {
        this.file = file;
        Init();

        windowMaxCnt++;
        EventManager.StartListening(EWindowEvent.AlarmSend, AlarmCheck);
        EventManager.TriggerEvent(EWindowEvent.CreateWindow, new object[] { this });
    }

    //public void OnPointerClick(PointerEventData eventData)
    //{
    //    SelectWindow();
    //}

    private void CheckSelected(object[] hits)
    {
        if (Define.ExistInFirstHits(gameObject, hits[0]))
        {
            SelectWindow();
        }
    }

    protected void SelectWindow()
    {
        WindowManager.Inst.SelectObject(this);
        SetCurrentWindow(this);
        EventManager.TriggerEvent(EWindowEvent.AlarmCheck, new object[1] { file.windowType });
    }

    protected void AlarmCheck(object[] ps)
    {
        if (!(ps[0] is EWindowType))
        {
            return;
        }
        EWindowType type = (EWindowType)ps[0];
        if (type != file.windowType)
        {
            return;
        }

        if (!isSelected)
        {
            EventManager.TriggerEvent(EWindowEvent.AlarmRecieve, ps);
        }
    }
    //사용법
    //EWindowType type = EWindowType.ProfileWindow;
    //EventManager.TriggerEvent(EWindowEvent.AlarmSend, new object[1] { type });

    protected virtual void OnDestroyWindow()
    { 
        DOTween.Kill(gameObject, true);
        EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckSelected);
    }
    protected virtual void OnEnable()
    {
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckSelected);
    }
    protected virtual void OnDisable()
    {
        EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckSelected);
    }

#if UNITY_EDITOR
    public void Reset()
    {
        windowBar = GetComponentInChildren<WindowBar>();
    }

#endif
}
