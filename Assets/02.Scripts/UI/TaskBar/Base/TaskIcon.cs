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

    [SerializeField]
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

    //�ӽÿ�
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
        //if (iconImage.sprite != target.WindowData.IconSprite)
        //{
        //    iconImage.sprite = target.WindowData.IconSprite;
        //}

        //windowType = target.WindowType;

        //target.OnClose += RemoveTargetWindow;
        //target.OnSelected += () => SelectedTargetWindow(true);
        //target.OnUnSelected += () => SelectedTargetWindow(false);

        //targetWindowList.Add(target);
        //activeImage.gameObject.SetActive(true);

    }

    public void RemoveTargetWindow(int windowTitle)
    {
        foreach (Window window in targetWindowList)
        {
            if (window.windowTitleID == windowTitle)
            {
                targetWindowList.Remove(window);
                break;
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

                if (targetWindowList.Count == 0)
                {
                    CreateWindow(defaultWindow.windowTitleID);
                }
                OpenTargetWindow();
                //TODO �̰� �ʿ��ѵ� �ٲ㺸��
                break;


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
        while(targetWindowList.Count != 0)
        {
            targetWindowList[0].Close();
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
            //TODO : â�� 2�� �̻� �������� �������� �̸� �����ִ°� ����
        }
    }

    protected void CreateWindow(int titleID)
    {
        
        foreach (Window window in targetWindowList)
        {
            if (window.windowTitleID == titleID)
            {
                window.gameObject.SetActive(true);
                return;
                //TODO : OrderInLayer �� ������ �ű��
            }
        }

        if (isFixed && defaultWindow != null)
        {
            Window window = Instantiate(defaultWindow, transform.parent.parent.parent);
            if (iconImage.sprite != window.WindowData.IconSprite)
            {
                iconImage.sprite = window.WindowData.IconSprite;
            }
            windowType = window.WindowType;

            window.OnClose += RemoveTargetWindow;
            window.OnSelected += () => SelectedTargetWindow(true);
            window.OnUnSelected += () => SelectedTargetWindow(false);

            window.CreateWindow();

            targetWindowList.Add(window);
            activeImage.gameObject.SetActive(true);

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
