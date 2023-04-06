using System;
using System.Collections; 
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MediaPlayerSlider : Slider, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public Action OnMousePointDown;
    public Action<float> OnMousePointUp;
    public Action<float> OnMouseSlider;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        OnMousePointDown?.Invoke();
    }
    
    public override void OnDrag(PointerEventData eventData)
    {
        base.OnDrag(eventData);

        OnMouseSlider?.Invoke(value);
    }


    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        OnMousePointUp?.Invoke(value);
    }
}
