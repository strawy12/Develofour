using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NotUseIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, ISelectable
{
    //[SerializeField]
    //private Image iconImage;
    //[SerializeField]
    //private TMP_Text iconNameText;

    [SerializeField]
    private Image selectedImage;
    [SerializeField]
    private Image pointerStayImage;
    public Action OnSelected { get; set; }
    public Action OnUnSelected { get; set; }

    private void Awake()
    {
        OnSelected += () => SelectedIcon(true);
        OnUnSelected += () => SelectedIcon(false);
    }

    private void Start()
    {
        pointerStayImage.gameObject.SetActive(false);
        selectedImage.gameObject.SetActive(false);
    }

    private void SelectedIcon(bool isSelected)
    {
        // this.isSelected = isSelected;
        selectedImage.gameObject.SetActive(isSelected);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        selectedImage.gameObject.SetActive(true);
        WindowManager.Inst.SelectObject(this);
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