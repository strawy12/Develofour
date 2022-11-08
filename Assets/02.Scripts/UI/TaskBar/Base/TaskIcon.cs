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
        //TODO : attributePanel�� ������
    }

    //�׽�ũ �������� �������� -> �̺�Ʈ ���ֱ� �� Ÿ�� �⺻���� ����, �������ִ� ��� Ÿ�� ������ ����
    public void Release()
    {
        attributePanel.OnCloseTaskIcon -= CloseIcon;
        attributePanel.OnOpenWindow -= AttributeOpen;
        windowType = (int)EWindowType.None;
        while(targetWindowList.Count != 0)
        {
            targetWindowList[0].WindowClose();
            //window�� OnClose�������� �ڵ����� ����Ʈ���� ���ŵ�
        }
    }

    //fixed�� �ƴ϶�� 0�� �ƴҼ��� ����  fixed�� override�ؼ� if(cnt != 0) base() else {������ ����}
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
                //window�� OnClose�������� �ڵ����� ����Ʈ���� ���ŵ�
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

    protected void OpenWindow()
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
        isSelectedTarget = isSelected;
        highlightedImage.gameObject.SetActive(isSelected);
    }

    public void TargetWindowPanelClose(int titleID)
    {

    }


    protected virtual void LeftClick()
    {
        if (targetWindowList.Count <= 0) return;
        OpenWindow();
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
        if (targetWindowList.Count >= 1)
        {
            //����â ���� 
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelectedTarget)
        {
            highlightedImage.gameObject.SetActive(false);
        }
    }
}
