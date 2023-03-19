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
        PlayButtonClick += PlayAudio;
        StopButtonClick += StopAudio;

        playButton.onClick?.AddListener(PlayAudio);
        stopButton.onClick?.AddListener(StopAudio);
    }

    private void PlayAudio()
    {
        stopButton.gameObject.SetActive(true);
        playButton.gameObject.SetActive(false);
    }

    private void StopAudio()
    {
        playButton.gameObject.SetActive(true);
        stopButton.gameObject.SetActive(false);
    }

}
