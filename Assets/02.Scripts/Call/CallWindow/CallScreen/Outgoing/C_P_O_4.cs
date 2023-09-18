using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_P_O_4: CallScreen
{
    public override void StartCall()
    {
        base.StartCall();
        MonologSystem.AddOnEndMonologEvent("T_C_P_10", () => StopCall(true));
        MonologSystem.OnStartMonolog?.Invoke("T_C_P_10", false);

    }

    public override void StopCall(bool isClose)
    {
        DataManager.Inst.AddCallSave("C_P_O_4");
        base.StopCall(isClose);
    }
}
