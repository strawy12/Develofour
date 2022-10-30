using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class StartAttributeBtn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static readonly float DURATION = 0.15f;

    public Action<PointerEventData> OnEnter;

    private Image backgroundImage;
    private Color defaultColor;
    private Color highLightColor;

    private void Awake()
    {
        Bind();
        Init();
    }

    private void Bind()
    {
        backgroundImage = GetComponent<Image>();
    }

    private void Init()
    {
        float color = 81f / 256f;
        highLightColor = new Color(color, color, color);

        color = 41f / 256f;
        defaultColor = new Color(color, color, color);

        if(backgroundImage.color != defaultColor)
        {
            backgroundImage.color = defaultColor;
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter?.Invoke(eventData);
        backgroundImage.DOColor(highLightColor, DURATION);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        backgroundImage.DOColor(defaultColor, DURATION);
    }
}
