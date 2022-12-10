using UnityEngine;

public partial class Sound : MonoBehaviour
{

    public enum EBgm
    {
        None = -1,
        WriterBGM = 0,
        NewsBGM,
        Count
    }

    public enum EEffect 
    {
        None = -1,
        Notice = 1001,
        Glitch,
        SpaceKeyDown,
        EscKeyDown,
        #region NewsAnchorVoices
        NewsAnchorVoice_01,
        NewsAnchorVoice_02,
        NewsAnchorVoice_03,
        NewsAnchorVoice_04,
        NewsAnchorVoice_05,
        NewsAnchorVoice_06,
        NewsAnchorVoice_07,
        NewsAnchorVoice_08,
        NewsAnchorVoice_09,
        #endregion
        WindowAlarmSound,
        YoutubeHateBtnSound,
        YoutubeDefaultBtnSound,
        LoginFailed,
        LoginSuccess,
        PoliceMinigameArrowSuccess,
        PoliceMinigameArrowFailed,
        Count
    }

}

