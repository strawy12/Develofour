using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

// 접근지정자 키워드 반환형 이름()

public class Window : MonoUI, IPointerClickHandler, ISelectable
{
    [SerializeField]
    private WindowBar windowBar;
    [SerializeField]
    protected WindowDataSO windowData;

    private bool isSelected;
    private bool isMaximum;
    public bool IsSelected { get { return isSelected; } }
    private RectTransform rectTransform;

    public Action<int> OnClosed;

    public Action OnSelected { get; set; }
    public Action OnUnSelected { get; set; }
    public WindowDataSO WindowData { get { return windowData; } }

    private void Start()
    {
        Init();
    }

    protected virtual void Init()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        windowBar.Init(windowData, rectTransform);
        isMaximum = false;


        windowBar.OnClose?.AddListener(WindowClose);
        windowBar.OnMinimum?.AddListener(WindowMinimum);
        windowBar.OnMaximum?.AddListener(WindowMaximum);
        windowBar.OnSelected += SelectWindow;
    }

    public void WindowClose()
    {
        OnClosed?.Invoke(windowData.windowTitleID);
        Destroy(gameObject);
    }
    
    public void WindowMinimum()
    {
        WindowManager.Inst.SelectedObjectNull();
        SetActive(false);
    }

    public void WindowMaximum()
    {
        if(!isMaximum)
        {
            rectTransform.sizeDelta = Constant.MAXWINSIZE;
            isMaximum = true;
        }
        else
        {
            rectTransform.sizeDelta = windowData.size;
            isMaximum = false;
        }
    }

    public void WindowOpen()
    {
        WindowManager.Inst.SelectObject(this);
        SetActive(true);
    }

    public void CreatedWindow()
    {
        Init();
        WindowOpen();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectWindow();
    }

    private void SelectWindow()
    {
        WindowManager.Inst.SelectObject(this);

    }
}
