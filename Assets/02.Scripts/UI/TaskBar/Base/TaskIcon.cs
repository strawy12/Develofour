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
    protected TargetWindowPanels targetWindowPanelsUI;
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

    private int windowID;

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
        targetWindowPanelsUI.Init();
        windowID = windowPrefab.windowTitleID;
    }

    protected void Bind() 
    {
      
    }


    public void RemoveTargetWindow(int titleID)
    {
        for(int i =0; i < targetWindowList.Count; i++)
        {
            if (targetWindowList[i].windowTitleID == titleID)
            {
                targetWindowList.RemoveAt(i);
            }
        }
        
        if (targetWindowList.Count < 1)
        {
            activeImage.gameObject.SetActive(false);
            if (!isFixed)
            {
                OnDetroy.Invoke(windowType);
                Destroy(gameObject);
            }
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
                    CreateWindow(windowID);
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
        SelectedTargetWindow(false);
    }

    public void AttributeOpen()
    {
        if(targetWindowList.Count <= 0)
        {
            CreateWindow(windowID);
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
            targetWindowPanelsUI.OpenTargetWindowPanelUI();
        }
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
            target.Init(window);
            target.OnOpen += CreateWindow;
            window.OnClose += TargetWindowPanelClose;
            target.SelectedTargetWindow(true);
            window.OnSelected += () => target.SelectedTargetWindow(true);
            window.OnUnSelected += () => target.SelectedTargetWindow(false);
            targetWindowPanelDictionary.Add(target.WindowTitleId, target);
            SetTargetWindowPanelUISize();
        }
    }
    private void SetTargetWindowPanelUISize()
    {
        int panelCnt = targetWindowPanelDictionary.Count;
        int height = 40 * panelCnt + 10;
        targetWindowPanelsUI.TargetTransform.sizeDelta = new Vector2(180, height);
    }
    public void SelectedTargetWindow(bool isSelected)
    {
        isSelectedTarget = isSelected;
        highlightedImage.gameObject.SetActive(isSelected);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isSelectedTarget)
        {
            highlightedImage.gameObject.SetActive(true);
        }
        if (targetWindowList.Count >= 1)
        {
            targetWindowPanelsUI.gameObject.SetActive(true);
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSelectedTarget)
        {
            highlightedImage.gameObject.SetActive(false);
        }
        if (targetWindowPanelsUI.IsEnter == false)
        {
            targetWindowPanelsUI.gameObject.SetActive(false);
        }
    }

    public void TargetWindowPanelClose(int titleID)
    {
        TargetWindowPanel target = targetWindowPanelDictionary[titleID];
        targetWindowPanelDictionary.Remove(titleID);
        target.Close();
    }
}
