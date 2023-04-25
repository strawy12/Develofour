using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InformationTrigger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected ProfileInfoTextDataSO infomaitionData;
    [SerializeField] protected List<ProfileInfoTextDataSO> needInformaitonList;
    [SerializeField] protected List<ProfileInfoTextDataSO> linkInformaitonList;
    [SerializeField] protected Image backgroundImage;

    protected Color yellowColor = new Color(255, 255, 0, 40);
    protected Color redColor = new Color(255, 0, 0, 40);
    protected Color tempColor;

    public int monoLogType;
    public float delay;

    protected virtual void OnEnable()
    {
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();
    }

    protected virtual void Start()
    {
        tempColor = backgroundImage.color;
        if(!TriggerList.infoList.Contains(this))
        {
            TriggerList.infoList.Add(this);
        }
    }

    protected void FindInfo()
    {
        EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[2] { infomaitionData.category, infomaitionData.key});
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (infomaitionData.category == EProfileCategory.None)
        {
            MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
        }
        else
        {
            if (!DataManager.Inst.IsProfileInfoData(infomaitionData.category, infomaitionData.key))
            {
                if (needInformaitonList.Count == 0)
                {
                    GetInfo(eventData);
                }
                else
                {
                    foreach (ProfileInfoTextDataSO needData in needInformaitonList)
                    {
                        if (!DataManager.Inst.IsProfileInfoData(needData.category, needData.key))
                        {
                            MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
                            return;
                        }
                    }
                    GetInfo(eventData);
                }
            }
        }
    }

    private void GetInfo(PointerEventData eventData)
    {
        MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
        FindInfo();
        OnPointerEnter(eventData);
        TriggerList.CheckLinkInfos();
    }

    public void CheckLinkInfo()
    {
        if (!DataManager.Inst.IsProfileInfoData(infomaitionData.category, infomaitionData.key))
        {
            if (linkInformaitonList.Count != 0)
            {
                foreach (ProfileInfoTextDataSO linkData in linkInformaitonList)
                {
                    if (!DataManager.Inst.IsProfileInfoData(linkData.category, linkData.key))
                    {
                        return;
                    }
                }
                FindInfo();
            }
        }
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        CursorChangeSystem.ECursorState isListFinder = Define.ChangeInfoCursor(needInformaitonList, infomaitionData.category, infomaitionData.key);
        if (isListFinder == CursorChangeSystem.ECursorState.Default)
        {
            return;
        }

        if (!DataManager.Inst.IsProfileInfoData(infomaitionData.category, infomaitionData.key))
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

