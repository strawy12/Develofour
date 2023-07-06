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

    private bool isRecieveCall;

    [SerializeField]
    private Transform selectButtonParent;
    [SerializeField]
    private CallSelectButton selectButton;

    [SerializeField]
    private GameObject callCoverPanel;

    private List<CallSelectButton> buttonList;
    private bool isCalling;

    private CallProfileDataSO currentCallProfileData;

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
        CharacterInfoDataSO characterData = ResourceManager.Inst.GetCharacterDataSO(currentCallProfileData.CharacterID);
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

    public void OutGoingCall(CallProfileDataSO data)
    {
        currentCallProfileData = data;
        InitCallUI();

        ShowAnswerButton(false);
        ShowSpectrumUI(true);

        StartCoroutine(PlayPhoneCallSound(currentCallProfileData.delay));


        Show();
    }

    private void SetSelectBtns()
    {
        int spawnCnt = 0;
        foreach (string callDataID in currentCallProfileData.outGoingCallIDList)
        {
            CallDataSO callData = ResourceManager.Inst.GetCallData(callDataID);
            if (callData == null) continue;

            if (!DataManager.Inst.IsMonologShow(callData.monologID))
            {
                if (Define.NeedInfoFlag(callData.needInfoIDList))
                {
                    spawnCnt++;
                    MonologSystem.AddOnEndMonologEvent(callData.monologID, () => MakeSelectBtn(callData));
                }
            }
        }

        if(spawnCnt == 0)
        {
            MonologTextDataSO textData = ResourceManager.Inst.GetMonologTextData(currentCallProfileData.notExistMonologID);
            EventManager.TriggerEvent(ECallEvent.ClickSelectBtn, new object[] { textData });
        }
    }


    private void MakeSelectBtn(CallDataSO callData)
    {
        CallSelectButton instance = Instantiate(selectButton, selectButton.transform.parent);
        MonologTextDataSO textData = ResourceManager.Inst.GetMonologTextData(callData.monologID);
        buttonList.Add(instance);

        instance.btnText.text = textData.monologName;
        instance.btn.onClick.AddListener(() =>
        {
            HideSelectBtns();
            EventManager.TriggerEvent(ECallEvent.ClickSelectBtn, new object[] { textData, callData.additionFileIDList });
        });
        instance.gameObject.SetActive(true);
    }

    private IEnumerator PlayPhoneCallSound(float delay)
    {
        while (delay > 0f)
        {
            float soundSecond = (float)Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneCall);

            yield return new WaitForSeconds(soundSecond);
            delay -= soundSecond;
        }

        MonologTextDataSO textData = ResourceManager.Inst.GetMonologTextData(currentCallProfileData.monologID);
        MonologSystem.AddOnEndMonologEvent(textData.ID, SetSelectBtns);
        EventManager.TriggerEvent(ECallEvent.ClickSelectBtn, new object[] { textData });
    }


    private IEnumerator PhoneSoundCo()
    {
        if (isCalling)
        {
            yield return new WaitUntil(() => !isCalling);
            yield return new WaitForSeconds(5f);
        }
        yield return new WaitForSeconds(0.8f);
        while (!isRecieveCall)
        {
            transform.DOKill(true);
            transform.DOShakePosition(2.5f, 5);
            Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneAlarm);
            yield return new WaitForSeconds(4f);
        }

        isRecieveCall = false;
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


}
