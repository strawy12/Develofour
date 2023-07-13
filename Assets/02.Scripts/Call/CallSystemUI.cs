using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CallSystemUI : MonoBehaviour
{
    [SerializeField]
    private TMP_Text nameText;
    [SerializeField]
    private Image profileIcon;

    [SerializeField]
    private Button answerBtn;
    [SerializeField]
    private AudioSpectrumUI spectrumUI;

    [SerializeField]
    private Transform selectButtonParent;
    [SerializeField]
    private CallSelectButton selectButton;

    private List<CallSelectButton> buttonList;
    private CallProfileDataSO currentCallProfileData;
    private bool isRecieveCall;

    public Action OnClickAnswerBtn;

    public void Show()
    {
        GameManager.Inst.ChangeGameState(EGameState.CutScene);
        transform.DOLocalMoveX(770, 0.5f).SetEase(Ease.Linear);
    }

    public void Hide()
    {
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


    private void InitCallUI()
    {
        CharacterInfoDataSO characterData = ResourceManager.Inst.GetResource<CharacterInfoDataSO>(currentCallProfileData.ID);

        if (characterData.characterName == "")
        {
            nameText.text = characterData.phoneNum;
        }
        else
        {
            nameText.text = characterData.characterName;
        }
        profileIcon.sprite = characterData.profileIcon;
    }

    public void InCommingCall(CallProfileDataSO data)
    {
        currentCallProfileData = data;
        InitCallUI();

        ShowAnswerButton(true);
        ShowSpectrumUI(false);

        isRecieveCall = false;

        Show();
        // AnswerButton 셋팅
        StartCoroutine(PlayPhoneSoundAndShake());

    }

    public void OutGoingCall(CallProfileDataSO data)
    {
        currentCallProfileData = data;
        InitCallUI();

        ShowAnswerButton(false);
        ShowSpectrumUI(true);

        Show();
        StartCoroutine(PlayPhoneCallSound(currentCallProfileData.delay));

    }

    private IEnumerator PlayPhoneCallSound(float delay)
    {
        while (delay > 0f)
        {
            float soundSecond = (float)Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneCall);

            if (delay - soundSecond < 0f)
                yield return new WaitForSeconds(delay - soundSecond);

            else
                yield return new WaitForSeconds(soundSecond);

            delay -= soundSecond;
        }

        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.PhoneCall);
        RecivivedCall();
    }

    private IEnumerator PlayPhoneSoundAndShake()
    {
        yield return new WaitForSeconds(0.8f);
        while (!isRecieveCall)
        {
            transform.DOKill(true);
            transform.DOShakePosition(2.5f, 5);
            float soundSecond = (float)Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneAlarm);
            yield return new WaitForSeconds(soundSecond + Constant.PHONECALLSOUND_DELAY);
        }

        isRecieveCall = false;
    }

    private void SetSelectBtns()
    {
        foreach (string callDataID in currentCallProfileData.outGoingCallIDList)
        {
            CallDataSO callData = ResourceManager.Inst.GetResource<CallDataSO>(callDataID);
            if (callData == null) continue;

            if (!DataManager.Inst.IsMonologShow(callData.monologID))
            {
                if (Define.NeedInfoFlag(callData.needInfoIDList))
                {
                    MakeSelectBtn(callData);
                }
            }
        }

        CallDataSO notExistCallData = ResourceManager.Inst.GetResource<CallDataSO>(currentCallProfileData.notExistCallID);
        MakeSelectBtn(notExistCallData);
    }
    private void MakeSelectBtn(CallDataSO callData)
    {
        if (callData == null) return;

        CallSelectButton instance = Instantiate(selectButton, selectButton.transform.parent);
        MonologTextDataSO textData = ResourceManager.Inst.GetResource<MonologTextDataSO>(callData.monologID);
        buttonList.Add(instance);

        //instance.btnText.text = textData.monologName;
        instance.btn.onClick.AddListener(() =>
        {
            HideSelectBtns();
            EventManager.TriggerEvent(ECallEvent.ClickSelectBtn, new object[] { callData });
        });
        instance.gameObject.SetActive(true);
    }

    private void RecivivedCall()
    {
        MonologTextDataSO textData = ResourceManager.Inst.GetResource<MonologTextDataSO>(currentCallProfileData.monologID);
        MonologSystem.AddOnEndMonologEvent(textData.ID, SetSelectBtns);
        MonologSystem.OnStartMonolog?.Invoke(textData.ID, false);
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
        OnClickAnswerBtn?.Invoke();

        // 추후 문제가 생길 경우 변경을 시켜야한다
        OnClickAnswerBtn = null;
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

}
