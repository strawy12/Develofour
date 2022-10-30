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
    protected Window windowPrefab; 

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

    public bool IsFixed { get { return isFixed; } }

    public Action<int> OnDetroy;

    private int windowId;

    //�ӽÿ�
    void Awake()
    {
        if(IsFixed == true)
        {
            Init();
            Bind();
        }
    }

    /// <summary>
    /// ������ų�� ������������
    /// </summary>
    public void Init()
    {
        attributePanel.Init();
        windowId = windowPrefab.windowTitleID;
    }

    protected void Bind() 
    {
      
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
                    CreateWindow(windowId);
                }
                OpenTargetWindow();
                //TODO �̰� �ʿ��ѵ� �ٲ㺸��
                break;


            case PointerEventData.InputButton.Right:
                OpenAttributePanel();
                break;
        }
    }

    public void OpenAttributePanel()
    {
        attributePanel.Open();
    }

    public void CloseAttributePanel()
    {
        attributePanel.Close();
    }

    public void AttributeClose()
    {
        while (targetWindowList.Count != 0)
        {
            targetWindowList[0].Close();
        }
    }

    public void AttributeOpen()
    {
        CreateWindow(windowId);
    }

    protected virtual void OpenTargetWindow()
    {
        if (targetWindowList.Count == 1)
        {
            if(targetWindowList[0].isOpen)
            {
                targetWindowList[0].MinimumWindow();
            }
            else
            {
                targetWindowList[0].Open();
            }
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

        if (isFixed && windowPrefab != null)
        {
            Window window = Instantiate(windowPrefab, transform.parent.parent.parent);
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
