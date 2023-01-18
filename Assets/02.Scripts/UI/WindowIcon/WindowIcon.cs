using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class WindowIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectable
{
    public RectTransform rectTranstform { get; set; }

    private int clickCount = 0;
    private bool isSelected = false;

    private Window targetWindow = null;
    private Sprite sprite;

    [SerializeField]
    private WindowDataSO windowData;
    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Image selectedImage;
    [SerializeField]
    private Image pointerStayImage;

    [SerializeField]
    private TMP_Text iconNameText;

    [SerializeField]
    private WindowIconDataSO windowIconData;

    public Action OnSelected { get; set; }
    public Action OnUnSelected { get; set; }

    public void Awake()
    {
        Bind();
        Init();
    }

    public bool IsSelected(GameObject hitObject)
    {
        bool flag1 = hitObject == gameObject;
        return isSelected && flag1;
    }

    private void Bind()
    {
        rectTranstform = GetComponent<RectTransform>();
    }

    private void Init()
    {
        pointerStayImage.gameObject.SetActive(false);
        selectedImage.gameObject.SetActive(false);

        OnSelected += () => SelectedIcon(true);
        OnUnSelected += () => SelectedIcon(false);
    }
   
    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if (clickCount != 0)
            {
                // 여기에서 이벤트 쏨
                clickCount = 0;

                if (targetWindow == null)
                {
                    OpenWindow();
                    //TaskBar.OnAddIcon?.Invoke(targetWindow);
                }
                else
                {
                    targetWindow.WindowOpen();
                }
                WindowManager.Inst.SelectedObjectNull();
            }
            else 
            {
                WindowManager.Inst.SelectObject(this);
                clickCount++;
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            WindowManager.Inst.SelectObject(this);
            CreateAttributeUI(eventData);
        }
    }

    private void OpenWindow()
    {
        targetWindow = WindowManager.Inst.GetWindow(windowData.windowType, windowData.windowTitleID);
        if (targetWindow == null)
        {
            targetWindow = WindowManager.Inst.CreateWindow(windowData.windowType, windowData.windowTitleID);
        }
        targetWindow.OnClosed += CloseTargetWindow;
        targetWindow.WindowOpen();
    }

    private void SelectedIcon(bool isSelected)
    {
        if(!isSelected)
        {
            clickCount = 0;
        }

        this.isSelected = isSelected;
        selectedImage.gameObject.SetActive(isSelected);
    }

    public void CloseTargetWindow(int a)
    {
        targetWindow.OnClosed -= CloseTargetWindow;
        targetWindow = null;
    }    

    void CreateAttributeUI(PointerEventData eventData)
    {
        Vector3 mousePos = eventData.position;
        mousePos.x -= Constant.MAX_CANVAS_POS.x;
        mousePos.y -= Constant.MAX_CANVAS_POS.y;
        mousePos.z = 0f;

        WindowIconAttributeUI.OnCreateMenu?.Invoke(mousePos, windowIconData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        pointerStayImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerStayImage.gameObject.SetActive(false);
    }


}
