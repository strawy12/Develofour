using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_A_R_0 : CallScreen
{
    public override void StartCall()
    {
        base.StartCall();
        MonologSystem.AddOnEndMonologEvent("T_C_A_8", () => StopCall(true));
        MonologSystem.OnStartMonolog?.Invoke("T_C_A_8", false);

    }

    public override void StopCall(bool isClose)
    {
        DataManager.Inst.AddCallSave("C_A_R_0");
        base.StopCall(isClose);
    }
}
