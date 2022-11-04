using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public abstract class Window : MonoUI, IPointerClickHandler, ISelectable
{
    [SerializeField]
    protected WindowBar windowBar;
    [SerializeField]
    protected WindowDataSO windowData;

    protected bool isSelected;
    protected bool isMaximum;

    public bool IsSelected { get { return isSelected; } }
    protected RectTransform rectTransform;

    public Action<int> OnClose;

    public Action OnSelected { get; set; }
    public Action OnUnSelected { get; set; }
    public WindowDataSO WindowData { get { return windowData; } }
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
        OnClose?.Invoke(windowData.windowTitleID);
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

    protected void SelectWindow()
    {
        WindowManager.Inst.SelectObject(this);

    }
}
