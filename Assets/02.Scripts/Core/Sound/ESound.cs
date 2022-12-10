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
        Reporter_01,
        Reporter_02,
        Reporter_03,
        Reporter_04,
        Reporter_05,
        Reporter_06,
        Reporter_07,
        Reporter_08,
        Reporter_09,
        Reporter_10,
        Reporter_11,
        Reporter_12,
        Reporter_13,
        Reporter_14,
        Count
    }

}

