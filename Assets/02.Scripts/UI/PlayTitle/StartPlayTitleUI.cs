using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using System;

public class StartPlayTitleUI : MonoBehaviour
{
    [SerializeField]
    private Button startPlayButton;
    [SerializeField]
    private Button creditButton;
    [SerializeField]
    private Button exitButton;
    [SerializeField]
    private CreditPanel creditPanel;
    [SerializeField]
    private GameObject creditPopup;
    [SerializeField]
    private Button popupClose;
    [SerializeField]
    private Button showCredit;

    [SerializeField]
    private Button reStartBtn;

    [SerializeField]
    private SoundPanel soundPanel;

    [SerializeField]
    private WindowsLockScreen lockScreen;

    private void Start()
    {
        GameManager.Inst.OnStartCallback += StartCallBack;
    }

    private void StartCallBack()
    {
        gameObject.SetActive(true);

        startPlayButton.onClick?.AddListener(() => StartplayGame(true));

        if (!DataManager.Inst.SaveData.isNewStart)
        {
            reStartBtn.interactable = true;
            reStartBtn.onClick?.AddListener(() => StartplayGame(false));
        }

        creditButton.onClick?.AddListener(OnCreditButton);
        exitButton.onClick?.AddListener(ExitGame);
        popupClose.onClick?.AddListener(() => creditPopup.SetActive(false));
        showCredit.onClick?.AddListener(() => { creditPopup.SetActive(false); creditPanel.StartCredit(); });

        soundPanel.Init();
        lockScreen.Init();
        delayBGM();
    }

    private void delayBGM()
    {
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.AfterDiscordMail);
    }
    private void StartplayGame(bool isNewStart)
    {
        Sound.OnStopBGM?.Invoke(false);
        if (isNewStart)
        {
            DataManager.Inst.CreateSaveData();
        }
        else
        {
            Sound.OnImmediatelyStop(Sound.EAudioType.AfterDiscordMail);
            Sound.OnPlaySound?.Invoke(Sound.EAudioType.StartMainBGM);
        }

        //DataManager.Inst.SaveDefaultJson();

        DataManager.Inst.SaveData.isNewStart = false;

        StartCutScene.OnPlayCutScene?.Invoke();
        GameManager.Inst.OnGameStartCallback?.Invoke();
        this.gameObject.SetActive(false);
    }

    private void OnCreditButton()
    {
        creditPopup.SetActive(true);
    }

    private void ExitGame()
    {
        GameManager.Inst.GameQuit();
    }

}
