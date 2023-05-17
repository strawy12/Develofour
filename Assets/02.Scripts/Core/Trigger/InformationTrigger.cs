﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InformationTrigger : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] protected List<int> needInfoIDList;
    [SerializeField] protected List<int> linkInfoIDList;
    [SerializeField] protected int infomaitionDataId;
    [SerializeField] protected Image backgroundImage;

    protected ProfileInfoTextDataSO infomaitionData;
    protected List<ProfileInfoTextDataSO> needInformaitonList;
    protected List<ProfileInfoTextDataSO> linkInformaitonList;

    protected Color yellowColor = new Color(255, 255, 0, 40);   
    protected Color redColor = new Color(255, 0, 0, 40);
    protected Color tempColor;

    public int monoLogType;
    public float delay;

    [SerializeField] protected bool isFakeInfo;

    protected virtual void OnEnable()
    {
        if (backgroundImage == null)
            backgroundImage = GetComponent<Image>();
    }

    protected virtual void Start()
    {
        tempColor = backgroundImage.color;
        infomaitionData = ResourceManager.Inst.GetProfileInfoData(infomaitionDataId);

        if (!TriggerList.infoList.Contains(this))
        {
            TriggerList.infoList.Add(this);
        }
    }
    protected void Bind()
    {
        infomaitionData ??= ResourceManager.Inst.GetProfileInfoData(infomaitionDataId);
        if(needInfoIDList.Count != 0 && needInformaitonList == null)
        {
            needInformaitonList = new List<ProfileInfoTextDataSO>();
            foreach(var id in needInfoIDList)
            {
                needInformaitonList.Add(ResourceManager.Inst.GetProfileInfoData(id));
            }
        }
        if (linkInfoIDList.Count != 0 && linkInformaitonList == null)
        {
            linkInformaitonList = new List<ProfileInfoTextDataSO>();
            foreach (var id in linkInfoIDList)
            {
                linkInformaitonList.Add(ResourceManager.Inst.GetProfileInfoData(id));
            }
        }
    }
    protected void FindInfo()
    {
        Bind();
        EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[2] { infomaitionData.category, infomaitionData.id});
    }

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        Bind();

        if (infomaitionData == null || infomaitionData.category == EProfileCategory.None)
        {
            MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
            return;
        }

        else
        {
            if (needInformaitonList.Count == 0)
            {
                GetInfo(eventData);
            }
            else
            {
                foreach (ProfileInfoTextDataSO needData in needInformaitonList)
                {
                    if (!DataManager.Inst.IsProfileInfoData(needData.id))
                    {
                        if (monoLogType == -1)
                            return;
                        MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
                        return;
                    }
                }
                GetInfo(eventData);
            }

        }
    }

    private void GetInfo(PointerEventData eventData)
    {
        MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
       
        FindInfo();
        TriggerList.CheckLinkInfos();

        OnPointerEnter(eventData);
    }

    public void CheckLinkInfo()
    {
        if(infomaitionData == null) { return; }

        if (!DataManager.Inst.IsProfileInfoData(infomaitionData.id))
        {
            if (linkInformaitonList.Count != 0)
            {
                foreach (ProfileInfoTextDataSO linkData in linkInformaitonList)
                {
                    if (!DataManager.Inst.IsProfileInfoData(linkData.id))
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
        if (infomaitionData == null)
        {
            EventManager.TriggerEvent(ECoreEvent.CursorChange, new object[] { CursorChangeSystem.ECursorState.FindInfo });
            yellowColor.a = 0.4f;
            backgroundImage.color = yellowColor;
            return;
        }

        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }

        CursorChangeSystem.ECursorState isListFinder = Define.ChangeInfoCursor(needInformaitonList, infomaitionData.category, infomaitionData.id);
        if (isListFinder == CursorChangeSystem.ECursorState.Default)
        {
            return;
        }

        if (!DataManager.Inst.IsProfileInfoData(infomaitionData.id))
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

    //[ContextMenu("SetInfoID")]
    //public void SetInfoID()
    //{
    //    if(infomaitionData != null)
    //     getInfoID = infomaitionData.id;

    //    foreach(var infoID in needInformaitonList)
    //    {
    //        needInfoIDList.Add(infoID.id);
    //    }

    //    foreach (var infoID in linkInformaitonList)
    //    {
    //        linkInfoIDList.Add(infoID.id);
    //    }
    //}
}

