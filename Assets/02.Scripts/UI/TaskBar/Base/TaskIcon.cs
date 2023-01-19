﻿using System.Collections;
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
    protected TargetWindowPanels targetWindowPanels;

    [SerializeField]
    protected Image iconImage;
    [SerializeField]
    protected Image activeImage;
    [SerializeField]
    protected Image highlightedImage;

    protected List<TargetWindowPanel> targetPanelList = new List<TargetWindowPanel>();

    protected bool isFixed = false;
    protected bool isSelectedTarget = false;
    private bool isEnter = false;
    private bool isClick = false;

    public Action<TaskIcon> OnClose;

    private const float SHOW_TARGET_PANELS_DELAY_TIME = 1.5f;

    private static TargetWindowPanels showTargetPanels = null;

    private Coroutine showTargetPanelDelayCoroutine = null;


    #region Task Icon

    // TaskIcon은 Window에 대한 정보는 Type만 가지고 있고 
    // TargetWindowPanel이 Window에 대한 정보를 가지고있다.
    // TargetWindowPanels는 단순히 UI이며 TargetWidnowPanel은 TaskIcon가 관리한다.
    public void Init(FileSO windowFile)
    {
        this.windowType = (int)windowFile.windowType;

        attributePanel.Init();
        targetWindowPanels.Init();

        SetIcon(windowFile.iconSprite);

        targetWindowPanels.OnUnSelectIgnoreFlag += () => isEnter && !isClick;
        attributePanel.OnCloseWindow += CloseIcon;
        attributePanel.OnOpenWindow += ShowFirstWindow;
    }

    public void SetIcon(Sprite sprite)
    {
        iconImage.sprite = sprite;
    }

    public void CloseIcon()
    {
        Release();
        //TODO : attributePanel 종료
    }

    public void Release()
    {
        attributePanel.OnCloseWindow -= CloseIcon;
        attributePanel.OnOpenWindow -= ShowFirstWindow;
        windowType = (int)EWindowType.None;

        while (targetPanelList.Count != 0)
        {
            targetPanelList[0].Close();
            //window의 OnClose에서 remove를 시켜줄꺼임
        }
    }

    protected virtual void ClickIcon()
    {
        if (targetPanelList.Count <= 0) return;

        isClick = true;

        if (isSelectedTarget)
        {
            HideWindow();
        }

        else
        {
            ShowWindow();
        }

        if (targetPanelList.Count > 1)
        {
            StopShowTargetPanelsDelayCoroutine();

            if (targetWindowPanels.IsShow)
            {
                HideTargetPanels();
            }

            else
            {
                ShowTargetPanels();
            }

        }

        isClick = false;

        attributePanel.Hide();
    }
    #endregion

    #region Window Show & Hide
    // 여기선 그냥 누르면 보여주기만 할 꺼임
    protected void ShowWindow(Window window = null)
    {
        if (window == null && targetPanelList.Count == 1)
        {
            window = targetPanelList[0].TargetWindow;
        }

        if (window == null) return;

        if (!window.IsSelected())
        {
            window.WindowOpen();
        }
    }

    private void HideWindow()
    {
        if (targetPanelList.Count == 1)
        {
            if (targetPanelList[0].TargetWindow.IsSelected())
            {
                // WARNING
                // 이대로 써도 될지 생각해보기
                targetPanelList[0].TargetWindow.WindowMinimum();
            }
        }
    }

    //fixed라면 override해서 if(cnt != 0) base() else { 윈도우 생성 }
    public virtual void ShowFirstWindow()
    {
        if (targetPanelList.Count != 0)
        {
            targetPanelList[0].ShowWindow();
            attributePanel.Hide();
        }
    }

    #endregion

    #region Window Manage
    public void AddWindow(Window window)
    {
        TargetWindowPanel panel = AddTargetPanel(window);

        if (panel == null)
        {
            Debug.LogError("TargetWindowPanel이 null 입니다.");
            return;
        }

        window.OnSelected += () => SelectedWindow(true);
        window.OnUnSelected += () => SelectedWindow(false);

        window.OnUnSelectIgnoreFlag += () => isEnter && !isClick;

        activeImage.gameObject.SetActive(true);
    }



    public void SelectedWindow(bool isSelected)
    {
        isSelectedTarget = isSelected;
        activeImage.gameObject.SetActive(isSelected);
    }

    public void RemoveTargetPanel(TargetWindowPanel panel)
    {
        targetPanelList.Remove(panel);

        if (targetPanelList.Count <= 0)
        {
            activeImage.gameObject.SetActive(false);
            if (!isFixed)
            {
                OnClose?.Invoke(this);
                CloseIcon();
            }
        }
    }

    #endregion

    #region TargetPanel
    public TargetWindowPanel AddTargetPanel(Window window)
    {
        TargetWindowPanel panel = targetWindowPanels.AddTargetPanel(window);
        if (panel == null) return null;

        panel.OnClose += RemoveTargetPanel;
        panel.OnClick += HideTargetPanels;

        targetPanelList.Add(panel);

        return panel;
    }

    #endregion

    #region TargetPaenels

    private IEnumerator ShowTargetPanelsDelay()
    {
        if (!targetWindowPanels.IsShow)
        {
            yield return new WaitForSeconds(SHOW_TARGET_PANELS_DELAY_TIME);
            ShowTargetPanels();
        }

        showTargetPanelDelayCoroutine = null;
    }

    private void StopShowTargetPanelsDelayCoroutine()
    {
        if (showTargetPanelDelayCoroutine != null)
        {
            StopCoroutine(showTargetPanelDelayCoroutine);
        }

        showTargetPanelDelayCoroutine = null;
    }

    private void ShowTargetPanels()
    {
        StopShowTargetPanelsDelayCoroutine();

        showTargetPanels = targetWindowPanels;
        targetWindowPanels.Show();
    }

    private void HideTargetPanels()
    {
        StopShowTargetPanelsDelayCoroutine();

        targetWindowPanels.Hide();
    }

    #endregion

    #region EventSystem Func
    public void OnPointerClick(PointerEventData eventData)
    {
        switch (eventData.button)
        {
            case PointerEventData.InputButton.Left:
                ClickIcon();
                break;
            case PointerEventData.InputButton.Right:
                HideTargetPanels();
                attributePanel.Show();
                break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isEnter = true;
        highlightedImage.gameObject.SetActive(true);

        if (targetPanelList.Count >= 1)
        {
            if (targetWindowPanels.IsShow)
            {
                targetWindowPanels.OnPointerEnter(eventData);
            }
            else
            {
                if (showTargetPanelDelayCoroutine != null)
                {
                    StopCoroutine(showTargetPanelDelayCoroutine);
                }

                showTargetPanelDelayCoroutine = StartCoroutine(ShowTargetPanelsDelay());
            }
            // Attribute 오픈
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isEnter = false;
        highlightedImage.gameObject.SetActive(false);

        StopShowTargetPanelsDelayCoroutine();

        if (targetWindowPanels.IsShow)
        {
            targetWindowPanels.OnPointerExit(eventData);
        }
    }

    #endregion
}
