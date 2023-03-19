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
    private MediaPlayerSlider mediaPlaySlider;
    [SerializeField]
    private TMP_Text mediaPlayTimeText;

    [Header("MediaAdditionalScripts")]
    [SerializeField]
    private MediaPlayerDownBar mediaPlayerDownBar;
    [SerializeField]
    private CdPlayMedia cdPlayMedia;

    private MediaPlayerDataSO mediaPlayerData;

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

        mediaDetailText.maxVisibleCharacters = 0;

        mediaPlaySlider.OnMousePointDown += SetSliderMediaText;
        mediaPlaySlider.OnMousePointUp += PlayMediaPlayer;

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
        StartCoroutine(PrintMediaText());
        StartCoroutine(PrintTimerText());
    }

    private void StopMediaPlayer()
    {
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
        while (true)
        {
            secondTimer++;

            if(secondTimer >= 60)
            {
                minuteTimer++;
                secondTimer = 0;
            }

            PlaySettingSlider();

            mediaPlayTimeText.SetText(string.Format("{0:00} : {1:00}", minuteTimer, secondTimer));
            yield return new WaitForSeconds(1f);
        }
    }

    private void PlaySettingSlider()
    {
        int n = (minuteTimer * 60) + secondTimer;
        int m = (int)(mediaPlayerData.textPlaySpeed * mediaPlayerData.textData.Length);
        float t = (float)n / m;

        mediaPlaySlider.value = t;
    }

    private void SetSliderMediaText(float t)
    {
        StopAllCoroutines();
        
        int m = (int)(mediaPlayerData.textPlaySpeed * mediaPlayerData.textData.Length);
        int n = (int)(m * t); // √ 

        mediaDetailText.maxVisibleCharacters = (int)Mathf.Lerp(0, mediaPlayerData.textData.Length, t);

        minuteTimer = n / 60;
        secondTimer = n % 60;

        mediaPlaySlider.value = t;

        mediaPlayTimeText.SetText(string.Format("{0:00} : {1:00}", minuteTimer, secondTimer));
    }
}
