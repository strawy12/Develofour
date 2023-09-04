using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CallWindow : Window
{
    private bool isCalling = false;

    private CallScreen currentCallScreen;
    private CallScreen InstantiateCallScreen;
    private string callID;

    [SerializeField]
    private Transform callScreenTrm;

    protected override void Init()
    {
        base.Init();
    }

    public void Setting(string callID)
    {
        this.callID = callID;
        CallDataSO callData = ResourceManager.Inst.GetResource<CallDataSO>(callID);
        currentCallScreen = callData.callScreen;
    }

    public void StartCall()
    {
        InstantiateCallScreen = Instantiate(currentCallScreen, GameManager.Inst.CutSceneCanvas.transform);
        currentCallScreen.StartCall();
    }
}
