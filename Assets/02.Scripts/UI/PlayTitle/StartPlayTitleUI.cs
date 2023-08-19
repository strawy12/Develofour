using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

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

    private void Start()
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
        GameManager.Inst.OnStartCallback += delayBGM;
    }

    private async void delayBGM()
    {
        await Task.Delay(2000);
        Sound.OnPlaySound?.Invoke(Sound.EAudioType.AfterDiscordMail);
    }
    private void StartplayGame(bool isNewStart)
    {
        if (isNewStart)
        {
            DataManager.Inst.CreateSaveData();
        }

        StartCutScene.OnPlayCutScene?.Invoke();
        Sound.OnStopBGM?.Invoke(false);
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
