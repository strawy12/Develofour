using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class C_P_I_1 : CallScreen
{
    public override void StartCall()
    {
        base.StartCall();
        MonologSystem.AddOnEndMonologEvent(Constant.CallScreenMonologID.POLICE_UNIVERSITY_CALLSCREEN_START, StartEvidencePanel);
        MonologSystem.OnStartMonolog?.Invoke(Constant.CallScreenMonologID.POLICE_UNIVERSITY_CALLSCREEN_START, false);
    }

    private void StartEvidencePanel()
    {
        StartCoroutine(StartEvidencePanelCor());
    }

    private IEnumerator StartEvidencePanelCor()
    {
        EventManager.StartListening(EEvidencePanelEvent.Answer, GetAnswer);
        yield return new WaitForSeconds(0.4f);


        EvidencePanel.Inst.Init("E_P_1");
    }

    private void GetAnswer(object[] obj)
    {
        EventManager.StopListening(EEvidencePanelEvent.Answer, GetAnswer);
        MonologSystem.OnStartMonolog?.Invoke(Constant.CallScreenMonologID.POLICE_UNIVERSITY_CALLSCREEN_ANSWER, false);
        MonologSystem.AddOnEndMonologEvent(Constant.CallScreenMonologID.POLICE_UNIVERSITY_CALLSCREEN_ANSWER, () => StopCall(true));
    }

    public override void StopCall(bool isClose)
    {
        DataManager.Inst.AddCallSave("C_P_I_1");
        GameManager.Inst.WindowReset();
        base.StopCall(isClose);
    }
}
