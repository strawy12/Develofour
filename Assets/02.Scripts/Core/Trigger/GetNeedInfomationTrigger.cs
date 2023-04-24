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
    private EMonologTextDataType notFinderNeedStringMonoLog;

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
        if (category == EProfileCategory.None)
        {
            MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
        }
        else
        {
            if (!DataManager.Inst.IsProfileInfoData(category, information))  // ã�� �������� Ȯ��
            {
                if (needInformaitonList.Count == 0) // ã�� ������ ������
                {
                    MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
                    EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, information, null });
                    OnPointerEnter(eventData);
                }
                else // ã�ƾ� �ϴ� �ܾ ������
                {
                    foreach (ProfileInfoTextDataSO needData in needInformaitonList) // ����Ʈ�� Ȯ��
                    {
                        if (!DataManager.Inst.IsProfileInfoData(needData.category, needData.key))
                            // �� ã���� �ִٸ�
                        {
                            MonologSystem.OnStartMonolog?.Invoke(notFinderNeedStringMonoLog, delay, true);
                            return;
                        }
                    }

                    // return �ȵǸ� ã������
                    MonologSystem.OnStartMonolog?.Invoke(monoLogType, delay, true);
                    EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[3] { category, information, null });
                }
            }
        }
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);
    }
}
