using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleLoginDecision : Decision
{
    public override void Init()
    {
        isClear = false;
        EventManager.StartListening(ELoginSiteEvent.LoginSuccess, SuccessLogin);
    }

    private void SuccessLogin(object[] obj)
    {
        isClear = true;
        OnChangedValue?.Invoke();
        EventManager.StopListening(ELoginSiteEvent.LoginSuccess, SuccessLogin);
    }


}
