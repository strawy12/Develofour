using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chapter1_1_PoliceUniversity : CallScreen
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
        EvidenceType type = new EvidenceType();
        type.maxCount = 3;
        type.selectMonolog = Constant.CallScreenMonologID.POLICE_UNIVERSITY_CALLSCREEN_PRESENT;
        type.wrongHintMonolog = Constant.CallScreenMonologID.POLICE_UNIVERSITY_CALLSCREEN_WRONG_HINT;
        type.wrongMonolog = Constant.CallScreenMonologID.POLICE_UNIVERSITY_CALLSCREEN_WRONG;
        Debug.Log(EvidencePanel.evidencePanel);
        Debug.Log(type);
        EvidencePanel.evidencePanel.Init("I_I_1_1", type);
    }

    private void GetAnswer(object[] obj)
    {
        EventManager.StopListening(EEvidencePanelEvent.Answer, GetAnswer);
        MonologSystem.OnStartMonolog?.Invoke(Constant.CallScreenMonologID.POLICE_UNIVERSITY_CALLSCREEN_ANSWER, false);
        MonologSystem.AddOnEndMonologEvent(Constant.CallScreenMonologID.POLICE_UNIVERSITY_CALLSCREEN_ANSWER, () => StopCall(true));
    }

    public override void StopCall(bool isClose)
    {
        DataManager.Inst.AddCallSave("C_A_1_1");
        base.StopCall(isClose);
    }
}
