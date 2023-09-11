using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_P_R_2 : CallScreen
{
    public override void StartCall()
    {
        base.StartCall();
        MonologSystem.AddOnEndMonologEvent("T_C_P_13", () => StopCall(true));
        MonologSystem.OnStartMonolog?.Invoke("T_C_P_13", false);
    }

    public override void StopCall(bool isClose)
    {
        DataManager.Inst.AddCallSave("T_C_P_13");
        base.StopCall(isClose);
    }
}
