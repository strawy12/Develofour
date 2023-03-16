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

    public Button mediaPlayButton;
    public TMP_Text mediaPlayFileName;

    [SerializeField]
    private Image playImage;
    [SerializeField]
    private Image stopImage;

    private bool isPlayingMediaCheck;

    public void Init()
    {
        PlayButtonClick += PlayAudio;
        StopButtonClick += StopAudio;   

        mediaPlayButton.onClick?.AddListener(WhetherInPlayAudio);
    }

    private void WhetherInPlayAudio()
    {
        if(isPlayingMediaCheck)
        {
            PlayButtonClick?.Invoke();
        }
        else if(!isPlayingMediaCheck)
        {
            StopButtonClick?.Invoke();
        }
    }

    private void PlayAudio()
    {
        isPlayingMediaCheck = false;

        stopImage.gameObject.SetActive(true);
        playImage.gameObject.SetActive(false);
    }

    private void StopAudio()
    {
        isPlayingMediaCheck = true;

        playImage.gameObject.SetActive(true);
        stopImage.gameObject.SetActive(false);
    }

}
