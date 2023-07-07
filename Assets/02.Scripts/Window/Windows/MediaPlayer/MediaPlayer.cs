using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Globalization;

public partial class MediaPlayer : Window
{
    [Header("MediaUI")]
    [SerializeField]
    private MediaPlayerSlider mediaPlaySlider;
    [SerializeField]
    private TMP_Text mediaPlayTimeText;

    [SerializeField]
    private MediaPlayerBody body;
    private TMP_Text mediaDetailText => body.mediaDetailText;
    private ScrollRect scroll => body.scroll;
    private RectTransform coverRectTrm => body.coverRectTrm;

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

    //private MediaPlayInfoFind infoFind;
    [SerializeField]
    private ProfileOverlayOpenTrigger overlayTrigger;

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

    [Header("WordSize")]
    [SerializeField]
    private float wordSizeY;

    protected override void Init()
    {
        base.Init();

        mediaPlayerData = ResourceManager.Inst.GetMediaPlayerData(file.ID);

        if(mediaPlayerData.body != null)
        {
            Transform parent = body.transform.parent;
            Destroy(body.gameObject);
            body = Instantiate(mediaPlayerData.body, parent);
        }

        body.Init(this);
        gameObject.SetActive(true);

        mediaPlayerDownBar.Init();
        cdPlayMedia.Init();
        ButtonActionInit();
        audioSource = GetComponent<AudioSource>();

        //mediaPlayerData = ResourceManager.Inst.GetMediaPlayerData(file.id);
        //infoFind = GetComponent<MediaPlayInfoFind>();
        //infoFind.Init(this);

        windowBar.OnMaximum.AddListener(body.SetPosition);

        mediaPlayerDownBar.mediaPlayFileName.SetText(mediaPlayerData.name);

        audioSource.clip = mediaPlayerData.mediaAudioClip;

        textParentRect = mediaDetailText.transform.parent.GetComponent<RectTransform>();
        InitDelayList();

        mediaPlaySlider.OnMousePointDown += MediaSliderDown;
        mediaPlaySlider.OnMousePointUp += PointUpMediaPlayer;
        mediaPlaySlider.OnMouseSlider += SetSliderMediaText;
        mediaDetailText.SetText(notCommandString);

        mediaDetailText.maxVisibleCharacters = 0;
        secondTimer = 0;
        minuteTimer = 0;

        isRePlaying = false;

        audioSource.Play();
        mediaPlayerDownBar.PlayButtonClick?.Invoke();

        OnSelected += OverlayOpen;
        OnUnSelected += OverlayClose;
        OverlayOpen();
    }

    public override void WindowOpen()
    {
        base.WindowOpen();
        body.SetPosition();
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
        if (isPlaying || isRePlaying)
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
        for (int i = mediaDetailText.maxVisibleCharacters; i < delayList.Count; i++)
        {
            mediaDetailText.maxVisibleCharacters++;

            TMP_CharacterInfo charInfo = mediaDetailText.textInfo.characterInfo[i];

            float height = (charInfo.bottomRight.y * -1f) - mediaDetailText.rectTransform.anchoredPosition.y;
            float parentHeight = (mediaDetailText.transform.parent as RectTransform).rect.height;
            if (parentHeight < height)
            {
                Vector2 pos = mediaDetailText.rectTransform.anchoredPosition;
                pos.y += Mathf.Abs(parentHeight - height);
                mediaDetailText.rectTransform.anchoredPosition = pos;
            }
            
            body.SetPositionCoverImage(charInfo);

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
        float time = (t* MediaLength); // 초


        //mediaDetailText.text = notCommandString.Substring(0, TimeToIndex(time));
        mediaDetailText.maxVisibleCharacters = TimeToIndex(time);
        body.SetPosition();

        minuteTimer = (int)(time / 60f);
        secondTimer = (int)(time % 60f);

        mediaPlaySlider.value = t;

        mediaPlayTimeText.SetText(string.Format("{0:00} : {1:00}", minuteTimer, secondTimer));

        if(isPlaying)
        {
            mediaDetailText.rectTransform.anchoredPosition = Vector2.zero;
        }

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

    private void OverlayClose()
    {
        if (overlayTrigger == null) // 없다면 찾아와
        {
            overlayTrigger = body.GetComponent<ProfileOverlayOpenTrigger>();
            if (overlayTrigger == null) { return; }
        }
        overlayTrigger.Close();
    }

    private void OverlayOpen()
    {
        if (overlayTrigger == null) // 없다면 찾아와
        {
            overlayTrigger = body.GetComponent<ProfileOverlayOpenTrigger>();
            if (overlayTrigger == null) { return; }
        }
        overlayTrigger.Open();
    }

    protected override void OnDestroyWindow()
    {
        base.OnDestroyWindow();
        OnSelected -= OverlayOpen;
        OnUnSelected -= OverlayClose;
    }
}
