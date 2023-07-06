using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CallSystem : MonoBehaviour
{
    /// <summary>
    /// string: CallCharacterID
    /// </summary>
    public static Action<string> OnOutGoingCall;

    [SerializeField]
    private CallSystemUI callSystemUI;

    [SerializeField]
    private float deflaultDelayTime = 8f;

    private bool isCalling = false;

    public void Start()
    {
        GameManager.Inst.OnStartCallback += Init;
    }

    private void Init()
    {
        EventManager.StartListening(EMonologEvent.MonologEnd, IncomingCheck);
        EventManager.StartListening(EProfilerEvent.FindInfoInProfiler, IncomingCheck);

        EventManager.StartListening(ECallEvent.ClickSelectBtn, StartMonolog);

        //EventManager.StartListening(EMonologEvent.MonologEnd, DecisionCheck);
        //EventManager.StartListening(EProfilerEvent.FindInfoInProfiler, DecisionCheck);

        GetIncomingData();
    }


    // 얘는 받는 전용
    public void OnAnswerCall(ECharacterDataType characterType, int monologType)
    {
        if (characterType == ECharacterDataType.None) return;
        CharacterInfoDataSO charSO = ResourceManager.Inst.GetCharacterDataSO(characterType);
        SetCallUI(charSO);

        ShowSpectrumUI(false);
        ShowAnswerButton(true);
        ButtonSetting(monologType);

        if (DataManager.Inst.IsSavePhoneNumber(charSO.phoneNum) == false)
        {
            Debug.Log(charSO.characterName);
            //EventManager.TriggerEvent(EProfilerEvent.FindInfoText, new object[2] { EProfilerCategory.InvisibleInformation, charSO.phoneNumberInfoID });
            EventManager.TriggerEvent(ECallEvent.AddAutoCompleteCallBtn, new object[1] { charSO.phoneNum });
        }
        Show();
    }

    // 얘는 결국에는 거는 전용
    public void StartOutGoingCall(string callProfileID)
    {
        if (isCalling) return;

        CallProfileDataSO data = ResourceManager.Inst.GetCallProfileData(callProfileID);
        if (data == null) return;

        callSystemUI.OutGoingCall(data);
    }

    public void StartMonolog(object[] ps)
    {
        if (ps.Length == 0 ||
            !(ps[0] is string) ||
            (ps.Length == 2 && !(ps[1] is List<AdditionFile>))
            ) return;

        string monologID = (string)ps[0];
        List<AdditionFile> additionFiles = (List<AdditionFile>)ps[1];

        MonologSystem.AddOnEndMonologEvent(monologID, callSystemUI.Hide);
        MonologSystem.AddOnEndMonologEvent(monologID, () => AddFiles(additionFiles));

        MonologSystem.OnStartMonolog?.Invoke(monologID, false);
    }

    private void AddFiles(List<AdditionFile> additionFiles)
    {
        foreach (AdditionFile additionFile in additionFiles)
        {
            FileManager.Inst.AddFile(additionFile.fileID, additionFile.directoryID);
        }
    }

    public void SaveReturnMonolog(MonologLockData data)
    {

        if (data == null)
            return;
        foreach (ReturnMonologData returnData in data.returnMonologDataList)
        {
            if (returnData.characterType == ECharacterDataType.None || returnData.MonologID == 0) continue;
            DataManager.Inst.AddReturnData(returnData);
        }

    }


    private void GetIncomingData()
    {
        foreach (var incomingData in ResourceManager.Inst.IncomingCallDataList)
        {
            incomingCallDataList.Add(incomingData.Value);
        }
    }

    private void IncomingCheck(object[] ps)
    {
        if (isCalling) return;

        foreach (var incomingCallData in incomingCallDataList) //캐릭마다
        {
            foreach (ReturnMonologData data in incomingCallData.incomingMonologList) //한 캐릭의 리턴 독백마다
            {
                Debug.Log(data.MonologID);
                if (DataManager.Inst.IsMonologShow(data.MonologID))//이미 본 독백이면
                {
                    continue;//넘어가
                }

                if (Define.MonologLockDecisionFlag(data.decisions))// 조건 확인
                {
                    OnAnswerCall(data.characterType, data.MonologID);// 전화걸리기
                    if (data.additionFiles != null && data.additionFiles.Count > 0)//추가 파일 있으면 추가
                    {
                        MonologSystem.OnEndMonologEvent = () => data.additionFiles.ForEach(x => FileManager.Inst.AddFile((int)x.x, (int)x.y));
                    }
                }
            }
        }
    }






    public void SetEndMonolog(int monologType)
    {
        MonologSystem.OnStopMonolog?.Invoke();
    }
}
