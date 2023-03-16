using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MediaPlayer : Window
{
    [Header("MediaUI")]
    [SerializeField]
    private Slider mediaPlaySlider;
    [SerializeField]
    private TMP_Text mediaPlayTimeText;

    [Header("MediaAdditionalScripts")]
    [SerializeField]
    private MediaPlayerDownBar mediaPlayerDownBar;
    [SerializeField]
    private CdPlayMedia cdPlayMedia;

    private MediaPlayerDataSO mediaPlayerDataSO;

    private float playMediaValue;

    protected override void Init()
    {
        base.Init();

        mediaPlayerDataSO = ResourceManager.Inst.GetMediaPlayerData(file.GetFileLocation());

        mediaPlayerDownBar.Init();
        cdPlayMedia.Init();

        mediaPlayerDownBar.PlayButtonClick += PlayMediaPlayer;
        mediaPlayerDownBar.StopButtonClick += StopMediaPlayer;

        mediaPlayerDownBar.PlayButtonClick += cdPlayMedia.PlayCdAnimation;
        mediaPlayerDownBar.StopButtonClick += cdPlayMedia.StopCdAnimation;

        mediaPlayerDownBar.mediaPlayFileName.SetText(mediaPlayerDataSO.name);
    }

    private void PlayMediaPlayer()
    {

    }

    private void StopMediaPlayer()
    {

    }
}
