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

    }

    public void Release()
    {

    }

    public void RemoveTargetWindow(int titleID)
    {

    }

    public void CloseIcon()
    {

    }

    protected void ShowWindow() // 여기선 그냥 누르면 보여주기만 할 꺼임
    {
        if (targetWindowList.Count == 1)
        {
            if (targetWindowList[0].IsSelected)
            {
                targetWindowList[0].WindowMinimum();
            }
            else
            {
                targetWindowList[0].WindowOpen();
            }
            attributePanel.Close();
        }
        else if (targetWindowList.Count >= 2)
        {
            // 복수 보여주기
        }
    }

    public void AddWindow(Window window)
    {
        window.OnClosed += RemoveTargetWindow;
        window.OnSelected += () => SelectedWindow(true);
        window.OnUnSelected += () => SelectedWindow(false);

        window.CreatedWindow();

        targetWindowList.Add(window);

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
        if (targetWindowList.Count <= 0) return;
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
        if (targetWindowList.Count >= 1)
        {
            //다중창 오픈 
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
