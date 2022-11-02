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

    public UnityEvent OnClose   { get { return closeBtn.onClick; } }
    public UnityEvent OnMinimum { get { return minimumBtn.onClick; } }
    public UnityEvent OnMaximum { get { return maximumBtn.onClick; } }

private bool isClicked;
    private float clickDelayTime = 0.0f;
    private Vector2 offsetPos = Vector2.zero;

    private WindowDataSO windowData;
    private RectTransform windowRectTransform;
    private RectTransform rectTrasnform;
    private void Awake()
    {
        rectTrasnform = GetComponent<RectTransform>();
    }
    public void Init(WindowDataSO winData, RectTransform rectTrm) 
    {
        windowData = winData;
        windowRectTransform = rectTrm;
        windowName.text = $"{windowData.windowType} - {windowName.text}";

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
        Vector2 mousePos = eventData.position - (Constant.MAXWINSIZE / 2);
        offsetPos = windowRectTransform.anchoredPosition - mousePos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 mousePos = eventData.position - (Constant.MAXWINSIZE / 2);
        windowRectTransform.anchoredPosition = mousePos + offsetPos;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Click");
        if (isClicked == false)
        {
            isClicked = true;
        }
        else if(isClicked == true)
        {
            if(clickDelayTime <= 1.0f)
            {
                OnMaximum?.Invoke();
                isClicked = false;
            }
        }
    }
}
