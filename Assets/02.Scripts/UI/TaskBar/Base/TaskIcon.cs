using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class TaskIcon : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private int windowType;
    public int WindowType => windowType;

    [SerializeField]
    private TaskIconAttribute attributePanel;

    [SerializeField]
    private Window windowPrefab;
    [SerializeField]
    private TargetWindowPanel targetWindowPanelPrefab;
    [SerializeField]
    private TargetWindowPanels targetWindowPanelsUI;
    [SerializeField]
    private List<Window> targetWindowList = new List<Window>();

    private Dictionary<int,TargetWindowPanel> targetWindowPanelDictionary = new Dictionary<int,TargetWindowPanel>();

    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Image activeImage;
    [SerializeField]
    private Image highlightedImage;

    [SerializeField]
    private bool isFixed = false;
    private bool isSelectedTarget = false;

    public bool IsFixed { get { return isFixed; } }

    public Action<int> OnDetroy;

    private int windowPrefabID;

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
        windowType = (int)windowPrefab.WindowData.windowType;
        attributePanel.OnClose -= AttributeClose;
        attributePanel.OnOpen -= AttributeOpen;
        attributePanel.OnClose += AttributeClose;
        attributePanel.OnOpen += AttributeOpen;
        targetWindowPanelsUI.Init();
        //windowPrefabID = windowPrefab.WindowData.windowTitleID;
    }


    protected void Bind() 
    {
      
    }


    public void RemoveTargetWindow(int titleID)
    {
        for(int i =0; i < targetWindowList.Count; i++)
        {
            if (targetWindowList[i].WindowData.windowTitleID == titleID)
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
                    CreateWindow(windowPrefabID);
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
            targetWindowList[0].WindowClose();
        }
        SelectedTargetWindow(false);
    }

    public void AttributeOpen()
    {
        if(targetWindowList.Count <= 0)
        {
            CreateWindow(windowPrefabID);
        }
        else if(targetWindowList.Count == 1)
        {
            targetWindowList[0].WindowOpen();
        }
        else if (targetWindowList.Count > 1)
        {
            //TODO : 창이 2개 이상 켜있으면 위쪽으로 미리 보여주는거 제작
        }
    }

    private void OpenTargetWindow()
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
        else if (targetWindowList.Count > 1)
        {
            //TODO : 창이 2개 이상 켜있으면 위쪽으로 미리 보여주는거 제작
            targetWindowPanelsUI.OpenTargetWindowPanelUI();
        }
    }

    private void CreateWindow(int titleID)
    {
        foreach (Window window in targetWindowList)
        {
            if (window.WindowData.windowTitleID == titleID)
            {
                //TODO : OrderInLayer 맨 앞으로 옮기기
                targetWindowPanelDictionary[titleID].SelectedTargetWindow(true);
                window.WindowOpen();
                return;
            }
        }

        if (isFixed && windowPrefab != null)
        {
            Window window = Instantiate(windowPrefab, Define.WindowCanvasTrm);
            if (iconImage.sprite != window.WindowData.iconSprite)
            {
                iconImage.sprite = window.WindowData.iconSprite;
            }
            windowType = (int)window.WindowData.windowType;

            AddTargetWindow(window);
        }
    }

    public void AddTargetWindow(Window window)
    {
        window.OnClose += RemoveTargetWindow;
        window.OnSelected += () => SelectedTargetWindow(true);
        window.OnUnSelected += () => SelectedTargetWindow(false);

        window.CreatedWindow();

        targetWindowList.Add(window);

        activeImage.gameObject.SetActive(true);

        TargetWindowPanel target = Instantiate(targetWindowPanelPrefab, targetWindowPanelsUI.transform);
        target.Init(window);
        target.OnOpen += CreateWindow;
        target.OnClose += window.WindowClose;

        target.SelectedTargetWindow(true);
        window.OnSelected += () => target.SelectedTargetWindow(true);
        window.OnUnSelected += () => target.SelectedTargetWindow(false);
        window.OnClose += TargetWindowPanelClose;

        targetWindowPanelDictionary.Add(target.WindowTitleId, target);
        SetTargetWindowPanelUISize();
    }

    private void SetTargetWindowPanelUISize()
    {
        int panelCnt = targetWindowPanelDictionary.Count;
        int height = 50 * panelCnt + 10;
        targetWindowPanelsUI.TargetTransform.sizeDelta = new Vector2(180, height);
        targetWindowPanelsUI.TargetTransform.anchoredPosition = new Vector2(0, height + 20);
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
