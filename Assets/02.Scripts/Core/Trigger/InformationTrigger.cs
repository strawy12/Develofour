using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class InformationTrigger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public EProfileCategory category;
    public string information;
    public List<ProfileInfoTextDataSO> needInformaitonList;
    public Image backgroundImage;
    private Color yellowColor = new Color(255, 255, 0, 40);
    private Color redColor = new Color(255, 0, 0, 40);
    private Color tempColor;

    protected virtual void OnEnable()
    {
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();
    }
    protected virtual void Start()
    {
        tempColor = backgroundImage.color;
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!DataManager.Inst.IsProfileInfoData(category, information))
        {
            if (needInformaitonList.Count == 0)
            {
                EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, information, null });
            }
            else
            {
                EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, information, needInformaitonList });
            }
        }

        OnPointerEnter(eventData);
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        CursorChangeSystem.ECursorState isListFinder = Define.ChangeInfoCursor(needInformaitonList, category, information);
        if (isListFinder == CursorChangeSystem.ECursorState.Default)
        {
            return;
        }

        if (!DataManager.Inst.IsProfileInfoData(category, information))
        {
            yellowColor.a = 0.4f;
            backgroundImage.color = yellowColor;
        }
        else
        {
            redColor.a = 0.4f;
            backgroundImage.color = redColor;

        }

    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { CursorChangeSystem.ECursorState.Default });
        backgroundImage.color = tempColor;

    }
}
