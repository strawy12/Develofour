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

    [SerializeField]
    private GetCallMonologDataSO getCallData;

    public CallSelectButton selectButton;

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

        GetCallMonolog monolog = getCallData.GetCallMonologList.Find(x => x.charData == data.characterType);
        int result = -1;
        if (monolog != null)
        {
            result = monolog.textData;
        }

        //만약 해당 캐릭터의 독백이 존재하면?
        if (result != -1)
        {
            //끝나는 시점에 선택지 리스트
            MonologSystem.OnEndMonologEvent += () => SetMonologSelector(data.characterType);
        }

        StartCoroutine(StartRequestCall(result));

        // 여기서 Monolog 판단을 해줘야함
        // 몇초 뒤 받을 건지, switch Coroutine을 추천함
        // ECharacterType 으로 하고 다 SO 만들어주기

        ShowAnswerButton(false);
        ShowSpectrumUI(true);

        Show(false); 
    }

    // 선택지 UI 생성해주는 코드
    public void SetMonologSelector(ECharacterDataType type)
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

        RequestCallDataSO data = ResourceManager.Inst.GetRequestCallData(type);
        if(data == null) 
        {
            Debug.LogError($"RequestCallDataSO is Null! {type}");   
        }

        if (!Define.MonologLockDecisionFlag(data.defaultDecisions)) return;

        for (int i = 0; i < data.monologLockList.Count; i++)
        {
            if (!Define.MonologLockDecisionFlag(data.monologLockList[i].decisions)) continue;

            int num = i;
            MonologLockData lockData = data.monologLockList[i];
            CallSelectButton instance = Instantiate(selectButton, selectButton.transform.parent);
            MonologTextDataSO textData = ResourceManager.Inst.GetMonologTextData(lockData.monologID);

            instance.btnText.text = textData.monologName;
            instance.btn.onClick.AddListener(() =>
            {
                StartMonolog(textData.TextDataType, lockData.answerMonologID, lockData.answerDelay);
            });
            instance.gameObject.SetActive(true);
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

    public void StartMonolog(int monologType, int afterMonologId = -1, float delay = 0f)
    {
        //저장쪽은 나중에 생각

        // 딜레이 후 해당 독백이 실행되는 작업 해야함
        MonologSystem.OnEndMonologEvent += Hide;
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

    // 전화를 받았을 때 시작 
    public void Hide()
    {
        MonologSystem.OnEndMonologEvent -= Hide;
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
