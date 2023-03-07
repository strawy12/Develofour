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
            NoticeSystem.OnNotice.Invoke(decisionName, "작업을 완료했습니다.", 0, true, null, ENoticeTag.None); 
            EventManager.StopListening(decisionEvent, Click);
        }
    }

    public override bool CheckDecision()
    {
        return base.CheckDecision();
    }
}
