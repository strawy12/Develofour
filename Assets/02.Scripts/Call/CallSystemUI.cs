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

    private List<CallSelectButton> buttonList = new List<CallSelectButton>();
    private CallProfileDataSO currentCallProfileData;
    private bool isRecieveCall;

    public Action OnClickAnswerBtn;

    public void Init()
    {
        
    }

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

        HideSelectBtns();
    }

    private void HideSelectBtns()
    {
        if (buttonList == null) return;

        buttonList.ForEach(x =>
        {
            if(x.gameObject != null)
            Destroy(x.gameObject);
        });

        buttonList.Clear();
    }


    private void InitCallUI()
    {
        CharacterInfoDataSO characterData = ResourceManager.Inst.GetResource<CharacterInfoDataSO>(currentCallProfileData.ID);

        if (string.IsNullOrEmpty(characterData.characterName))
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

    //private void SetSelectBtns()
    //{
    //    foreach (CallOption callOption in currentCallProfileData.outGoingCallOptionList)
    //    {
    //        CallDataSO callData = ResourceManager.Inst.GetResource<CallDataSO>(callOption.outGoingCallID);
    //        if (callData == null) continue;

    //        if (!DataManager.Inst.IsMonologShow(callData.monologID))
    //        {
    //            if (Define.NeedInfoFlag(callData.needInfoIDList))
    //            {
    //                MakeSelectBtn(callData, callOption.decisionName);
    //            }
    //        }
    //    }

    //    CallDataSO notExistCallData = new CallDataSO();
    //    notExistCallData.monologID = currentCallProfileData.notExistCallID;
    //    MakeSelectBtn(notExistCallData, "통화 종료");
    //}
    private void MakeSelectBtn(CallDataSO callData, string btnText)
    {
        if (callData == null) return;

        //CallSelectButton instance = Instantiate(selectButton, selectButton.transform.parent);
        //instance.btnText.text = btnText;
        //buttonList.Add(instance);

        ////instance.btnText.text = textData.monologName;s
        //instance.btn.onClick.AddListener(() =>
        //{
        //    HideSelectBtns();
        //    EventManager.TriggerEvent(ECallEvent.ClickSelectBtn, new object[] { callData });
        //});
        //instance.gameObject.SetActive(true);
    }

    private void RecivivedCall()
    {
        MonologTextDataSO textData = ResourceManager.Inst.GetResource<MonologTextDataSO>(currentCallProfileData.defaultCallID);
        //MonologSystem.AddOnEndMonologEvent(textData.ID, SetSelectBtns);
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
        ShowAnswerButton(false);
        isRecieveCall = true;

        transform.DOKill(true);
        OnClickAnswerBtn?.Invoke();

        // 추후 문제가 생길 경우 변경을 시켜야한다
        OnClickAnswerBtn = null;
    }

}
