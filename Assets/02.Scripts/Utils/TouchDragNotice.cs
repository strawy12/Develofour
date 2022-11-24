using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchDragNotice : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public bool isClick = false;
    
    private bool HV = false;
    private bool isPlma = false;

    private bool isMove = false;

    private Vector3 saveOriginalPos;
    private Vector3 saveBeginDragPos;
    private Vector3 saveEndDragPos;

    private RectTransform rectTransform;

    public Action OnDragNotice;
    public Action OnClickNotice;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        saveOriginalPos = rectTransform.localPosition;
        
        isClick = true;
        OnClickNotice?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isClick = false;
        OnClickNotice?.Invoke();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isMove = false;

        HV = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);
     
        saveBeginDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 changeSavePos = saveBeginDragPos;

        if (HV) // Horizontal
        {
            isPlma = eventData.delta.x > 0; // right
            
            Vector3 dragPanelPos = changeSavePos - eventData.position;
            dragPanelPos.y = 0;
            dragPanelPos.z = 0;

            Vector3 movePos = rectTransform.localPosition - dragPanelPos;
            if (movePos.x < Constant.NOTICEDRAG_MAXPOS)
            {
                return;
            }

            rectTransform.localPosition = movePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        saveEndDragPos = eventData.position;
        float moveDistance = saveBeginDragPos.x - saveEndDragPos.x;

        if (moveDistance < Constant.NOTICEDRAG_INTERVAL)
        // 첫 시작 마우스 pos와 끝 마우스 pos의 격차가 일정 이상 존재한다면
        {
            isMove = true;
            // 드래그 효과 on
        }

        if (isPlma && isMove)
        {
            OnDragNotice?.Invoke();
        }
        else
        {
            rectTransform.localPosition = saveOriginalPos;
        }
    }
}