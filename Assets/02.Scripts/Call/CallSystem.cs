using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

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

    private bool isCalling = false;

    public void Start()
    {
        GameManager.Inst.OnStartCallback += Init;
    }

    private IEnumerator RepeatCheckReturnCall()
    {
        while (true)
        {
            yield return new WaitForSeconds(deflaultDelayTime);
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
        if (isCalling) return;
        List<ReturnMonologData> list = DataManager.Inst.GetReturnDataList();
        List<ReturnMonologData> temp = new List<ReturnMonologData>();

        foreach (ReturnMonologData data in list)
        {
            Debug.Log($"{data.EndDelayTime}_{DataManager.Inst.GetCurrentTime()}");

            if (data.EndDelayTime > DataManager.Inst.GetCurrentTime())
                continue;

            if (Define.MonologLockDecisionFlag(data.decisions))
            {
                Debug.Log(data.characterType);
                OnAnswerCall(data.characterType, data.MonologID);
                if (data.additionFiles != null && data.additionFiles.Count > 0)
                {
                    Debug.LogError("임시방편임");
                    MonologSystem.OnEndMonologEvent = () => data.additionFiles.ForEach(x => FileManager.Inst.AddFile(x, Constant.FileID.USB));
                }
                temp.Add(data);
            }
        }

        temp.ForEach(x => DataManager.Inst.RemoveReturnData(x));
    }

    // 얘는 결국에는 받는 전용
    public void OnAnswerCall(ECharacterDataType characterType, int monologType)
    {
        if (characterType == ECharacterDataType.None) return;
        CharacterInfoDataSO charSO = ResourceManager.Inst.GetCharacterDataSO(characterType);
        SetCallUI(charSO);

        StartCoroutine(PhoneSoundCor());

        ShowSpectrumUI(false);
        ShowAnswerButton(true);
        ButtonSetting(monologType);

        if(DataManager.Inst.IsSavePhoneNumber(charSO.phoneNum) == false)
        {
            EventManager.TriggerEvent(EProfileEvent.FindInfoText, new object[2] { EProfileCategory.InvisibleInformation, charSO.profileInfoID });
            EventManager.TriggerEvent(ECallEvent.AddAutoCompleteCallBtn, new object[1] { charSO.phoneNum });
        }
        Show();
    }

    // 얘는 결국에는 거는 전용
    public void OnRequestCall(CharacterInfoDataSO data)
    {
        SetCallUI(data);
        int result = -1;

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

            if (lockData.returnMonologData.characterType == ECharacterDataType.None)
            {
                lockData.returnMonologData.characterType = callData.characterType;
            }

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
        if (isCalling)
        {
            yield return new WaitUntil(() => !isCalling);
            yield return new WaitForSeconds(5f);
        }

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
        MonologSystem.OnEndMonologEvent = () => SaveReturnMonolog(data);

        MonologSystem.OnStartMonolog?.Invoke(monologType, 0, false);
    }

    public void SaveReturnMonolog(MonologLockData data)
    {
        if (data == null)
            return;

        if (data.returnMonologData.characterType == ECharacterDataType.None || data.returnMonologData.MonologID == 0) return;
        DataManager.Inst.AddReturnData(data.returnMonologData);
    }

    public void Show()
    {
        isCalling = true;
        GameManager.Inst.ChangeGameState(EGameState.CutScene);
        transform.DOLocalMoveX(770, 0.5f).SetEase(Ease.Linear);
    }

    private IEnumerator PhoneSoundCor()
    {
        if (isCalling)
        {
            yield return new WaitUntil(() => !isCalling);
            yield return new WaitForSeconds(5f);
        }
        yield return new WaitForSeconds(0.8f);
        while (!isRecieveCall)
        {
            transform.DOShakePosition(2.5f, 5);
            Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneAlarm);
            yield return new WaitForSeconds(4f);
        }
        transform.DOKill(true);
        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.PhoneAlarm);
        isRecieveCall = false;
    }


    public void Hide()
    {
        isCalling = false;
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
