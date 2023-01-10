using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoliceMiniGameDecision : Decision
{
    private bool isClear = false;
    public override void Init()
    {
        EventManager.StartListening(EMailSiteEvent.PoliceGameClear, MiniGameClear);
    }
    
    private void MiniGameClear(object[] dummy)
    {
        Debug.Log("게임 클리어");
        if (isClear) { return; }
        isClear = true;
        
        OnChangedValue?.Invoke();
        EventManager.StopListening(EMailSiteEvent.PoliceGameClear, MiniGameClear);
    }

    public override bool CheckDecision()
    {
        return true;
    }
}
