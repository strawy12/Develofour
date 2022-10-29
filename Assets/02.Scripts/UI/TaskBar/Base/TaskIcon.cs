using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TaskIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    protected string windowTitle;
    public string Title => windowTitle;

    protected List<Window> targetWindowList;
    [SerializeField]
    protected Image iconImage;
    [SerializeField]
    protected Image activeImage;
    [SerializeField]
    protected Image highlightedImage;

    protected bool isFixed = false;
    protected bool isSelectedTarget = false;

    public void Init()
    {
        targetWindowList = new List<Window>();
    }

    protected void Bind() // 여기 find에서 바꿔야함  
    {
      
    }

    public void AddTargetWindow(Window target)
    {
        if (iconImage.sprite != target.WindowData.IconSprite)
        {
            iconImage.sprite = target.WindowData.IconSprite;
        }
        if (string.IsNullOrEmpty(windowTitle))
        {
            windowTitle = target.WindowData.Title;
        }

        target.OnClose += RemoveTargetWindow;
        target.OnSelected += () => SelectedTargetWindow(true);
        target.OnUnSelected += () => SelectedTargetWindow(false);

        targetWindowList.Add(target);
        activeImage.gameObject.SetActive(true);

    }
    public void RemoveTargetWindow(string wndID)
    { 
        if(!isFixed && targetWindowList.Count <= 1)
        {
            TaskBar.RemoveIconEvent.Invoke(wndID);
            Destroy(this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                OpenTargetWindow();
                break;

            //case PointerEventData.InputButton.Middle:

            case PointerEventData.InputButton.Right:
                break;
        }
    }

    protected virtual void OpenTargetWindow()
    {
        if (targetWindowList.Count == 1)
        {
            targetWindowList[0].Open();
        }
    }

    public void SelectedTargetWindow(bool isSelected)
    {
        isSelectedTarget = isSelected;
        highlightedImage.gameObject.SetActive(isSelected);
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelectedTarget) return;
        highlightedImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelectedTarget) return;
        highlightedImage.gameObject.SetActive(false);
    }

}
