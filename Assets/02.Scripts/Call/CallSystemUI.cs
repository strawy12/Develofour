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

    private CallProfileDataSO currentCallProfileData;
    private bool isRecieveCall;

    public Action OnClickAnswerBtn;

    public static bool isCanClick = true;

    public void Init()
    {
        transform.DOScale(Vector3.zero, 0);
    }

    public void Hide()
    {
        transform.DOKill(true);
        Sound.OnImmediatelyStop(Sound.EAudioType.PhoneCall);
        GameManager.Inst.ChangeGameState(EGameState.Game);

        StopAllCoroutines();
        //transform.DOLocalMoveX(1200, 0.5f).SetEase(Ease.Linear);
        transform.DOScale(Vector3.zero, 0.3f).SetEase(Ease.OutCirc);
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

        // AnswerButton 셋팅
        StartCoroutine(PlayPhoneSoundAndShake());

    }

    public void OutGoingCall(CallProfileDataSO data)
    {
        currentCallProfileData = data;
        InitCallUI();

        ShowAnswerButton(false);

        StartCoroutine(PlayPhoneCallSound(currentCallProfileData.delay));
    }

    private IEnumerator PlayPhoneCallSound(float delay)
    {
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCirc);
        yield return new WaitForSeconds(delay);
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
        if (currentCallProfileData.id == Constant.CharacterKey.MISSING)
        {
            EventManager.TriggerEvent(ECallEvent.EndCall);
            Hide();
            yield break;
        }
        RecivivedCall();
    }

    private IEnumerator PlayPhoneSoundAndShake()
    {
        transform.DOScale(Vector3.one, 0.3f).SetEase(Ease.OutCirc);
        yield return new WaitForSeconds(0.3f);
        while (!isRecieveCall)
        {
            transform.DOKill(true);
            transform.DOShakePosition(2.5f, 5);
            if (GameManager.Inst.GameState != EGameState.Text)
            {
                float soundSecond = (float)Sound.OnPlaySound?.Invoke(Sound.EAudioType.PhoneAlarm);
                yield return new WaitForSeconds(soundSecond + Constant.PHONECALLSOUND_DELAY);
            }
            else
            {
                yield return new WaitForSeconds(4f);
            }
        }
        isRecieveCall = false;
        Sound.OnImmediatelyStop?.Invoke(Sound.EAudioType.PhoneAlarm);
    }
    private void RecivivedCall()
    {
        CallWindow window = WindowManager.Inst.WindowOpen(EWindowType.CallWindow) as CallWindow;

        window.Setting(currentCallProfileData);
        Hide();
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
        if (!isCanClick)
            return;

        ShowAnswerButton(false);
        isRecieveCall = true;

        transform.DOKill(true);
        OnClickAnswerBtn?.Invoke();

        // 추후 문제가 생길 경우 변경을 시켜야한다
        OnClickAnswerBtn = null;
        Hide();
    }

}
