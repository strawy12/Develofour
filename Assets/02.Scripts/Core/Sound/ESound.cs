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
        WindowAlarmSound,
        YoutubeHateBtnSound,
        YoutubeDefaultBtnSound,
        LoginFailed,
        LoginSuccess,
        PoliceMinigameArrowSuccess,
        PoliceMinigameArrowFailed,

        NewsAnchor_01,
        NewsAnchor_02,
        NewsAnchor_03,
        NewsAnchor_04,
        NewsAnchor_05,
        NewsAnchor_06,
        NewsAnchor_07,
        NewsAnchor_08,
        NewsAnchor_09,

        NewsReporter_01,
        NewsReporter_02,
        NewsReporter_03,
        NewsReporter_04,
        NewsReporter_05,
        NewsReporter_06,
        NewsReporter_07,
        NewsReporter_08,
        NewsReporter_09,
        NewsReporter_10,
        NewsReporter_11,
        NewsReporter_12,
        NewsReporter_13,
        NewsReporter_14,

        NotUseIconClick,

        Count
    }

}

