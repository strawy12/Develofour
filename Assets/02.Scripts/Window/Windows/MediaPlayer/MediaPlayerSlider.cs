using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MediaPlayerSlider : Slider, IPointerDownHandler, IPointerUpHandler
{
    public Action OnMousePointUp;
    public Action<float> OnMousePointDown;

    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        OnMousePointDown?.Invoke(value);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        OnMousePointUp?.Invoke();
    }
}
