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
    protected TargetWindowPanel targetWindowPanelPrefab;
    [SerializeField]
    protected Image targetWindowPanelsUI;
    [SerializeField]
    protected List<Window> targetWindowList = new List<Window>();

    protected Dictionary<int,TargetWindowPanel> targetWindowPanelDictionary = new Dictionary<int,TargetWindowPanel>();

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

    //임시용
    void Awake()
    {
        if(IsFixed == true)
        {
            Init();
            Bind();
        }
    }

    /// <summary>
    /// 생성시킬때 실행시켜줘야함
    /// </summary>
    public void Init()
    {
        attributePanel.Init();
        attributePanel.OnClose -= AttributeClose;
        attributePanel.OnOpen -= AttributeOpen;
        attributePanel.OnClose += AttributeClose;
        attributePanel.OnOpen += AttributeOpen;
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

        if(targetWindowList.Count == 0)
        {
            activeImage.gameObject.SetActive(false);
        }

        if (!isFixed && targetWindowList.Count <= 1)
        {
            OnDetroy.Invoke(windowType);
            Destroy(this);
        }
        SetTargetWindowPanelUISize();

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
                else
                {

                }
                OpenTargetWindow();
                
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
        if(targetWindowList.Count <= 0)
        {
            CreateWindow(windowId);
        }
        else if(targetWindowList.Count == 1)
        {
            targetWindowList[0].Open();
        }
        else if (targetWindowList.Count > 1)
        {
            //TODO : 창이 2개 이상 켜있으면 위쪽으로 미리 보여주는거 제작
        }
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
            attributePanel.Close();
        }
        else if (targetWindowList.Count > 1)
        {
            //TODO : 창이 2개 이상 켜있으면 위쪽으로 미리 보여주는거 제작
            OpenTargetWindowPanelUI();
        }
    }
    protected void OpenTargetWindowPanelUI()
    {
        if (targetWindowPanelsUI.gameObject.activeSelf) return;
        targetWindowPanelsUI.gameObject.SetActive(true);
    }
    protected void CreateWindow(int titleID)
    {
        foreach (Window window in targetWindowList)
        {
            if (window.windowTitleID == titleID)
            {
                //TODO : OrderInLayer 맨 앞으로 옮기기
                targetWindowPanelDictionary[titleID].SelectedTargetWindow(true);
                window.gameObject.SetActive(true);
                return;
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

            TargetWindowPanel target = Instantiate(targetWindowPanelPrefab, targetWindowPanelsUI.transform);
            target.CreateWinPanel(window);
            target.OnClose += window.Close;
            target.OnOpen += CreateWindow;
            target.SelectedTargetWindow(true);
            targetWindowPanelDictionary.Add(target.WindowTitleId, target);

            SetTargetWindowPanelUISize();
        }
    }
    private void SetTargetWindowPanelUISize()
    {
        int panelCnt = targetWindowPanelDictionary.Count;
        int height = 40 * panelCnt + 10;
        targetWindowPanelsUI.rectTransform.sizeDelta = new Vector2(180, height);
    }
    public void SelectedTargetWindow(bool isSelected)
    {
        isSelectedTarget = isSelected;
        highlightedImage.gameObject.SetActive(isSelected);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelectedTarget && targetWindowList.Count <= 1) return;
        highlightedImage.gameObject.SetActive(true);
        targetWindowPanelsUI.gameObject.SetActive(true);
        

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelectedTarget) return;
        highlightedImage.gameObject.SetActive(false);
    }

}
