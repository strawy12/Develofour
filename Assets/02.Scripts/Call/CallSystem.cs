using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;

public class CallSystem : MonoBehaviour
{
    /// <summary>
    /// string: CallCharacterID
    /// </summary>
    public static Action<string> OnOutGoingCall { get; private set; }
    public static Action<string, string> OnInComingCall { get; private set; }

    [SerializeField]
    private CallSystemUI callSystemUI;

    [SerializeField]
    private float deflaultDelayTime = 8f;

    private bool isCalling = false;
    private CallDataSO currentCallData;

    public void Start()
    {
        GameManager.Inst.OnStartCallback += Init;
    }

    private void Init()
    {
        EventManager.StartListening(EProfilerEvent.RegisterInfo, CallDataCheck);

        EventManager.StartListening(ECallEvent.ClickSelectBtn, StartCallMonolog);
        EventManager.StartListening(ECallEvent.RecivivedCall, StartCallMonolog);

        OnOutGoingCall += StartOutGoingCall;
        OnInComingCall += StartInComingCall;

        StartCoroutine(CallDataCheckTimer());
    }

    private IEnumerator CallDataCheckTimer()
    {
        WaitForSeconds waitTime = new WaitForSeconds(Constant.INCOMMING_CHECK_DELAY);
        while(true)
        {
            yield return waitTime;
            CallDataCheck(null);
        }
    }

    private void CallDataCheck(object[] ps)
    {
        var callDataList = ResourceManager.Inst.GetResourceList<CallDataSO>().Where(x => x.callDataType == ECallDataType.InComing).ToList();

        foreach (CallDataSO callData in callDataList)
        {
            if (!Define.NeedInfoFlag(callData.needInfoIDList)) continue;

            ReturnCallData returnData = DataManager.Inst.GetReturnData(callData.ID);
            if(returnData == null || returnData.EndDelayTime > DataManager.Inst.GetCurrentTime())
            {
                StartInComingCall(callData.callProfileID, callData.ID);
            }
        }
    }


    // 얘는 받는 전용
    public void StartInComingCall(string callProfileID, string callDataID)
    {
        if (isCalling) return;
        isCalling = true;

        CallProfileDataSO callProfileData = ResourceManager.Inst.GetResource<CallProfileDataSO>(callProfileID);
        if (callProfileData == null) return;

        CharacterInfoDataSO characterInfoData = ResourceManager.Inst.GetResource<CharacterInfoDataSO>(callProfileID);
        if (DataManager.Inst.IsSavePhoneNumber(characterInfoData.phoneNum) == false)
        {
            // 주소록 기능은 나중에 다시 코드를 짜십쇼
        }


        callSystemUI.InCommingCall(callProfileData);

        CallDataSO callData = ResourceManager.Inst.GetResource<CallDataSO>(callDataID);
        callSystemUI.OnClickAnswerBtn += () => StartCallMonolog(new object[] { callData });
    }

    // 얘는 거는 전용
    public void StartOutGoingCall(string callProfileID)
    {
        if (isCalling) return;
        isCalling = true;

        CallProfileDataSO data = ResourceManager.Inst.GetResource<CallProfileDataSO>(callProfileID);
        if (data == null) return;

        callSystemUI.OutGoingCall(data);
    }

    private void EndCall()
    {
        callSystemUI.Hide();
        isCalling = false;

        if (currentCallData != null)
        {
            AddFiles(currentCallData.additionFileIDList);

            if (string.IsNullOrEmpty(currentCallData.returnCallID))
            {
                CallDataSO returnCallData = ResourceManager.Inst.GetResource<CallDataSO>(currentCallData.returnCallID);
                DataManager.Inst.AddReturnCallData(returnCallData.ID, (int)returnCallData.delay);
            }

            currentCallData = null;
        }
    }

    public void StartCallMonolog(object[] ps)
    {
        if (ps.Length != 1 || !(ps[0] is CallDataSO)) return;

        CallDataSO callData = (CallDataSO)ps[0];
        string monologID = callData.monologID;
        currentCallData = callData;

        MonologSystem.AddOnEndMonologEvent(monologID, EndCall);

        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
    }

    private void AddFiles(List<AdditionFile> additionFiles)
    {
        foreach (AdditionFile additionFile in additionFiles)
        {
            FileManager.Inst.AddFile(additionFile.fileID, additionFile.directoryID);
        }
    }
}
