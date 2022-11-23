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
        // ù ���� ���콺 pos�� �� ���콺 pos�� ������ ���� 300~400 ���̸�
        {
            isMove = true;
            // �巡�� ȿ�� on
        }

        if (isPlma && isMove)
        {
            OnDragNotice?.Invoke();
        }
    }
}