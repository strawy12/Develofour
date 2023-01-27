using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HateBtnDecision : Decision
{

    public override void Init()
    {
        EventManager.StartListening(EYoutubeSiteEvent.ClickHateBtn, ClickHateBtn);
    }

    private void ClickHateBtn(object[] emptyParam)
    {
        if (isClear) { return; }
        isClear = true;

        OnChangedValue?.Invoke();
        EventManager.StopListening(EYoutubeSiteEvent.ClickHateBtn, ClickHateBtn);
    }


}
