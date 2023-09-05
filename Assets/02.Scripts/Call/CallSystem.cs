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
        GameManager.Inst.OnGameStartCallback += Init;
    }

    private void Init()
    {
        EventManager.StartListening(EProfilerEvent.RegisterInfo, IncomingCallDataCheck);
        EventManager.StartListening(ECallEvent.ClickSelectBtn, StartCallMonolog);
        EventManager.StartListening(ECallEvent.RecivivedCall, StartCallMonolog);
        EventManager.StartListening(ECallEvent.EndCall, EndCall);
        OnOutGoingCall += StartOutGoingCall;
        OnInComingCall += StartInComingCall;
        callSystemUI.Init();
        StartCoroutine(CallDataCheckTimer());

        isCalling = false;
    }

    private IEnumerator CallDataCheckTimer()
    {
        WaitForSeconds waitTime = new WaitForSeconds(Constant.INCOMMING_CHECK_DELAY);
        while (true)
        {
            yield return waitTime;
            IncomingCallDataCheck(null);
        }
    }

    private void IncomingCallDataCheck(object[] ps)
    {
        var callDataList = ResourceManager.Inst.GetResourceList<CallDataSO>().Where(x => x.callDataType == ECallDataType.InComing).ToList();

        foreach (CallDataSO callData in callDataList)
        {
            if (DataManager.Inst.IsSaveCallData(callData.id)) continue;
            if (!Define.NeedInfoFlag(callData.needInfoIDList)) continue;
            ReturnCallData returnData = DataManager.Inst.GetReturnData(callData.ID);
            if (returnData != null && returnData.EndDelayTime <= DataManager.Inst.GetCurrentTime())
            {
                StartInComingCall(callData.callProfileID, callData.ID);
            }
        }
    }


    // 얘는 받는 전용
    public void StartInComingCall(string callProfileID, string callDataID)
    {
        if (isCalling)
        {
            Debug.Log("isCalling = true");
            return;
        }
        isCalling = true;

        CallProfileDataSO callProfileData = ResourceManager.Inst.GetResource<CallProfileDataSO>(callProfileID);

        if (callProfileData == null) return;
        CharacterInfoDataSO characterInfoData = ResourceManager.Inst.GetResource<CharacterInfoDataSO>(callProfileID);
        if (DataManager.Inst.IsSavePhoneNumber(characterInfoData.phoneNum) == false)
        {
            EventManager.TriggerEvent(ECallEvent.AddAutoCompleteCallBtn, new object[1] { characterInfoData.phoneNum });
        }


        callSystemUI.InCommingCall(callProfileData);

        CallDataSO callData = ResourceManager.Inst.GetResource<CallDataSO>(callDataID);
        callSystemUI.OnClickAnswerBtn += () => StartCallMonolog(new object[] { callData });
    }

    // 얘는 거는 전용
    public void StartOutGoingCall(string characterID)
    {
        Debug.Log(isCalling);
        if (isCalling) return;
        isCalling = true;


        CallProfileDataSO data = ResourceManager.Inst.GetResource<CallProfileDataSO>(characterID);
        if (data == null)
        {
            Debug.LogWarning($"{characterID}를 id로 하는 데이터가 존재하지않습니다. CallProfilerDataSO의 id값을 확인해 주세요.");
            isCalling = false;
            return;
        }
        CharacterInfoDataSO characterData = ResourceManager.Inst.GetResource<CharacterInfoDataSO>(characterID);

        if (characterData != null)
        {
            EventManager.TriggerEvent(ECallEvent.AddAutoCompleteCallBtn, new object[1] { characterData.phoneNum });
        }

        callSystemUI.OutGoingCall(data);
    }

    private void EndCall(object[] ps)
    {
        Debug.Log("EndCall");

        callSystemUI.Hide();
        isCalling = false;

        if (currentCallData != null)
        {
            Debug.Log("notNull");

            AddFiles(currentCallData.additionFileIDList);

            if (!string.IsNullOrEmpty(currentCallData.returnCallID))
            {
                if (DataManager.Inst.GetReturnData(currentCallData.returnCallID) == null)
                {
                    Debug.Log("Retrun");
                    CallDataSO returnCallData = ResourceManager.Inst.GetResource<CallDataSO>(currentCallData.returnCallID);
                    if (returnCallData != null)
                        DataManager.Inst.AddReturnCallData(returnCallData.ID, (int)returnCallData.delay);
                }
            }

            currentCallData = null;
        }
    }

    public void StartCallMonolog(object[] ps)
    {
        if (ps.Length != 1 || !(ps[0] is CallDataSO)) return;

        CallDataSO callData = (CallDataSO)ps[0];
        currentCallData = callData;
        CallWindow window = WindowManager.Inst.WindowOpen(EWindowType.CallWindow) as CallWindow;
        window.Setting(callData.id);
    }

    //이제 각자 CallScreen에서 추가해야함
    public void AddFiles(List<AdditionFile> additionFiles)
    {
        if (additionFiles == null) return;

        foreach (AdditionFile additionFile in additionFiles)
        {
            FileManager.Inst.AddFile(additionFile.fileID, additionFile.directoryID);
        }
    }
}
