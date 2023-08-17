using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private void Start()
    {
        gameObject.SetActive(true);

        startPlayButton.onClick?.AddListener(StartplayGame);
        creditButton.onClick?.AddListener(OnCreditButton);
        exitButton.onClick?.AddListener(ExitGame);
        popupClose.onClick?.AddListener(() => creditPopup.SetActive(false));
        showCredit.onClick?.AddListener(() => { creditPopup.SetActive(false); creditPanel.StartCredit(); });
    }

    private void StartplayGame()
    {
        StartCutScene.OnPlayCutScene?.Invoke();
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
