using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NotUseIcon : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    //[SerializeField]
    //private Image iconImage;
    //[SerializeField]
    //private TMP_Text iconNameText;

    [SerializeField]
    private Image selectedImage;
    [SerializeField]
    private Image pointerStayImage;

    private bool isSelected = false;
    private bool isShaking = false;

    private void Start()
    {
        pointerStayImage.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isShaking) return;

        if (isSelected)
        {
            isShaking = true;
            transform.DOKill(true);
            transform.DOShakePosition(0.3f, 25, 70);
            isSelected = false;
            selectedImage.gameObject.SetActive(false);
        }

        else
        {
            selectedImage.gameObject.SetActive(true);
            isSelected = true;
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isShaking) return;
        pointerStayImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isShaking) return;
        pointerStayImage.gameObject.SetActive(false);
    }
}