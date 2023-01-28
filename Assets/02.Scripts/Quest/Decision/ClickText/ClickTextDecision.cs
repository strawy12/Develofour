using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickTextDecision : Decision
{
    [SerializeField]
    private EDecisionEvent decisionEvent;

    public override void Init()
    {
        EventManager.StartListening(decisionEvent, Click);
    }

    private void Click(object[] ps)
    {
        if (!isClear)
        {
            isClear = true;
            OnChangedValue?.Invoke();
            NoticeSystem.OnNotice.Invoke(decisionName, "�۾��� �Ϸ��߽��ϴ�.", null); 
            EventManager.StopListening(decisionEvent, Click);
        }
    }

    public override bool CheckDecision()
    {
        return base.CheckDecision();
    }
}
