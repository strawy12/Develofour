using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HateBtnDecision : Decision
{
    private bool isClickHateBtn;

    public override void Init()
    {
        EventManager.StartListening(EEvent.ClickHateBtn, ClickHateBtn);
    }

    private void ClickHateBtn(object emptyParam)
    {
        if (isClickHateBtn) { return; }
        isClickHateBtn = true;

        OnChangedValue?.Invoke();
        EventManager.StopListening(EEvent.ClickHateBtn, ClickHateBtn);
    }

    public override bool CheckDecision()
    {
        return isClickHateBtn;
    }


}
