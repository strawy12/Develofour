using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GetInformationTrigger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public EProfileCategory category;
    public string information;
    public List<ProfileInfoTextDataSO> needInformaitonList;
    public Image backgroundImage;
    private Color yellowColor = new Color(255,255,0,40);
    private Color redColor = new Color(255, 0, 0, 40);

    void OnEnable()
    {
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(!DataManager.Inst.IsProfileInfoData(category, information))
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
        backgroundImage.color = redColor;
        var tempColor = backgroundImage.color;
        tempColor.a = 0.2f;
        backgroundImage.color = tempColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        if (!DataManager.Inst.IsProfileInfoData(category, information))
        {
            backgroundImage.color = yellowColor;
            var tempColor = backgroundImage.color;
            tempColor.a = 0.2f;
            backgroundImage.color = tempColor;
        }

        Define.ChangeInfoCursor(needInformaitonList, category, information);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { CursorChangeSystem.ECursorState.Default });
        if(!DataManager.Inst.IsProfileInfoData(category, information))
        {
            backgroundImage.color = new Color(0, 0, 0, 0);
        }

    }
}
