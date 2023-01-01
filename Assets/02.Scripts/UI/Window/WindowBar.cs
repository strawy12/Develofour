using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using Unity.VisualScripting;

public class WindowBar : MonoBehaviour, IPointerClickHandler,IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Button maximumBtn;
    [SerializeField] private Button minimumBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text windowName;


    public UnityEvent OnClose   { get { return closeBtn.onClick; } }
    public UnityEvent OnMinimum { get { return minimumBtn.onClick; } }
    public UnityEvent OnMaximum { get { return maximumBtn.onClick; } }

    public Action OnSelected;

    private bool isDrag = false;
    private bool isClicked;
    private float clickDelay = 0.75f;
    private float clickDelayTime = 0.0f;
    private Vector2 offsetPos = Vector2.zero;

    private WindowDataSO windowData;
    private RectTransform windowRectTransform;

    public void Init(WindowDataSO winData, RectTransform rectTrm) 
    {
        windowData = winData;
        
        windowRectTransform = rectTrm;
        if (windowName != null)
        {
            windowName.text = $"{windowData.windowType} - {windowName.text}";
        }

        if (iconImage != null)
        {
            iconImage.sprite = winData.iconSprite;
        }
    }

    public void SetNameText(string msg)
    {
        windowName.text = msg; 
    }

    public void Update()
    {
        if(isClicked)
        {
            clickDelayTime += Time.deltaTime;
            if(clickDelayTime > clickDelay)
            {
                isClicked = false;
                clickDelayTime = 0.0f;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (windowData.isMaximum) { return; }

        isDrag = true;
        Vector2 mousePos = Define.CanvasMousePos;
        offsetPos = windowRectTransform.anchoredPosition - mousePos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log(eventData.position.y);
        if (isDrag == false) { return; }
        if (eventData.position.x > Define.MaxWindowSize.x || 
           eventData.position.x < 0 ||
           eventData.position.y > Define.MaxWindowSize.y||
           eventData.position.y < 50)
        {
            return;
        }

        Vector2 mousePos = Define.CanvasMousePos;
        windowRectTransform.anchoredPosition = mousePos + offsetPos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isDrag) { return; }

        OnSelected?.Invoke();
        if (isClicked == false)
        {
            isClicked = true;
        }
        else if(isClicked == true)
        {
            if(clickDelayTime <= clickDelay)
            {
                OnMaximum?.Invoke();
                isClicked = false;
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isDrag == false) { return; }

        isDrag = false;
    }
}
