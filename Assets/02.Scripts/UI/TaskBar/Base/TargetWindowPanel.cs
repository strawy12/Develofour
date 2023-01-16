using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class TargetWindowPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private Image windowIcon;
    [SerializeField]
    private TMP_Text windowTitle;
    [SerializeField]
    private Image highlightedImage;
    [SerializeField]
    private Button showBtn;
    [SerializeField]
    private Button closeBtn;

    private Window targetWindow;
    public Window TargetWindow => targetWindow;

    public Action<TargetWindowPanel> OnClick;
    public Action<TargetWindowPanel> OnClose;
    private bool isSelected = false;

    public void Init(Window window)
    {
        windowIcon.sprite = window.WindowData.iconSprite;
        windowTitle.text = $"{window.WindowData.name} - ";

        showBtn.onClick.AddListener(ClickPanel);
        closeBtn.onClick.AddListener(Close);
        targetWindow = window;

        targetWindow.OnClosed += (a) => Close();
    }

    public bool CheckWindowTitle(int titleID)
    {
        if(targetWindow.WindowData.windowTitleID == titleID)
        {
            return true;
        }
        return false;
    }

    public void SelectedTargetWindow(bool value)
    {
        isSelected = value;
        highlightedImage.gameObject.SetActive(value);
    }

    public void WindowOpen()
    {
        // 여기서는 윈도우를 생성한다.
    }

    public void ShowWindow()
    {
        targetWindow?.WindowOpen();
    }

    public void WindowHide()
    {
        targetWindow?.WindowOpen();

    }

    public void Close()
    {
        OnClose?.Invoke(this);
        Destroy(this.gameObject);
    }

    private void ClickPanel()
    {
        OnClick?.Invoke(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightedImage.gameObject.SetActive(true);
        closeBtn.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightedImage.gameObject.SetActive(false);
        closeBtn.gameObject.SetActive(false);
    }

}
