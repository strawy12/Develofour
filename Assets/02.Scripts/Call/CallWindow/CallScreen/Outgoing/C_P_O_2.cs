using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_P_O_2: CallScreen
{
    public override void StartCall()
    {
        base.StartCall();
        MonologSystem.AddOnEndMonologEvent("T_C_P_5", () => StopCall(true));
        MonologSystem.OnStartMonolog?.Invoke("T_C_P_5", false);

    }

    public override void StopCall(bool isClose)
    {
        DataManager.Inst.AddCallSave("T_C_P_5");
        base.StopCall(isClose);
    }
}
