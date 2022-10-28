using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WindowBar : MonoBehaviour,IDragHandler,IPointerDownHandler, IBeginDragHandler
{
    private Window targetWindow;
    private Vector3 offset;

    public void Init(Window target)
    {
        targetWindow = target;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Define.MainCam.ScreenToWorldPoint(eventData.position);
        mousePos.z = 0f;
        offset = targetWindow.rectTransform.position - mousePos;

    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePos = Define.MainCam.ScreenToWorldPoint(eventData.position);
        mousePos.z = 0f;
        targetWindow.rectTransform.position = mousePos + offset;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetWindow.SelectWindow();
    }
}
