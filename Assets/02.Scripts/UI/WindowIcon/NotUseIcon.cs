using DG.Tweening;
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

    private int clickCount = 0;

    [SerializeField]
    private Image selectedImage;
    [SerializeField]
    private Image pointerStayImage;
    [SerializeField]
    private Image shakingImage;

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
    }

    public bool IsSelected(GameObject hitObject)
    {
        bool flag1 = hitObject == gameObject;

        return flag1 && isSelected;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isShaking) return;

        if (clickCount != 0)
        {
            clickCount = 0;

            ShakingIcon();

            WindowManager.Inst.SelectedObjectNull();
        }
        else
        {
            WindowManager.Inst.SelectObject(this);
            clickCount++;
        }
    }

    private void SelectedIcon(bool isSelected)
    {
        if (!isSelected)
        {
            clickCount = 0;
        }

        this.isSelected = isSelected;
        selectedImage.gameObject.SetActive(isSelected);
    }

    private void ShakingIcon()
    {
        if (isShaking) return;

        isShaking = true;
        shakingImage.gameObject.SetActive(true);

        Sound.OnPlayEffectSound?.Invoke(Sound.EEffect.NotUseIconClick);

        shakingImage.DOKill();
        transform.DOKill(true);
        Color originColor = shakingImage.color;
        shakingImage.color = shakingColor;
        transform.DOShakePosition(duration, strength, vibrato).OnComplete(() =>
        {
            shakingImage.color = originColor;
            isShaking = false;

            selectedImage.gameObject.SetActive(false);
            shakingImage.gameObject.SetActive(false);
        });
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isShaking) return;
        pointerStayImage.gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        pointerStayImage.gameObject.SetActive(false);
    }


}