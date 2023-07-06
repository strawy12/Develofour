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
    public void OnOutGoingCall(string callCharacterID)
    {
        SetCallUI(data);
        int result = -1;

        // 한번 선택한 선택지는 다시는 안 나오게 만들기 위해 쓴 코드
        if (!DataManager.Inst.IsExistReturnData(data.characterType))
        {
            RequestCallDataSO callData = ResourceManager.Inst.GetRequestCallData(data.characterType);
            if (callData != null && Define.MonologLockDecisionFlag(callData.defaultDecisions))
            {
                MonologSystem.OnEndMonologEvent = () => SetMonologSelector(callData);
                result = callData.defaultMonologID;
            }
        }

        StartCoroutine(StartRequestCall(result));

        ShowAnswerButton(false);
        ShowSpectrumUI(true);

        Show();
    }

    // 선택지 UI 생성해주는 코드
    public void SetMonologSelector(RequestCallDataSO callData)
    {
        // 기존에 존재하던 선택지UI들 지우기
        // TODO
        // 추후 풀링으로 변경할 예정 
        int spawnCnt = 0;
        
        for (int i = 0; i < callData.monologLockList.Count; i++)
        {
            if (!Define.MonologLockDecisionFlag(callData.monologLockList[i].decisions)) continue;

            int num = i;
            MonologLockData lockData = callData.monologLockList[i];

            foreach (ReturnMonologData data in lockData.returnMonologDataList)
            {
                if (data.characterType == ECharacterDataType.None)
                {
                    data.characterType = callData.characterType;
                }
            }

            MakeCallTextDataBtn(lockData.monologID, lockData);
            spawnCnt++;
        }
        Debug.Log(spawnCnt);
        if (spawnCnt <= 0)
        {
            StartMonolog(callData.notExistMonoLogID);
            return;
        }
        MakeCallTextDataBtn(callData.notExistMonoLogID);
     
    }
    private void MakeCallTextDataBtn(int monologID, MonologLockData lockData = null)
    {
        CallSelectButton instance = Instantiate(selectButton, selectButton.transform.parent);
        MonologTextDataSO textData = ResourceManager.Inst.GetMonologTextData(monologID);
        buttonList.Add(instance);

        instance.btnText.text = textData.monologName;
        instance.btn.onClick.AddListener(() =>
        {
            HideSelectBtns();
            StartMonolog(textData.TextDataType, lockData);
        });
        instance.gameObject.SetActive(true);
    }
    private void ButtonSetting(int data)
    {
        answerBtn.onClick.AddListener(() => StartMonolog(data));
    }

    private IEnumerator StartRequestCall(int monologType)
    {
        if (isCalling)
        {
            yield return new WaitUntil(() => !isCalling);
            yield return new WaitForSeconds(5f);
        }

        float delay = 5f;
        yield return PlayPhoneCallSound(delay);
        if (monologType != -1)
        {
            MonologSystem.OnStartMonolog?.Invoke(monologType, 0, false);
        }
        else
        {
            Hide();
        }
    }

    private IEnumerator PlayPhoneCallSound(float delay)
    {
        while (delay > 0f)
        {
            float soundSecond = (float)Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneCall);

            yield return new WaitForSeconds(soundSecond);
            delay -= soundSecond;
        }
    }

    private void ShowAnswerButton(bool isShow)
    {
        if (isShow)
        {
            answerBtn.onClick.RemoveAllListeners();
            answerBtn.onClick.AddListener(ClickAnswerBtn);
        }

        answerBtn.gameObject.SetActive(isShow);
    }

    private void ClickAnswerBtn()
    {
        ShowSpectrumUI(true);
        ShowAnswerButton(false);
        isRecieveCall = true;

        transform.DOKill(true);
        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.PhoneAlarm);
    }

    private void ShowSpectrumUI(bool isShow)
    {
        spectrumUI.gameObject.SetActive(isShow);

        if (isShow)
        {
            spectrumUI.StartSpectrum();
        }
        else
        {
            spectrumUI.StopSpectrum();
        }
    }

    private void SetCoverPanel(bool value)
    {
        callCoverPanel.gameObject.SetActive(value);
    }



    public void StartMonolog(int monologType, MonologLockData data = null)
    {
        //저장쪽은 나중에 생각
        // 딜레이 후 해당 독백이 실행되는 작업 해야함

        Debug.Log($"monologType = {monologType}");
        MonologSystem.OnEndMonologEvent = Hide;

        MonologSystem.OnEndMonologEvent = () => DataManager.Inst.SetMonologShow(monologType, true);
        MonologSystem.OnEndMonologEvent = () => SaveReturnMonolog(data);
        MonologSystem.OnEndMonologEvent = () =>
        {
            if (data == null) return;
            if (data.additionFiles.Count != 0)
            {
                data.additionFiles.ForEach(x => data.additionFiles.ForEach(x => FileManager.Inst.AddFile((int)x.x, (int)x.y)));
            }
        };

        MonologSystem.OnStartMonolog?.Invoke(monologType, 0, false);
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


    public void Show()
    {
        isCalling = true;
        SetCoverPanel(true);
        GameManager.Inst.ChangeGameState(EGameState.CutScene);
        transform.DOLocalMoveX(770, 0.5f).SetEase(Ease.Linear);
    }



    public void Hide()
    {
        isCalling = false;
        SetCoverPanel(false);
        transform.DOKill(true);
        Sound.OnImmediatelyStop(Sound.EAudioType.PhoneCall);
        GameManager.Inst.ChangeGameState(EGameState.Game);

        StopAllCoroutines();
        StartCoroutine(RepeatCheckReturnCall());
        transform.DOLocalMoveX(1200, 0.5f).SetEase(Ease.Linear);
        spectrumUI.StopSpectrum();

        HideSelectBtns();
    }

    private void HideSelectBtns()
    {
        buttonList.ForEach(x => Destroy(x.gameObject));
        buttonList.Clear();
    }

    public void SetEndMonolog(int monologType)
    {
        MonologSystem.OnStopMonolog?.Invoke();
    }
}
