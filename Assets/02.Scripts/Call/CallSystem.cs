using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CallSystem : MonoSingleton<CallSystem>
{
    public Action<ECharacterDataType, EMonologTextDataType> OnPlayCallSystem;

    [Header("CallUI")]
    public TMP_Text nameText;
    public Image profileIcon;

    public Button answerBtn;
    public AudioSpectrumUI spectrumUI;

    private IEnumerator coroutine;

    public void Start()
    {
        spectrumUI.Init();
        OnPlayCallSystem += OnStartCall;
    }

    public void OnStartCall(ECharacterDataType characterType, EMonologTextDataType monologType)
    {
        CharacterInfoDataSO charSO = ResourceManager.Inst.GetCharacterDataSO(characterType);

        SetCallUI(charSO);

        SetEndMonolog(characterType);
        answerBtn.onClick.RemoveAllListeners();
        answerBtn.onClick.AddListener(delegate { StartMonolog(monologType); });
        answerBtn.onClick.AddListener(Hide);

        Show();
    }

    private void SetCallUI(CharacterInfoDataSO charSO)
    {
        if(charSO.name == "")
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

    public void Show()
    {
        coroutine = PhoneSoundCor();
        StartCoroutine(coroutine);
        EventManager.TriggerEvent(ECoreEvent.CoverPanelSetting, new object[] { true });
        transform.DOLocalMoveX(770, 0.5f).SetEase(Ease.Linear);
    }

    private IEnumerator PhoneSoundCor()
    {
        yield return new WaitForSeconds(0.8f);
        while(true)
        {
            transform.DOShakePosition(2.5f, 5);
            Sound.OnPlaySound(Sound.EAudioType.PhoneCall);
            yield return new WaitForSeconds(4f);
        }
    }

    public void Hide()
    {
        transform.DOKill(true);
        Sound.OnImmediatelyStop(Sound.EAudioType.PhoneCall);
        EventManager.TriggerEvent(ECoreEvent.CoverPanelSetting, new object[] { false });
        StopCoroutine(coroutine);
        transform.DOLocalMoveX(1200, 0.5f).SetEase(Ease.Linear);
    }

    public void SetEndMonolog(ECharacterDataType charType)
    {
        switch(charType)
        {
            //여기에서 EndMonolog 해줘
        }
    }
}
