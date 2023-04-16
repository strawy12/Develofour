using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CallSystem : MonoBehaviour
{
    public static Action<ECharacterDataType, EMonologTextDataType, bool> OnPlayCallSystem;

    [Header("CallUI")]
    public TMP_Text nameText;
    public Image profileIcon;

    public Button answerBtn;
    public AudioSpectrumUI spectrumUI;

    private IEnumerator coroutine;

    public void Start()
    {
        answerBtn.gameObject.SetActive(false);
        spectrumUI.gameObject.SetActive(false);

        Init();
    }

    private void Init()
    {
        spectrumUI.Init();

        OnPlayCallSystem += OnStartCall;
    }

    public void OnStartCall(ECharacterDataType characterType, EMonologTextDataType monologType, bool isAnswer)
    {
        CharacterInfoDataSO charSO = ResourceManager.Inst.GetCharacterDataSO(characterType);

        SetCallUI(charSO);

        SetEndMonolog(characterType);

        if (isAnswer)
        {
            ShowAnswerButton(true);
            answerBtn.onClick.AddListener(() => StartMonolog(monologType));
        }
        else
        {
            ShowSpectrumUI(true);
        }

        Show(isAnswer);
    }

    private void ShowAnswerButton(bool isShow)
    {
        if (isShow)
        {
            answerBtn.onClick.RemoveAllListeners();
            answerBtn.onClick.AddListener(Hide);
            answerBtn.onClick.AddListener(() => ShowSpectrumUI(true));
            answerBtn.onClick.AddListener(() => ShowAnswerButton(false));
        }

        answerBtn.gameObject.SetActive(isShow);
    }

    private void ShowSpectrumUI(bool isShow)
    {
        spectrumUI.gameObject.SetActive(true);

        if(isShow)
        {
            spectrumUI.StartSpectrum();
        }
        else
        {
            spectrumUI.StopSpectrum();
        }
    }

    private void SetCallUI(CharacterInfoDataSO charSO)
    {
        if (charSO.name == "")
        {
            nameText.text = charSO.phoneNum;
        }
        else
        {
            nameText.text = charSO.name;
        }
        profileIcon.sprite = charSO.profileIcon;
    }

    public void StartMonolog(EMonologTextDataType monologType)
    {
        //저장쪽은 나중에 생각
        MonologSystem.OnStartMonolog.Invoke(monologType, 0, false);
    }

    public void Show(bool isShake)
    {
        if(isShake)
        {
            coroutine = PhoneSoundCor();
            StartCoroutine(coroutine);
        }
        EventManager.TriggerEvent(ECoreEvent.CoverPanelSetting, new object[] { true });
        transform.DOLocalMoveX(770, 0.5f).SetEase(Ease.Linear);
    }

    private IEnumerator PhoneSoundCor()
    {
        yield return new WaitForSeconds(0.8f);
        while (true)
        {
            transform.DOShakePosition(2.5f, 5);
            Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneCall);
            yield return new WaitForSeconds(4f);
        }
    }

    // 전화를 받았을 때 시작 
    public void Hide()
    {
        transform.DOKill(true);
        Sound.OnImmediatelyStop(Sound.EAudioType.PhoneCall);
        EventManager.TriggerEvent(ECoreEvent.CoverPanelSetting, new object[] { false });
        StopCoroutine(coroutine);
        //transform.DOLocalMoveX(1200, 0.5f).SetEase(Ease.Linear);
    }

    public void SetEndMonolog(ECharacterDataType charType)
    {
        switch (charType)
        {
            //여기에서 EndMonolog 해줘
        }
    }
}
