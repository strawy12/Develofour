using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class WindowBar : MonoBehaviour, IPointerClickHandler,IBeginDragHandler, IDragHandler
{
    [SerializeField] private Button maximumBtn;
    [SerializeField] private Button minimumBtn;
    [SerializeField] private Button closeBtn;
    [SerializeField] private Image iconImage;
    [SerializeField] private TMP_Text windowName;

    public UnityEvent OnClose;
    public UnityEvent OnMinimum;
    public UnityEvent OnMaximum;

    private bool isClicked;
    private float clickDelayTime = 0.0f;
    private Vector2 offsetPos = Vector2.zero;

    private WindowDataSO windowData;
    private RectTransform windowRectTransform;

    public void Init(WindowDataSO winData, RectTransform rectTrm) 
    {
        windowName.text = $"{windowData.windowType} - {windowName.text}";
        windowData = winData;
        windowRectTransform = rectTrm;
    }

    public void Update()
    {
        if(isClicked)
        {
            clickDelayTime += Time.deltaTime;
            if(clickDelayTime > 1.0f)
            {
                isClicked = false;
                clickDelayTime = 0.0f;
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        offsetPos = eventData.position - (Vector2)transform.position;
        offsetPos.y += windowData.size.y / 2;
    }

    public void OnDrag(PointerEventData eventData)
    {
        windowRectTransform.position = eventData.position - offsetPos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isClicked == false)
        {
            isClicked = true;
        }
        else if(isClicked == true)
        {
            if(clickDelayTime <= 1.0f)
            {
                //todo:윈도우 최대화OnOff
                isClicked = false;
            }
        }
    }
}
