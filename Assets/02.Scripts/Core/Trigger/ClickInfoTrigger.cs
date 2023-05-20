﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static CursorChangeSystem;

public class ClickInfoTrigger : InformationTrigger, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected Image backgroundImage;

    protected Color yellowColor = new Color(255, 255, 0, 40);
    protected Color redColor = new Color(255, 0, 0, 40);
    protected Color tempColor;
    private GameObject lockImage;

    protected override void Bind()
    {
        if (backgroundImage == null)
        {
            backgroundImage = GetComponent<Image>();
            tempColor = backgroundImage.color;
        }

        base.Bind();
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        GetInfo();
        OnPointerEnter(eventData);
    }

    private void ChangeCursor(ECursorState state)
    {
        switch (state)
        {
            case ECursorState.Default:
                backgroundImage.color = tempColor;
                ActiveLockImage(false);
                break;
            case ECursorState.NeedInfo:
                backgroundImage.color = yellowColor;
                ActiveLockImage(true);
                break;
            case ECursorState.FindInfo:
                backgroundImage.color = yellowColor;
                ActiveLockImage(false);
                break;
            case ECursorState.FoundInfo:
                backgroundImage.color = redColor;
                ActiveLockImage(false);
                break;
        }

        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { state });
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        if (infomaitionDataList == null || infoDataIDList.Count == 0)
        {
            ChangeCursor(ECursorState.FindInfo);
            return;
        }

        ECursorState isListFinder = Define.ChangeInfoCursor(needInfoList, infoDataIDList);
        ChangeCursor(isListFinder);
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        ChangeCursor(ECursorState.Default);
    }
    private void ActiveLockImage(bool isActive)
    {
        if (needInfoList == null || needInfoList.Count <= 0) return;
        if (lockImage == null)
        {
            lockImage = ResourceManager.Inst.GetLockImage();
            lockImage.transform.SetParent(transform);
        }

        lockImage.gameObject.SetActive(isActive);
    }
#if UNITY_EDITOR
    private void Reset()
    {
        backgroundImage = GetComponent<Image>();
    }
#endif
}