using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ClickTextDecision : Decision
{
    [SerializeField]
    private EDecisionEvent decisionEvent;
    [SerializeField]
    private ENoticeType decisionNoticeType;

    public override void Init()
    {
        Debug.Log("StartListening");
        EventManager.StartListening(decisionEvent, Click);
    }

    private void Click(object[] ps)
    {
        if (!isClear)
        {
            isClear = true;
            Debug.Log("Complete");
            OnChangedValue?.Invoke();
            CheckDecision();
            NoticeSystem.OnGeneratedNotice.Invoke(decisionNoticeType, 0);
            EventManager.StopListening(decisionEvent, Click);
        }
    }

    public override bool CheckDecision()
    {
        return base.CheckDecision();
    }
}
