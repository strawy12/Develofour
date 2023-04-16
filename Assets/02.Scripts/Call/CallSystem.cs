using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;
using System.Runtime.InteropServices;

public class CallSystem : MonoSingleton<CallSystem>
{
    [Header("CallUI")]
    public TMP_Text nameText;
    public Image profileIcon;

    public Button answerBtn;
    public AudioSpectrumUI spectrumUI;

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

    // ��� �ᱹ���� �޴� ����
    public void OnAnswerCall(ECharacterDataType characterType, EMonologTextDataType monologType)
    {
        CharacterInfoDataSO charSO = ResourceManager.Inst.GetCharacterDataSO(characterType);
        SetCallUI(charSO);

        ShowSpectrumUI(false);
        ShowAnswerButton(true);
        answerBtn.onClick.AddListener(() => StartMonolog(monologType));

        Show(true);
    }

    // ��� �ᱹ���� �Ŵ� ����
    public void OnRequestCall(CharacterInfoDataSO data)
    {
        SetCallUI(data);

        // ���⼭ Monolog �Ǵ��� �������
        // ���� �� ���� ����, switch Coroutine�� ��õ��
        // ECharacterType ���� �ϰ� �� SO ������ֱ�
        StartCoroutine(StartRequestCall(data.characterType));

        ShowAnswerButton(false);
        ShowSpectrumUI(true);

        Show(false);
    }

    private IEnumerator StartRequestCall(ECharacterDataType characterType)
    {
        float delay = 5f;

        switch (characterType)
        {
            default:
                Debug.Log(delay);
                yield return PlayPhoneCallSound(delay);
                Hide();
                break;
        }
    }

    private IEnumerator PlayPhoneCallSound(float delay)
    {
        Debug.Log(delay);
        while (delay > 0f)
        {
            Debug.Log(delay);
            float soundSecond = (float)Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneCall);

            yield return new WaitForSeconds(soundSecond);
            Debug.Log(delay);
            delay -= soundSecond;
        }
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

    public void StartMonolog(EMonologTextDataType monologType)
    {
        //�������� ���߿� ����
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
        while (true)
        {
            transform.DOShakePosition(2.5f, 5);
            Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneAlarm);
            yield return new WaitForSeconds(4f);
        }
    }

    // ��ȭ�� �޾��� �� ���� 
    public void Hide()
    {
        transform.DOKill(true);
        Sound.OnImmediatelyStop(Sound.EAudioType.PhoneCall);
        EventManager.TriggerEvent(ECoreEvent.CoverPanelSetting, new object[] { false });

        StopAllCoroutines();
        //transform.DOLocalMoveX(1200, 0.5f).SetEase(Ease.Linear);
    }

    public void SetEndMonolog(EMonologTextDataType monologType)
    {
        MonologSystem.OnStopMonolog?.Invoke();
    }
}
