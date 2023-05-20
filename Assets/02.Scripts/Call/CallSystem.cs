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
    private List<CallSelectButton> buttonList = new List<CallSelectButton>();
    //Call Stack(전화 쌓이는거) 저장 해야함.

    [Header("CallUI")]
    public TMP_Text nameText;
    public Image profileIcon;

    public Button answerBtn;
    public AudioSpectrumUI spectrumUI;

    public bool isRecieveCall;

    public Transform selectButtonParent;
    public CallSelectButton selectButton;

    [SerializeField]
    private float deflaultDelayTime = 10f;

    public void Start()
    {
        GameManager.Inst.OnStartCallback += Init;
    }

    private IEnumerator RepeatCheckReturnCall()
    {
        yield break;
        while (true)
        {
            yield return new WaitForSeconds(deflaultDelayTime);

            Debug.Log("checkDelayEnd");
            DecisionCheck();
        }
    }

    private void Init()
    {
        answerBtn.gameObject.SetActive(false);
        spectrumUI.gameObject.SetActive(false);

        EventManager.StartListening(EMonologEvent.MonologEnd, DecisionCheck);
        EventManager.StartListening(EProfileEvent.FindInfoInProfile, DecisionCheck);

        spectrumUI.Init();

        StartCoroutine(RepeatCheckReturnCall());
    }

    public void DecisionCheck(object[] ps = null)
    {
        List<ReturnMonologData> list = DataManager.Inst.GetReturnDataList();

        foreach (ReturnMonologData data in list)
        {
            Debug.Log($"{data.EndDelayTime}_{DataManager.Inst.GetCurrentTime()}");

            if (data.EndDelayTime < DataManager.Inst.GetCurrentTime())
                continue;

            if (Define.MonologLockDecisionFlag(data.decisions))
            {
                OnAnswerCall(data.characterType, data.MonologID);
            }
        }
    }

    // 얘는 결국에는 받는 전용
    public void OnAnswerCall(ECharacterDataType characterType, int monologType)
    {
        if (characterType == ECharacterDataType.None) return;
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
        if (callData != null && Define.MonologLockDecisionFlag(callData.defaultDecisions))
        {
            MonologSystem.OnEndMonologEvent = () => SetMonologSelector(callData);
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
        int spawnCnt = 0;
        for (int i = 0; i < callData.monologLockList.Count; i++)
        {
            if (!Define.MonologLockDecisionFlag(callData.monologLockList[i].decisions)) continue;

            int num = i;
            MonologLockData lockData = callData.monologLockList[i];
            lockData.returnMonologData.characterType = callData.characterType;

            CallSelectButton instance = Instantiate(selectButton, selectButton.transform.parent);
            MonologTextDataSO textData = ResourceManager.Inst.GetMonologTextData(lockData.monologID);
            buttonList.Add(instance);

            instance.btnText.text = textData.monologName;
            instance.btn.onClick.AddListener(() =>
            {
                HideSelectBtns();
                StartMonolog(textData.TextDataType, lockData);
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

    public void StartMonolog(int monologType, MonologLockData data = null)
    {
        //저장쪽은 나중에 생각
        // 딜레이 후 해당 독백이 실행되는 작업 해야함

        Debug.Log("Hide");
        MonologSystem.OnEndMonologEvent = Hide;
        //MonologSystem.OnEndMonologEvent += () => SaveReturnMonolog(data);

        MonologSystem.OnStartMonolog?.Invoke(monologType, 0, false);
    }

    public void SaveReturnMonolog(MonologLockData data)
    {
        if (data == null)
            return;

        if (data.returnMonologData.characterType == ECharacterDataType.None || data.returnMonologData.MonologID == 0) return;
        DataManager.Inst.AddReturnData(data.returnMonologData);
    }

    public void Show(bool isShake)
    {
        if (isShake)
        {
            StartCoroutine(PhoneSoundCor());
        }
        GameManager.Inst.ChangeGameState(EGameState.CutScene);
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


    public void Hide()
    {
        Debug.Log("Hid2e");
        transform.DOKill(true);
        Sound.OnImmediatelyStop(Sound.EAudioType.PhoneCall);
        GameManager.Inst.ChangeGameState(EGameState.Game);

        StopAllCoroutines();
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
