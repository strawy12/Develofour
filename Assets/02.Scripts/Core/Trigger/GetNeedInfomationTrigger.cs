using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GetNeedInfomationTrigger : InformationTrigger
{
    [Header("MonoLogTexts")]
    [SerializeField]
    protected int notFinderNeedStringMonoLog;

    [SerializeField]
    private bool isGetInfomation;

    protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (infomaitionDataList == null || infoDataIDList.Count == 0)
        {
            MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
        }
        else
        {
            if (needInformaitonList.Count == 0) // 찾는 조건이 없으면
            {
                MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
                FindInfo();
                OnPointerEnter(eventData);
            }
            else // 찾아야 하는 단어가 있으면
            {
                foreach (ProfileInfoTextDataSO needData in needInformaitonList) // 리스트로 확인
                {
                    if (!DataManager.Inst.IsProfileInfoData(needData.id))
                    // 안 찾은게 있다면
                    {
                        MonologSystem.OnStartMonolog?.Invoke(notFinderNeedStringMonoLog, delay, true);
                        if (isGetInfomation)
                        {
                            FindInfo();
                            OnPointerEnter(eventData);
                        }
                        return;
                    }
                }

                // return 안되면 찾은거임
                MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
                FindInfo();
                OnPointerEnter(eventData);
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (!DataManager.Inst.SaveData.isProfilerInstall)
        {
            return;
        }
        CursorChangeSystem.ECursorState isListFinder = Define.ChangeInfoCursor(needInformaitonList, infoDataIDList);

        if (isListFinder == CursorChangeSystem.ECursorState.Default)
        {
            return;
        }

        if (isListFinder == CursorChangeSystem.ECursorState.FindInfo)
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
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }
}