using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Window : MonoUI, IPointerClickHandler, ISelectable
{
    [SerializeField]
    private WindowBar windowBar;
    [SerializeField]
    private WindowDataSO windowData;

    private bool isSelected;
    private bool isMaximum;
    public bool IsSelected { get { return isSelected; } }
    private RectTransform rectTransform;

    public Action<int> OnClose;

    public Action OnSelected { get; set; }
    public Action OnUnSelected { get; set; }
    public WindowDataSO WindowData { get { return windowData; } }
    private void Init()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();

        isMaximum = false;

        windowBar.OnClose?.AddListener(WindowClose);
        windowBar.OnMinimum?.AddListener(WindowMinimum);
        windowBar.OnMaximum?.AddListener(WindowMaximum);

        windowBar.Init(windowData, rectTransform);
    }

    public void WindowClose()
    {
        OnClose?.Invoke(windowData.windowTitleID);
        Destroy(gameObject);
    }
    
    public void WindowMinimum()
    {
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
        SetActive(true);
    }

    // 생성 당해버린 상태에서 실행되는 함수임
    public void CreatedWindow()
    {
        Init();
        WindowOpen();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        WindowManager.Inst.SelectObject(this);
    }
}
