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

    protected void OpenWindow()
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
            //´ÙÁßÃ¢ ¿ÀÇÂ 
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
