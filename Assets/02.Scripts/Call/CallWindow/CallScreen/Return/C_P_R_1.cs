using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_P_R_1 : CallScreen
{
    public override void StartCall()
    {
        base.StartCall();
        MonologSystem.AddOnEndMonologEvent("T_C_P_12", () => StopCall(true));
        MonologSystem.OnStartMonolog?.Invoke("T_C_P_12", false);
    }

    public override void StopCall(bool isClose)
    {
        DataManager.Inst.AddCallSave("C_P_R_1");
        base.StopCall(isClose);
    }
}
