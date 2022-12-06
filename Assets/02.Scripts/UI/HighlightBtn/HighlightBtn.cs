using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtenstionMethod;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightBtn : Button 
{
    public Image highLightImage;
    public float duration = 0.2f;
    public float maxAlpha = 0.8f;

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
        highLightImage.DOKill(true);
        highLightImage.DOFade(maxAlpha, duration);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        highLightImage.DOKill();
        highLightImage.ChangeImageAlpha(0f);
        Debug.Log(highLightImage.color.a);
        base.OnPointerClick(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
        highLightImage.DOKill(true);
        highLightImage.DOFade(0f, duration);
    }

    protected override void Reset()
    {
        base.Reset();
        transition = Transition.None;
        highLightImage = transform.GetChild(0).GetComponent<Image>();
    }
}
