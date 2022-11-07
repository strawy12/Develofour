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

    private int cilckCount = 0;

    private Window targetWindow = null;
    private Sprite sprite;

    [SerializeField]
    private Window targetWindowTemp;
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
            if (cilckCount != 0)
            {
                // 여기에서 이벤트 쏨
                cilckCount = 0;

                if (targetWindow == null)
                {
                    CreateWindow();
                    TaskBar.OnAddIcon?.Invoke(targetWindow);
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
                cilckCount++;
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            WindowManager.Inst.SelectObject(this);
            CreateAttributeUI(eventData);
        }
    }

    private void CreateWindow()
    {
        targetWindow = Instantiate(targetWindowTemp, targetWindowTemp.transform.parent);
        targetWindow.CreatedWindow();

        targetWindow.OnClosed += (int x) => targetWindow = null;  
        
    }

    private void SelectedIcon(bool isSelected)
    {
        if(!isSelected)
        {
            cilckCount = 0;
        }
        selectedImage.gameObject.SetActive(isSelected);
    }

    void CreateAttributeUI(PointerEventData eventData)
    {
        Vector3 mousePos = eventData.position;
        mousePos.x -= Constant.MAXCANVASPOS.x;
        mousePos.y -= Constant.MAXCANVASPOS.y;
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
