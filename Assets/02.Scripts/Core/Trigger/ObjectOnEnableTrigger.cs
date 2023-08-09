using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectOnEnableTrigger : InformationTrigger
{
    //디버그 안해본 코드. 오류 가능성 있음.
    private void OnEnable()
    {
        if (triggerData == null) return;
        if (DataManager.Inst.IsMonologShow(triggerData.monoLogType) && string.IsNullOrEmpty(triggerData.completeMonologType)) return;

        GetInfo();
    }
}
