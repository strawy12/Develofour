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
    protected TargetWindowPanel targetWindowPanelTemp;

    [SerializeField]
    protected Image iconImage;
    [SerializeField]
    protected Image activeImage;
    [SerializeField]
    protected Image highlightedImage;

    protected List<Window> targetWindowList;

    protected bool isFixed = false;
    protected bool isSelectedTarget = false;

    public Action<TaskIcon> OnClose;

    //테스크 아이콘이 실행됬을때 -> 속성패널, 타겟윈도우템프패널 init 및 이벤트 넣어주기. + 타입 변경
    public void Init(Window window)
    {
        attributePanel.Init();
        targetWindowPanelTemp.Init(window);
        windowType = (int)window.WindowData.windowType;
        
        attributePanel.OnCloseTaskIcon += CloseIcon;
        attributePanel.OnOpenWindow += AttributeOpen;
    }

    public void CloseIcon()
    {
        Release();
        Destroy(this.gameObject);
        //TODO : attributePanel을 꺼야함
    }

    //테스크 아이콘이 꺼졌을때 -> 이벤트 빼주기 및 타입 기본으로 변경, 가지고있는 모든 타겟 윈도우 제거
    public void Release()
    {
        attributePanel.OnCloseTaskIcon -= CloseIcon;
        attributePanel.OnOpenWindow -= AttributeOpen;
        windowType = (int)EWindowType.None;
        while(targetWindowList.Count != 0)
        {
            targetWindowList[0].WindowClose();
            //window의 OnClose실행으로 자동으로 리스트에서 제거됨
        }
    }

    //fixed가 아니라면 0이 아닐수가 없음  fixed는 override해서 if(cnt != 0) base() else {윈도우 생성}
    public virtual void AttributeOpen()
    {
        if(targetWindowList.Count != 0)
        {
            OpenTargetWindow(targetWindowList[0]);
            attributePanel.AttributeClose();
        }
    }

    public void RemoveWindow(int titleID)
    {
        for(int i = 0; i < targetWindowList.Count; i++)
        {
            if(targetWindowList[i].WindowData.windowTitleID == titleID)
            {
                targetWindowList[i].WindowClose();
                //window의 OnClose실행으로 자동으로 리스트에서 제거됨
            }
        }

        if (targetWindowList.Count < 1)
        {
            activeImage.gameObject.SetActive(false);
            if (!isFixed)
            {
                OnClose.Invoke(this);
                CloseIcon();
            }
        }
    }


    protected virtual void LeftClick()
    {

    }
    protected void OpenTargetWindow()
    {

    }

    protected void OpenTargetWindow(Window window)
    {

    }

    public void AddTargetWindow(Window window)
    {

    }

    public void SelectedTargetWindow(bool isSelected)
    {

    }

    public void TargetWindowPanelClose(int titleID)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
