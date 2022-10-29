using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class WindowIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectable
{
    public RectTransform rectTranstform;

    private bool isSelected = false;

    private WindowIconData data;

    private Window targetWindow = null;


    [SerializeField]
    private Image iconImage;
    [SerializeField]
    private Image selectedImage;
    [SerializeField]
    private Image pointerStayImage;


    [SerializeField]
    private TMP_Text iconNameText;
    
    public Action OnSelected { get; set; }
    public Action OnUnSelected { get; set; }

    public void Create(WindowIconData windowIconData)
    {
        data = windowIconData;

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
        if(isSelected == false)
        {
            WindowManager.Inst.SelectObject(this);
        }
        else // task바로 이벤트 쏠거임
        {
            if(targetWindow == null)
            {

            }
            else
            {
                targetWindow.Open();
            }
        }
    }

    private void CreateWindow()
    {
        targetWindow.CreateWindow();
        targetWindow.OnClose += (id) => targetWindow = null;
    }

    private void SelectedIcon(bool isSelected)
    {
        this.isSelected = isSelected;
        selectedImage.gameObject.SetActive(isSelected);
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
