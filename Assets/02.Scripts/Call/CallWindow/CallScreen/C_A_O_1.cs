using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_A_O_1 : CallScreen
{
    public override void StartCall()
    {
        base.StartCall();
        MonologSystem.AddOnEndMonologEvent("T_C_A_4", () => StopCall(true));
        MonologSystem.OnStartMonolog?.Invoke("T_C_A_4", false);

    }

    public override void StopCall(bool isClose)
    {
        DataManager.Inst.AddCallSave("C_A_O_1");
        base.StopCall(isClose);
    }
}
