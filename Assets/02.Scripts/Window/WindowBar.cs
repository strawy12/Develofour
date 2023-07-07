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

    public Button MaximumBtn => maximumBtn;
    public Button MinimumBtn => minimumBtn;
    public Button CloseBtn => closeBtn;
    public UnityEvent OnClose   
    { 
        get 
        { 
            if(closeBtn != null)
            {
                return closeBtn.onClick; 
            }
            else
            {
                return null;
            }
        } 
    }

    public UnityEvent OnMinimum 
    { 
        get 
        { 
            if(minimumBtn != null)
            {
                return minimumBtn.onClick; 
            }
            else
            {
                return null;
            }
        } 
    }
    
    public UnityEvent OnMaximum 
    { 
        get 
        { 
            if(maximumBtn != null)
            {
                return maximumBtn.onClick; 
            }
            else
            {
                return null;
            }
        } 
    }

    public Action OnSelected;

    private bool isDrag = false;
    private bool isClicked;
    private float clickDelay = 0.75f;
    private float clickDelayTime = 0.0f;
    private Vector2 offsetPos = Vector2.zero;
    
    private WindowAlterationSO windowAlteration;
    private FileSO currentFile;

    private RectTransform windowRectTransform;

    public void Init(WindowAlterationSO windowAlteration, FileSO file, RectTransform rectTrm) 
    {
        this.windowAlteration = windowAlteration;

        if (file != null)
        {
            this.currentFile = file;
        }

        windowRectTransform = rectTrm;

        if (windowName != null && file.name != "WindowPinLockSO")
        {
            windowName.text = $"{currentFile.fileName} - {windowName.text}";
        }

        if (iconImage != null)
        {
            // FileSO
            iconImage.sprite = currentFile.iconSprite;
            iconImage.color = currentFile.iconColor;
        }
    }

    public void Init(WindowAlterationSO windowAlteration, RectTransform rectTrm)
    {
      this.windowAlteration = windowAlteration;

        windowRectTransform = rectTrm;
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
        // WindowTransform
        if (windowAlteration.isMaximum) { return; }

        isDrag = true;
        Vector2 mousePos = Define.CanvasMousePos;
        offsetPos = windowRectTransform.anchoredPosition - mousePos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isDrag == false) { return; }
        if (eventData.position.x > Define.MaxWindowSize.x || 
           eventData.position.x < 0 ||
           eventData.position.y > Define.MaxWindowSize.y||
           eventData.position.y < 100)
        {
            return;
        }

        Vector2 mousePos = Define.CanvasMousePos;
        windowRectTransform.anchoredPosition = mousePos + offsetPos;
        windowAlteration.pos = windowRectTransform.anchoredPosition;
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
