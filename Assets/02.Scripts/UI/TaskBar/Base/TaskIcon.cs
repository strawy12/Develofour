using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TaskIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected int windowType;
    public int WindowType => windowType;
    [SerializeField]
    protected TaskIconAttribute attributePanel;
    [SerializeField]
    protected TargetPanel targetPanelTemp;

    [SerializeField]
    protected Image iconImage;
    [SerializeField]
    protected Image activeImage;
    [SerializeField]
    protected Image highlightedImage;

    protected List<Window> windowList;
    protected List<TargetPanel> targetPanelList;
    public List<TargetPanel> TargetPanelList => targetPanelList;

    protected bool isFixed = false;
    protected bool isSelectedTarget = false;

    public Action<TaskIcon> OnClose;

    public RectTransform rectTransform { get; private set; }

    public void Init(Window window)
    {
        attributePanel.Init();
        targetPanelTemp.Init(window);
        windowType = (int)window.WindowData.windowType;
        
        attributePanel.OnCloseTaskIcon += CloseIcon;
        attributePanel.OnOpenWindow += AttributeOpen;
    }

    public void CloseIcon()
    {
        Release();
        Destroy(this.gameObject);
        //TODO : attributePanel 종료
    }

    public void Release()
    {
        attributePanel.OnCloseTaskIcon -= CloseIcon;
        attributePanel.OnOpenWindow -= AttributeOpen;
        windowType = (int)EWindowType.None;
        while(windowList.Count != 0)
        {
            windowList[0].WindowClose();
            //window의 OnClose에서 remove를 시켜줄꺼임
        }
    }

    //fixed라면 override해서 if(cnt != 0) base() else { 윈도우 생성 }
    public virtual void AttributeOpen()
    {
        if(windowList.Count != 0)
        {
            ShowWindow(windowList[0]);
            attributePanel.AttributeClose();
        }
    }

    public void RemoveWindow(int titleID)
    {
        for(int i = 0; i < windowList.Count; i++)
        {
            if(windowList[i].WindowData.windowTitleID == titleID)
            {
                windowList[i].WindowClose();
                //window의 OnClose에서 remove를 시켜줄꺼임
            }
        }

        if (windowList.Count < 1)
        {
            activeImage.gameObject.SetActive(false);
            if (!isFixed)
            {
                OnClose.Invoke(this);
                CloseIcon();
            }
        }
    }

    protected void ShowWindow(Window window) // 여기선 그냥 누르면 보여주기만 할 꺼임
    {

    }

    protected void ShowWindow() // 여기선 그냥 누르면 보여주기만 할 꺼임
    {
        if (windowList.Count == 1)
        {
            if (windowList[0].IsSelected)
            {
                windowList[0].WindowMinimum();
            }
            else
            {
                windowList[0].WindowOpen();
            }
            attributePanel.AttributeClose();
        }
        else if (windowList.Count >= 2)
        {
            // 복수 보여주기
        }
    }


    public void AddWindow(Window window)
    {
        window.OnClosed += RemoveWindow;
        window.OnSelected += () => SelectedWindow(true);
        window.OnUnSelected += () => SelectedWindow(false);

        window.CreatedWindow();

        windowList.Add(window);

        activeImage.gameObject.SetActive(true);
    }

    public void SelectedWindow(bool isSelected)
    {
        isSelectedTarget = isSelected;
        highlightedImage.gameObject.SetActive(isSelected);
    }

    //public void TargetWindowPanelClose(int titleID)
    //{

    //}


    protected virtual void LeftClick()
    {
        if (windowList.Count <= 0) return;
        ShowWindow();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                LeftClick();
                break;
            case PointerEventData.InputButton.Right:
                //AttributeOpen();
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelectedTarget)
        {
            highlightedImage.gameObject.SetActive(true);
        }
        if (windowList.Count >= 1)
        {
            TargetPanels.OnOpened?.Invoke(this);
            // Attribute 오픈
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelectedTarget)
        {
            highlightedImage.gameObject.SetActive(false);
        }
        if (windowList.Count >= 1)
        {
            TargetPanels.OnClosed?.Invoke();
            // Attribute 오픈
        }
    }
}
