using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     
using DG.Tweening;

public class UIRectWidthMove : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float moveDir;
    public float delay;

    private RectTransform rectTransform;

    private float defaultWidth;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();  

        defaultWidth = rectTransform.sizeDelta.x;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rectTransform.DOKill(false);

        rectTransform.DOSizeDelta(new Vector2(defaultWidth * moveDir, rectTransform.sizeDelta.y), delay);
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        rectTransform.DOKill(false);

        rectTransform.DOSizeDelta(new Vector2(defaultWidth, rectTransform.sizeDelta.y), delay);
    }
}
