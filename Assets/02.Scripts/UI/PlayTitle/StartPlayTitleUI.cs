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

    private void Start()
    {
        gameObject.SetActive(true);

        startPlayButton.onClick?.AddListener(StartplayGame);
        creditButton.onClick?.AddListener(OnCreditButton);
    }

    private void StartplayGame()
    {

    }

    private void OnCreditButton()
    {

    }

}
