using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JetBrains.Annotations;
using static System.Net.Mime.MediaTypeNames;

public class MediaPlayer : Window
{
    [Header("MediaUI")]
    [SerializeField]
    private TMP_Text mediaDetailText;
    [SerializeField]
    private Slider mediaPlaySlider;
    [SerializeField]
    private TMP_Text mediaPlayTimeText;

    [Header("MediaAdditionalScripts")]
    [SerializeField]
    private MediaPlayerDownBar mediaPlayerDownBar;
    [SerializeField]
    private CdPlayMedia cdPlayMedia;

    private MediaPlayerDataSO mediaPlayerData;

    private int saveVisibleCharacters = 0;

    private int secondTimer;
    private int minuteTimer;

    protected override void Init()
    {
        base.Init();

        gameObject.SetActive(true);

        mediaPlayerDownBar.Init();
        cdPlayMedia.Init();
        ButtonActionInit();

        mediaPlayerData = ResourceManager.Inst.GetMediaPlayerData(file.GetFileLocation());

        mediaPlayerDownBar.mediaPlayFileName.SetText(mediaPlayerData.name);
        mediaDetailText.SetText(mediaPlayerData.textData);

        saveVisibleCharacters = mediaDetailText.maxVisibleCharacters;
        mediaDetailText.maxVisibleCharacters = 0;

        secondTimer = 0;
        minuteTimer = 0;    

        PlayMediaPlayer();
    }

    private void ButtonActionInit()
    {
        mediaPlayerDownBar.PlayButtonClick += PlayMediaPlayer;
        mediaPlayerDownBar.StopButtonClick += StopMediaPlayer;

        mediaPlayerDownBar.PlayButtonClick += cdPlayMedia.PlayCdAnimation;
        mediaPlayerDownBar.StopButtonClick += cdPlayMedia.StopCdAnimation;
    }

    private void PlayMediaPlayer()
    {
        StartCoroutine("PrintMediaText");
        StartCoroutine("PrintTimerText");
    }

    private void StopMediaPlayer()
    {
        Debug.Log("Stop?");
        StopAllCoroutines();
    }

    private IEnumerator PrintMediaText()
    {
        for (int i = 0; i < mediaPlayerData.textData.Length; i++)
        {
            mediaDetailText.maxVisibleCharacters++;

            yield return new WaitForSeconds(mediaPlayerData.textPlaySpeed);
        }
    }

    private IEnumerator PrintTimerText()
    {
        while(true)
        {
            secondTimer++;

            if(secondTimer >= 60)
            {
                minuteTimer++;
                secondTimer = 0;
            }

            SettingSlider();
            
            mediaPlayTimeText.SetText(minuteTimer.ToString() + " : " + secondTimer.ToString());
            yield return new WaitForSeconds(1f);
        }
    }

    private void SettingSlider()
    {
        int n = (minuteTimer * 60) + secondTimer;
        int m = mediaPlayerData.length;
        float t = (float)n / m;

        mediaPlaySlider.value = t;
    }
}
