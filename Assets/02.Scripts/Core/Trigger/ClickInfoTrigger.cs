using System;
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

    protected Color yellowColor = new Color(1f, 1f, 0f, 0.4f);
    protected Color redColor = new Color(1f, 0f, 0f, 0.4f);
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
        if (!DataManager.Inst.SaveData.isProfilerInstall || DataManager.Inst.GetProfilerTutorialIdx() == -1) return;
        GetInfo();
        ProfileOverlaySystem.OnAdd(fileID);
        OnPointerEnter(eventData);
    }

    private void ChangeCursor(ECursorState state)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall || DataManager.Inst.GetProfilerTutorialIdx() == -1) return;
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
        if (!DataManager.Inst.SaveData.isProfilerInstall || DataManager.Inst.GetProfilerTutorialIdx() == -1) return;

        if (infoDataIDList == null || infoDataIDList.Count == 0)
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
        if (infoDataIDList == null || infoDataIDList.Count == 0) return;
        if (needInfoList == null || needInfoList.Count <= 0) return;
        if (lockImage == null)
        {
            lockImage = ResourceManager.Inst.GetLockImage();
            lockImage.transform.SetParent(transform);
            lockImage.transform.rotation = transform.rotation;
        }

        Vector2 thisSizeDelta = ((RectTransform)transform).sizeDelta;
        if(thisSizeDelta == Vector2.zero)
        {
            RectTransform rect = transform.parent as RectTransform;
            thisSizeDelta = rect.sizeDelta;
        }
        if(thisSizeDelta == Vector2.zero)
        {
            return;
        }

        RectTransform lockRect = (lockImage.transform as RectTransform);
        
        if(thisSizeDelta.x < thisSizeDelta.y)
        {
            lockRect.sizeDelta = new Vector2(thisSizeDelta.x * 0.75f, thisSizeDelta.x);
        }
        else
        {
            lockRect.sizeDelta = new Vector2(thisSizeDelta.y * 0.75f, thisSizeDelta.y);
        }

        Vector2 maxSizeDelta = new Vector2(150, 200);

        if (lockRect.sizeDelta.x > maxSizeDelta.x)
        {
            lockRect.sizeDelta = maxSizeDelta;
        }

        lockRect.localPosition = Vector3.zero;
        lockRect.localScale = Vector3.one;

        lockImage.gameObject.SetActive(isActive);
    }

    private void OnDestroy()
    {
        if (GameManager.Inst.isApplicationQuit) return;

        if (lockImage != null)
            ResourceManager.Inst.PushLockImage(lockImage);
    }

#if UNITY_EDITOR
    private void Reset()
    {
        backgroundImage = GetComponent<Image>();
    }
#endif
}