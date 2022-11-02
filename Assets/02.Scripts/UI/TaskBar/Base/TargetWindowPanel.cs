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
    private Button openBtn;
    [SerializeField]
    private Button closeBtn;

    public Action<int> OnOpen;
    public Action OnClose;
    private bool isSelected = false;
    private int windowTitleID;
    public int WindowTitleId { get { return windowTitleID; } }
    public void Init(Window window)
    {
        windowIcon.sprite = window.WindowData.iconSprite;
        windowTitle.text = $"{window.WindowData.name} - ";
        openBtn.onClick.AddListener(Open);
        closeBtn.onClick.AddListener(OnPressCloseButton);
        windowTitleID = window.WindowData.windowTitleID;
    }

    public bool CheckWindowTitle(int titleID)
    {
        if(windowTitleID == titleID)
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
    public void Open()
    {
        OnOpen?.Invoke(windowTitleID);
    }

    public void Close()
    {
        Destroy(this.gameObject);
    }

    public void OnPressCloseButton()
    {
        OnClose?.Invoke();
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
