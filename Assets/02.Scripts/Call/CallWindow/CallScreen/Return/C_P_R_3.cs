using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_P_R_3 : CallScreen
{
    public override void StartCall()
    {
        base.StartCall();
        MonologSystem.AddOnEndMonologEvent("T_C_P_2", () => StopCall(true));
        MonologSystem.OnStartMonolog?.Invoke("T_C_P_2", false);
    }

    public override void StopCall(bool isClose)
    {
        DataManager.Inst.AddCallSave("C_P_R_3");
        base.StopCall(isClose);
    }
}
