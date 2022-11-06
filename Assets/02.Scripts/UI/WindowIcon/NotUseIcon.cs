using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.ShaderGraph.Serialization;
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

    [Header("Skaking Data")]
    [SerializeField]
    private int strength;
    [SerializeField]
    private int vibrato;
    [SerializeField]
    private float duration;
    [SerializeField]
    private Color shakingColor;



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
            ShakingIcon();
            isSelected = false;
            pointerStayImage.gameObject.SetActive(false);
        }

        else
        {
            selectedImage.gameObject.SetActive(true);
            isSelected = true;
        }
    }

    private void ShakingIcon()
    {
        if (isShaking) return;
        isShaking = true;
        selectedImage.DOKill();
        transform.DOKill(true);
        Color originColor = selectedImage.color;
        selectedImage.color = shakingColor;
        transform.DOShakePosition(duration, strength, vibrato).OnComplete(() =>
        {
            selectedImage.color = originColor;
            isShaking = false;
            selectedImage.gameObject.SetActive(false);
        });
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