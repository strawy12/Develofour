using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public partial class MediaPlayer : Window
{
    [Header("MediaUI")]
    [SerializeField]
    private TMP_Text mediaDetailText;
    [SerializeField]
    private MediaPlayerSlider mediaPlaySlider;
    [SerializeField]
    private TMP_Text mediaPlayTimeText;
    [SerializeField]
    private ScrollRect scroll;

    [Header("MediaAdditionalScripts")]
    [SerializeField]
    private MediaPlayerDownBar mediaPlayerDownBar;
    [SerializeField]
    private CdPlayMedia cdPlayMedia;

    public MediaPlayerDataSO mediaPlayerData;
    private RectTransform textParentRect;

    private AudioSource audioSource;

    private int secondTimer;
    private int minuteTimer;

    private string notCommandString;
    private bool isPlaying;

    public Action OnEnd;

    private MediaPlayInfoFind infoFind;
    private float MediaLength
    {
        get
        {
            if (mediaPlayerData.mediaAudioClip != null)
                return mediaPlayerData.mediaAudioClip.length;
            else
                return allDelay;
        }
    }

    private bool isRePlaying;

    private void BottomScrollView()
    {
        
        if(textParentRect.rect.height < 700) 
        {
            if (mediaDetailText.rectTransform.rect.height <= textParentRect.rect.height)
            {
                StartCoroutine(ScrollToTop());
            }
            else
            {
                StartCoroutine(ScrollToBottom());
            }
        }
        else
        {
            if (mediaDetailText.rectTransform.rect.height <= 770)
            {
                StartCoroutine(ScrollToTop());
            }
            else
            {
                StartCoroutine(ScrollToBottom());
            }
        } 
    }

    IEnumerator ScrollToTop()
    {
        yield return new WaitForEndOfFrame();
        scroll.gameObject.SetActive(true);
        scroll.verticalNormalizedPosition = 1f;
    }

    IEnumerator ScrollToBottom()
    {
        yield return new WaitForEndOfFrame();
        scroll.gameObject.SetActive(true);
        scroll.verticalNormalizedPosition = 0f;
    }
    protected override void Init()
    {
        base.Init();

        gameObject.SetActive(true);

        mediaPlayerDownBar.Init();
        cdPlayMedia.Init();
        ButtonActionInit();
        audioSource = GetComponent<AudioSource>();
        mediaPlayerData = ResourceManager.Inst.GetMediaPlayerData(file.GetFileLocation());
        infoFind = GetComponent<MediaPlayInfoFind>();
        infoFind.Init(this);

        mediaPlayerDownBar.mediaPlayFileName.SetText(mediaPlayerData.name);

        audioSource.clip = mediaPlayerData.mediaAudioClip;
        mediaDetailText.SetText("");

        textParentRect = mediaDetailText.transform.parent.GetComponent<RectTransform>();

        InitDelayList();
        BottomScrollView();
        mediaPlaySlider.OnMousePointDown += MediaSliderDown;
        mediaPlaySlider.OnMousePointUp += PointUpMediaPlayer;
        mediaPlaySlider.OnMouseSlider += SetSliderMediaText;

        secondTimer = 0;
        minuteTimer = 0;

        isRePlaying = false;

        audioSource.Play();
        mediaPlayerDownBar.PlayButtonClick?.Invoke();
    }

    private void ButtonActionInit()
    {
        mediaPlayerDownBar.PlayButtonClick += PlayMediaPlayer;
        mediaPlayerDownBar.StopButtonClick += StopMediaPlayer;

        mediaPlayerDownBar.PlayButtonClick += cdPlayMedia.PlayCdAnimation;
        mediaPlayerDownBar.StopButtonClick += cdPlayMedia.StopCdAnimation;
    }

    private void PlayMediaPlayer() // 플레이 
    {
        isPlaying = true;
        cdPlayMedia.PlayCdAnimation();
        audioSource.UnPause();
        StopAllCoroutines();
        StartCoroutine(PrintMediaText());
        StartCoroutine(PrintTimerText());
    }

    private void PointUpMediaPlayer(float value)
    {
        SetSliderMediaText(value);
        audioSource.time = minuteTimer * 60f + secondTimer;
        if (isPlaying)
        {
            PlayMediaPlayer();
        }
        else
        {
            StopMediaPlayer();
        }
    }

    private void StopMediaPlayer()
    {
        audioSource.Pause();
        isPlaying = false;
        StopAllCoroutines();
    }

    private IEnumerator PrintMediaText()
    {
        for (int i = mediaDetailText.text.Length; i < delayList.Count; i++)
        {
            mediaDetailText.text += notCommandString[i];

            BottomScrollView();

            yield return new WaitForSeconds(delayList[i]);
        }

    }

    private IEnumerator PrintTimerText()
    {
        while (minuteTimer * 60f + secondTimer < MediaLength)
        {
            secondTimer++;

            if (secondTimer >= 60)
            {
                minuteTimer++;
                secondTimer = 0;
            }

            PlaySettingSlider();

            mediaPlayTimeText.SetText(string.Format("{0:00} : {1:00}", minuteTimer, secondTimer));
            yield return new WaitForSeconds(1f);
        }

        isRePlaying = true;
        OnEnd?.Invoke();
        mediaPlayerDownBar.StopButtonClick?.Invoke();

    }

    private void PlaySettingSlider()
    {
        int n = (minuteTimer * 60) + secondTimer;
        int m = (int)MediaLength;
        float t = (float)n / m;

        mediaPlaySlider.value = t;
    }

    private void SetSliderMediaText(float t)
    {
        int m = (int)MediaLength;
        float time = (m * t); // 초


        mediaDetailText.text = notCommandString.Substring(0, TimeToIndex(time));

        minuteTimer = (int)(time / 60f);
        secondTimer = (int)(time % 60f);

        mediaPlaySlider.value = t;

        mediaPlayTimeText.SetText(string.Format("{0:00} : {1:00}", minuteTimer, secondTimer));

        BottomScrollView();

        if (isRePlaying)
        {
            mediaPlayerDownBar.PlayButtonClick?.Invoke();
            isRePlaying = false;
        }
    }

    private void MediaSliderDown()
    {
        cdPlayMedia.StopCdAnimation();
        audioSource.Pause();
        StopAllCoroutines();
    }

}
