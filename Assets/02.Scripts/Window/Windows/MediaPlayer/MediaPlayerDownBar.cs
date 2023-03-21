using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MediaPlayerDownBar : MonoBehaviour
{
    public Action PlayButtonClick;
    public Action StopButtonClick;

    public TMP_Text mediaPlayFileName;

    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button stopButton;

    public void Init()
    {
        PlayButtonClick += PlayButtonChange;
        StopButtonClick += StopButtonChange;

        playButton.onClick?.AddListener(PlayClick);
        stopButton.onClick?.AddListener(StopClick);
    }

    private void PlayClick()
    {
        PlayButtonClick?.Invoke();
    }

    private void StopClick()
    {
        StopButtonClick?.Invoke();
    }

    private void PlayButtonChange()
    {
        playButton.gameObject.SetActive(false);
        stopButton.gameObject.SetActive(true);
    }

    private void StopButtonChange()
    {
        playButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(false);
    }

}
