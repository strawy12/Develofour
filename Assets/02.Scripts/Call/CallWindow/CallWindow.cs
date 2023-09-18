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
    private RectTransform callScreenTrm;

    [SerializeField]
    private DefaultCallScreen defaultCallScreen;
    [SerializeField]
    private RectTransform selectBtnsParent;
    protected override void Init()
    {
        base.Init();
        currentCanvas.sortingLayerName = "Windows";
        EventManager.StartListening(ECallEvent.EndCall, EndCall);
    }
    /// <summary>
    /// 거는 통화
    /// </summary>
    /// <param name="callProfileData"></param>
    public void Setting(CallProfileDataSO callProfileData)
    {
        if(callProfileData.id == Constant.CharacterKey.MISSING)
        {
        }
        
        InstantiateCallScreen = Instantiate(defaultCallScreen, callScreenTrm);
        defaultCallScreen.Init(selectBtnsParent);
        defaultCallScreen.Setting(callProfileData);
    }

    /// <summary>
    /// 오는 통화
    /// </summary>
    /// <param name="callID"></param>
  
    public void Setting(string callID)
    {
        CallDataSO callData = ResourceManager.Inst.GetResource<CallDataSO>(callID);
        if (callData.callScreen == null) return;
        this.callID = callID;
        
        currentCallScreen = callData.callScreen;
        StartCall();
    }
    
    public void StartCall()
    {
        if (currentCallScreen == null) return;
        if(InstantiateCallScreen != null)
        {
            Destroy(InstantiateCallScreen.gameObject);
        }

        InstantiateCallScreen = Instantiate(currentCallScreen, callScreenTrm);
        InstantiateCallScreen.Init(selectBtnsParent);
        InstantiateCallScreen.StartCall();
    }

    private void EndCall(object[] ps = null)
    {
        EndCall();
    }

    public void EndCall()
    {
        StopAllCoroutines();
        WindowClose();
        EventManager.StopListening(ECallEvent.EndCall, EndCall);
    }

    private void OnDestroy()
    {
        EventManager.StopListening(ECallEvent.EndCall, EndCall);
    }
}
