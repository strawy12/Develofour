using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;


[RequireComponent(typeof(GraphicRaycaster))]
public abstract class Window : MonoUI, IPointerClickHandler, ISelectable
{
    public static int windowMaxCnt;
    public static Window currentWindow;

    [SerializeField]
    protected WindowBar windowBar;
    [SerializeField]
    protected WindowDataSO windowData;

    protected bool isSelected;

    public bool IsSelected { get { return isSelected; } }
    protected RectTransform rectTransform;

    public Action<int> OnClosed;

    public Action OnSelected { get; set; }
    public Action OnUnSelected { get; set; }
    
    public WindowDataSO WindowData { get { return windowData; } }
    
    private Vector3 windowPos;

    private Canvas windowCanvas;

    protected virtual void Init()
    {
        windowCanvas = GetComponent<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        windowData.isMaximum = false;
      
        windowBar.Init(windowData, rectTransform);
        OnSelected += () => WindowSelected(true);
        OnUnSelected += () => WindowSelected(false);
        windowBar.OnClose?.AddListener(WindowClose);
        windowBar.OnMinimum?.AddListener(WindowMinimum);
        windowBar.OnMaximum?.AddListener(WindowMaximum);
        windowBar.OnSelected += SelectWindow;

    }

    public void WindowSelected(bool windowSelected)
    {
        isSelected = windowSelected;

        if(windowCanvas == null)
        {
            return;
        }

        if(isSelected)
        {
            windowCanvas.sortingOrder = windowMaxCnt;
        }
        if (!isSelected)
        {
            windowCanvas.sortingOrder -= 1;

            if (windowCanvas.sortingOrder <= 0)
            {
                windowCanvas.sortingOrder = 1;
            }
        }
    }

    public void WindowClose()
    {
        OnClosed?.Invoke(windowData.windowTitleID);

        windowMaxCnt--;
        Destroy(gameObject);
    }
    
    public void WindowMinimum()
    {
        WindowManager.Inst.SelectedObjectNull();
        SetActive(false);
    }

    public void WindowMaximum()
    {
        if(!windowData.isMaximum)
        {
            Vector2 size = Constant.MAX_CANVAS_SIZE;
            size.y -= 25;
            rectTransform.sizeDelta = size;

            windowPos = rectTransform.localPosition;
            rectTransform.localPosition = new Vector3(0, 25, 0);

            windowData.isMaximum = true;
        }
        else
        {
            rectTransform.localPosition = windowPos;
            rectTransform.sizeDelta = windowData.size;

            windowData.isMaximum = false;
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

    public void CreatedWindow()
    {
        Init();
        WindowOpen();
        windowMaxCnt++;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectWindow();
    }

    protected void SelectWindow()
    {
        WindowManager.Inst.SelectObject(this);
        SetCurrentWindow(this);
    }
}
