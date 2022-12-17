using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtenstionMethod;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image highLightImage;
    public float duration = 0.2f;
    public float maxAlpha = 0.8f;

    public  void OnPointerEnter(PointerEventData eventData)
    {
        highLightImage.DOKill(true);
        highLightImage.DOFade(maxAlpha, duration);
    }

    public void ImmediatelyStop()
    {
        highLightImage.DOKill();
        highLightImage.ChangeImageAlpha(0f);
    }

    public  void OnPointerExit(PointerEventData eventData)
    {
        highLightImage.DOKill(true);
        highLightImage.DOFade(0f, duration);
    }

}
