using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_A_O_4 : CallScreen
{
    public override void StartCall()
    {
        base.StartCall();
        MonologSystem.AddOnEndMonologEvent("T_C_A_7", () => StopCall(true));
        MonologSystem.OnStartMonolog?.Invoke("T_C_A_7", false);

    }

    public override void StopCall(bool isClose)
    {
        DataManager.Inst.AddCallSave("C_A_O_4");
        base.StopCall(isClose);
    }
}
