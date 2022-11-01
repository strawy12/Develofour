using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public class Window : MonoBehaviour, IPointerClickHandler, ISelectable
{
    [SerializeField]
    private WindowBar windowBar;
    [SerializeField]
    private WindowDataSO windowDataSO;

    private bool isSelected;

    private RectTransform rectTransform;

    public Action<int> OnClose;

    public Action OnSelected { get; set; }
    public Action OnUnSelected { get; set; }

    private void Init()
    {
        windowBar.Init(windowDataSO, rectTransform);

        windowBar.OnClose?.AddListener(WindowClose);
        windowBar.OnMinimum?.AddListener(WindowMinimum);
        windowBar.OnMaximum?.AddListener(WindowMaximum);
        rectTransform = GetComponent<RectTransform>();
    }

    public void WindowClose()
    {

    }
    public void WindowMinimum()
    {

    }

    public void WindowMaximum()
    {
        rectTransform.sizeDelta = Constant.MAXWINSIZE;
    }

    public void WindowOpen()
    {

    }

    public void CreateWindow()
    {
        Instantiate(this);
    }


    public void OnPointerClick(PointerEventData eventData)
    {
    }
}
