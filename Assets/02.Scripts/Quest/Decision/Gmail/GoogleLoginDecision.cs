using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoogleLoginDecision : Decision
{
    private bool successLogin = false;
    public override void Init()
    {
        successLogin = false;
        EventManager.StartListening(ELoginSiteEvent.EmailLoginSuccess, SuccessLogin);
    }

    private void SuccessLogin(object[] obj)
    {
        successLogin = true;
        OnChangedValue?.Invoke();
        EventManager.StopListening(ELoginSiteEvent.EmailLoginSuccess, SuccessLogin);
    }


    public override bool CheckDecision()
    {
        return successLogin;
    }

}
