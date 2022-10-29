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
    protected GameObject attributePanel;

    [SerializeField]
    protected GameObject windowPrefab; 

    protected List<Window> targetWindowList = new List<Window>();
    
    [SerializeField]
    protected Image iconImage;
    [SerializeField]
    protected Image activeImage;
    [SerializeField]
    protected Image highlightedImage;

    [SerializeField]
    protected bool isFixed = false;
    protected bool isSelectedTarget = false;

    private Window defaultWindow;

    public bool IsFixed { get { return isFixed; } }

    public Action<int> OnDetroy;

    //임시용
    void Awake()
    {
        defaultWindow = windowPrefab.GetComponent<Window>();
    }
    public void Init()
    {
  
    }

    protected void Bind() 
    {
      
    }

    public void AddTargetWindow(Window target)
    {
        if (iconImage.sprite != target.WindowData.IconSprite)
        {
            iconImage.sprite = target.WindowData.IconSprite;
        }

        windowType = target.WindowType;
      
        target.OnClose += RemoveTargetWindow;
        target.OnSelected += () => SelectedTargetWindow(true);
        target.OnUnSelected += () => SelectedTargetWindow(false);

        targetWindowList.Add(target);

        activeImage.gameObject.SetActive(true);
    
    }

    public void RemoveTargetWindow(int windowTitle)
    {
        foreach (Window window in targetWindowList)
        {
            if (window.windowTitleID == windowTitle)
            {
                targetWindowList.Remove(window);
            }
        }

        if (!isFixed && targetWindowList.Count <= 1)
        {
            OnDetroy.Invoke(windowType);
            Destroy(this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                defaultWindow.CreateWindow();
                CreateWindow(defaultWindow.windowTitleID);
                OpenTargetWindow();
                break;

            //case PointerEventData.InputButton.Middle:

            case PointerEventData.InputButton.Right:
                ShowAttributePanel();
                break;
        }
    }

    public void ShowAttributePanel()
    {
        attributePanel.SetActive(true);
    }

    public void AttributeClose()
    {
        foreach (Window window in targetWindowList)
        {
            window.OnClose?.Invoke(window.windowTitleID);
        }
    }

    public void AttributeOpen()
    {
        CreateWindow(defaultWindow.windowTitleID);
    }

    protected virtual void OpenTargetWindow()
    {
        if (targetWindowList.Count == 1)
        {
            targetWindowList[0].Open();
        }
        else if (targetWindowList.Count > 1)
        {
            //TODO : 창이 2개 이상 켜있으면 위쪽으로 미리 보여주는거 제작
        }
    }

    protected void CreateWindow(int titleID)
    {
        int sameDefaultWindowCount = 0;
        foreach (Window window in targetWindowList)
        {
            if (window.windowTitleID == titleID)
            {
                sameDefaultWindowCount++;
                if (sameDefaultWindowCount >= 2)
                {
                    window.gameObject.SetActive(true);
                    return;
                }
                //TODO : OrderInLayer 맨 앞으로 옮기기

            }
        }
        
        if (isFixed && defaultWindow != null)
        {
            Window window = Instantiate(defaultWindow, transform);
            targetWindowList.Add(window);
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
