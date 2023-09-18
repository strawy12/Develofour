using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_P_O_0 : CallScreen
{
    public override void StartCall()
    {
        base.StartCall();
        MonologSystem.AddOnEndMonologEvent("T_C_P_3", () => StopCall(true));
        MonologSystem.OnStartMonolog?.Invoke("T_C_P_3", false);

    }

    public override void StopCall(bool isClose)
    {
        DataManager.Inst.AddCallSave("C_P_O_0");
        base.StopCall(isClose);
    }
}
