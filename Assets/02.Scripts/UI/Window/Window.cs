﻿using System;
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
    TodoWindow,
    End
}

[RequireComponent(typeof(GraphicRaycaster))]
public abstract class Window : MonoUI, IPointerClickHandler, ISelectable
{
    public static int windowMaxCnt;
    public static Window currentWindow;

    [Header("Window Data")]
    [SerializeField]
    protected WindowAlterationSO windowAlteration; // 위도우 위치 크기 정보
    protected FileSO file;

    [SerializeField]
    protected WindowBar windowBar;

    protected bool isSelected;

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
    private Canvas currentCanvas;


    protected virtual void Init()
    {
        currentCanvas = GetComponent<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        windowBar.Init(windowAlteration, file, rectTransform);
        OnSelected += () => WindowSelected(true);
        OnUnSelected += () => WindowSelected(false);

        windowBar.OnClose?.AddListener(WindowClose);
        windowBar.OnMinimum?.AddListener(WindowMinimum);
        windowBar.OnMaximum?.AddListener(WindowMaximum);
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
        OnClosed?.Invoke(file.name);

        windowMaxCnt--;

        if (isSelected)
        {
            WindowManager.Inst.SelectedObjectNull();
        }

        OnDestroy();
        Destroy(gameObject);
    }

    public void WindowMinimum()
    {
        WindowManager.Inst.SelectedObjectNull();
        SetActive(false);
    }

    public void WindowMaximum()
    {
        if (!windowAlteration.isMaximum)
        {
            Vector2 size = Constant.MAX_CANVAS_SIZE;
            size.y -= 50;
            rectTransform.sizeDelta = size;

            windowPos = rectTransform.localPosition;
            rectTransform.localPosition = new Vector3(0, 25, 0);

            windowAlteration.isMaximum = true;
        }
        else
        {
            rectTransform.localPosition = windowPos;
            rectTransform.sizeDelta = windowAlteration.size;

            windowAlteration.isMaximum = false;
        }
    }

    public void WindowOpen()
    {
        WindowManager.Inst.SelectObject(this);

        SetCurrentWindow(this);
        SetActive(true);

    }

    public void SetCurrentWindow(Window selecetedWindow)
    {
        currentWindow = selecetedWindow;
    }

    public void CreatedWindow(FileSO file)
    {
        this.file = file;
        Init();
        WindowOpen();
        windowMaxCnt++;

        EventManager.TriggerEvent(EWindowEvent.CreateWindow, new object[] { this });
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectWindow();
    }

    private void CheckSelected(object[] hits)
    {
        if(Define.ExistInFirstHits(gameObject, hits[0]))
        {
            SelectWindow();
        }
    }

    protected void SelectWindow()
    {
        WindowManager.Inst.SelectObject(this);
        SetCurrentWindow(this);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(ECoreEvent.LeftButtonClick, CheckSelected);
    }
    private void OnEnable()
    {
        EventManager.StartListening(ECoreEvent.LeftButtonClick, CheckSelected);
    }
    private void OnDisable()
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
