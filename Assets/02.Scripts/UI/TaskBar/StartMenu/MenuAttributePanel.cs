using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DG.Tweening;

public class MenuAttributePanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public static readonly float DURATION = 0.15f;

    public Action<PointerEventData> OnEnter;

    private Image backgroundImage;

    private void Awake()
    {
        Init();
        Bind();
    }
    protected virtual void Init()
    {

    }

    private void Bind()
    {
        backgroundImage = GetComponent<Image>();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        OnEnter?.Invoke(eventData); 
        SelectedPanel(true);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        SelectedPanel(false);
    }

    protected void SelectedPanel(bool isSelected)
    {
        backgroundImage.DOKill(true);
        float alpha = isSelected ? 1f : 0f;
        backgroundImage.DOFade(alpha, DURATION);
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        //EventManager.TriggerEvent(EWindowEvent.ExpendMenu);
    }
}
