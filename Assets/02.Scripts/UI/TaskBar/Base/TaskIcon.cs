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


    public void Init(int windowType)
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

    protected virtual void LeftClick()
    {

    }

    protected void OpenTargetWindow()
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
