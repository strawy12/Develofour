using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class NoticeBtn : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Value")]
    [SerializeField]
    private float duration = 0.2f;
    [SerializeField]
    private float highLightOpacity = 0.5f;

    private Image currentImage;

    private void Awake()
    {
        currentImage = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HighLightBtn(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        HighLightBtn(false);
    }


    private void HighLightBtn(bool isHighLight)
    {
        currentImage.DOFade(isHighLight ? highLightOpacity : 0f, duration);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            EventManager.TriggerEvent(EEvent.ClickNoticeBtn);
        }
    }
}
