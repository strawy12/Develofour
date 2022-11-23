using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchDragNotice : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private bool HV = false;
    private bool isPlma = false;
    private bool isDrag = false;

    private bool isMove = false;

    private Vector3 saveBeginDragPos;
    private Vector3 saveEndDragPos;

    public Action OnDragNotice;

    public void OnBeginDrag(PointerEventData eventData)
    {
        isMove = false;
        HV = Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y);
     
        saveBeginDragPos = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (HV) // Horizontal
        {
            isPlma = eventData.delta.x > 0;
            
            if(isPlma) // Right Move
            {

            }
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        saveEndDragPos = eventData.position;
        float moveDistance = saveBeginDragPos.x - saveEndDragPos.x;

        Debug.Log(moveDistance);

        if (moveDistance < Constant.NOTICEDRAG_INTERVAL)
        // 첫 시작 마우스 pos와 끝 마우스 pos의 격차가 대충 300~400 차이면
        {
            isMove = true;
            // 드래그 효과 on
        }

        if (isPlma && isMove)
        {
            OnDragNotice?.Invoke();
        }
    }
}