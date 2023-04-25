using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Linq;
using System.Runtime.InteropServices;

class StackMonolog
{
    public int priority;
    public int monologType;
}

public class CallSystem : MonoSingleton<CallSystem>
{

    [Header("CallUI")]
    public TMP_Text nameText;
    public Image profileIcon;

    public Button answerBtn;
    public AudioSpectrumUI spectrumUI;

    public bool isRecieveCall;

    private Dictionary<ECharacterDataType, List<StackMonolog>> characterStackList = new Dictionary<ECharacterDataType, List<StackMonolog>>();

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
        if (characterStackList.ContainsKey(data.characterType) && characterStackList[data.characterType].Count != 0)
        {
            StartCoroutine(StartRequestCall(characterStackList[data.characterType][0].monologType));
            characterStackList[data.characterType].RemoveAt(0);
        }
        else
        {
            StartCoroutine(StartRequestCall(-1));
        }

        // 여기서 Monolog 판단을 해줘야함
        // 몇초 뒤 받을 건지, switch Coroutine을 추천함
        // ECharacterType 으로 하고 다 SO 만들어주기

        ShowAnswerButton(false);
        ShowSpectrumUI(true);

        Show(false);
    }

    private void ButtonSetting(int data)
    {
        answerBtn.onClick.AddListener(() => StartMonolog(data));
    }

    public void StackMonolog(ECharacterDataType data, MonologTextDataSO monologType)
    {
        StackMonolog stackMonolog = new StackMonolog() { monologType = monologType.TextDataType, priority = monologType.CallPriority };
        if (!characterStackList.ContainsKey(data))
        {
            characterStackList.Add(data, new List<StackMonolog>());
        }
        if (!characterStackList[data].Contains(stackMonolog))
        {
            characterStackList[data].Add(stackMonolog);
        }

        var monolog = characterStackList[data].OrderBy(x => x.priority).First();
    }

    private IEnumerator StartRequestCall(int characterType)
    {
        float delay = 5f;
        yield return PlayPhoneCallSound(delay);
        if(characterType != -1)
        {
            MonologSystem.OnEndMonologEvent += Hide;
            MonologSystem.OnStartMonolog(characterType, 0, true);
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

    public void StartMonolog(int monologType)
    {
        //저장쪽은 나중에 생각
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
