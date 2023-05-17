using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;
using System.Runtime.InteropServices;

//class StackMonolog
//{
//    public int priority;
//    public int monologType;
//    public string monologName;
//}

public class CallSystem : MonoSingleton<CallSystem>
{

    //Call Stack(전화 쌓이는거) 저장 해야함.

    [Header("CallUI")]
    public TMP_Text nameText;
    public Image profileIcon;

    public Button answerBtn;
    public AudioSpectrumUI spectrumUI;

    public bool isRecieveCall;

    public CallSelectButton selectButton;

    public int requestLogID;
    public float requestDelay;

    public void Start()
    {
        answerBtn.gameObject.SetActive(false);
        spectrumUI.gameObject.SetActive(false);

        Init();
    }

    private void Init()
    {
        spectrumUI.Init();
    }

    // 얘는 결국에는 받는 전용
    public void OnAnswerCall(ECharacterDataType characterType, int monologType)
    {
        CharacterInfoDataSO charSO = ResourceManager.Inst.GetCharacterDataSO(characterType);
        SetCallUI(charSO);

        ShowSpectrumUI(false);
        ShowAnswerButton(true);
        ButtonSetting(monologType);

        Show(true);
    }

    // 얘는 결국에는 거는 전용
    public void OnRequestCall(CharacterInfoDataSO data)
    {
        SetCallUI(data);
        // 해당 캐릭터가 받는 독백이 존재한지 체크

        RequestCallDataSO callData = ResourceManager.Inst.GetRequestCallData(data.characterType);

        int result = -1;
        if (callData!=null && Define.MonologLockDecisionFlag(callData.defaultDecisions))
        {
            MonologSystem.OnEndMonologEvent += () => SetMonologSelector(callData);
            result = callData.defaultMonologID;
        }

        StartCoroutine(StartRequestCall(result));

        ShowAnswerButton(false);
        ShowSpectrumUI(true);

        Show(false);
    }

    // 선택지 UI 생성해주는 코드
    public void SetMonologSelector(RequestCallDataSO callData)
    {
        // 기존에 존재하던 선택지UI들 지우기
        // TODO
        // 추후 풀링으로 변경할 예정 
        Transform[] childList = selectButton.transform.parent.GetComponentsInChildren<Transform>();

        if (childList != null)
        {
            for (int i = 1; i < childList.Length; i++)
            {
                if (childList[i] != transform)
                    Destroy(childList[i].gameObject);
            }
        }

        int spawnCnt = 0;
        for (int i = 0; i < callData.monologLockList.Count; i++)
        {
            if (!Define.MonologLockDecisionFlag(callData.monologLockList[i].decisions)) continue;

            int num = i;
            MonologLockData lockData = callData.monologLockList[i];
            CallSelectButton instance = Instantiate(selectButton, selectButton.transform.parent);
            MonologTextDataSO textData = ResourceManager.Inst.GetMonologTextData(lockData.monologID);

            instance.btnText.text = textData.monologName;
            instance.btn.onClick.AddListener(() =>
            {
                StartMonolog(textData.TextDataType, callData.characterType,lockData.answerMonologID, lockData.answerDelay);
            });
            instance.gameObject.SetActive(true);

            spawnCnt++;
        }

        if (spawnCnt <= 0)
        {
            StartMonolog(callData.notExistMonoLogID);
        }
    }

    private void ButtonSetting(int data)
    {
        answerBtn.onClick.AddListener(() => StartMonolog(data));
    }

    private IEnumerator StartRequestCall(int monologType)
    {
        float delay = 5f;
        yield return PlayPhoneCallSound(delay);
        if (monologType != -1)
        {
            //MonologSystem.OnEndMonologEvent += Hide;
            MonologSystem.OnStartMonolog?.Invoke(monologType, 0, true);
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
            //answerBtn.onClick.AddListener(Hide);
            answerBtn.onClick.AddListener(() => ShowSpectrumUI(true));
            answerBtn.onClick.AddListener(() => ShowAnswerButton(false));
            answerBtn.onClick.AddListener(() => isRecieveCall = true);
        }

        answerBtn.gameObject.SetActive(isShow);
    }

    private void ShowSpectrumUI(bool isShow)
    {
        spectrumUI.gameObject.SetActive(true);

        if (isShow)
        {
            spectrumUI.StartSpectrum();
        }
        else
        {
            spectrumUI.StopSpectrum();
        }
    }

    private void SetCallUI(CharacterInfoDataSO data)
    {
        if (data.characterName == "")
        {
            nameText.text = data.phoneNum;
        }
        else
        {
            nameText.text = data.characterName;
        }
        profileIcon.sprite = data.profileIcon;
    }

    public void StartMonolog(int monologType, ECharacterDataType type = ECharacterDataType.None, int afterMonologId = -1, float delay = 0f)
    {
        //저장쪽은 나중에 생각
        // 딜레이 후 해당 독백이 실행되는 작업 해야함

        requestLogID = afterMonologId;
        requestDelay = delay;
        
        MonologSystem.OnEndMonologEvent += Hide;
        MonologSystem.OnEndMonologEvent += () => DelayAnswerCall(type, afterMonologId, afterMonologId, delay);

        MonologSystem.OnStartMonolog.Invoke(monologType, 0, false);
    }

    public void Show(bool isShake)
    {
        if (isShake)
        {
            StartCoroutine(PhoneSoundCor());
        }
        EventManager.TriggerEvent(ECoreEvent.CoverPanelSetting, new object[] { true });
        transform.DOLocalMoveX(770, 0.5f).SetEase(Ease.Linear);
    }

    private IEnumerator PhoneSoundCor()
    {
        yield return new WaitForSeconds(0.8f);
        while (!isRecieveCall)
        {
            transform.DOShakePosition(2.5f, 5);
            Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneAlarm);
            yield return new WaitForSeconds(4f);
        }
        isRecieveCall = false;
    }

    public void DelayAnswerCall(ECharacterDataType type, int monologID, float delay)
    {
        StartCoroutine(DelayAnswerCallCo(type, monologID, delay));
    }

    private IEnumerator DelayAnswerCallCo(ECharacterDataType type, int monologID, float delay)
    {
        yield return new WaitForSeconds(delay);
        OnAnswerCall(type, monologID);
    }

    // 전화를 받았을 때 시작 
    public void Hide()
    {
        transform.DOKill(true);
        Sound.OnImmediatelyStop(Sound.EAudioType.PhoneCall);
        EventManager.TriggerEvent(ECoreEvent.CoverPanelSetting, new object[] { false });

        StopAllCoroutines();
        transform.DOLocalMoveX(1200, 0.5f).SetEase(Ease.Linear);
        spectrumUI.StopSpectrum();
    }

    public void SetEndMonolog(int monologType)
    {
        MonologSystem.OnStopMonolog?.Invoke();
    }
}
