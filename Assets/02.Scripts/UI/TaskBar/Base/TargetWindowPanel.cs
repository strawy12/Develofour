using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.EventSystems;

public class TargetWindowPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image windowIcon;
    public TMP_Text windowTitle;
    public Image highlightedImage;
    public Button openBtn;


    public Action<int> OnOpen;
    private bool isSelected = false;
    private int windowTitleID;
    public int WindowTitleId { get { return windowTitleID; } }
    public void Init(Window window)
    {
        windowIcon.sprite = window.WindowData.IconSprite;
        windowTitle.text = $"{window.WindowData.name} - {window.WindowData.Title}";
        openBtn.onClick.AddListener(Open);
        windowTitleID = window.windowTitleID;
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

    public void OnPointerEnter(PointerEventData eventData)
    {
        highlightedImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highlightedImage.gameObject.SetActive(false);
    }
}
