using Unity.VisualScripting;
using UnityEngine;

public static partial class Constant
{
    public static readonly Vector2 MAX_CANVAS_SIZE = new Vector2(1920, 1080);
    public static readonly Vector2 MAX_CANVAS_POS = new Vector2(960, 540);
    public static readonly Vector2 WINDOWICONSIZE = new Vector2(100, 100);
    public static readonly Vector2 WINDOWDEFAULTPOS = new Vector2(-890, 460);
    public static readonly Vector2 WINDOWMAXIMUMPOS = new Vector2(0, 25);

    public static readonly Vector2 NOTICE_POS = new Vector2(-17.5f, -470f);

    public static readonly float NOTICEDRAG_INVISIBILITY = 65.5f;
    public static readonly float NOTICEDRAG_INTERVAL = -45f;

    public static readonly float NOTICE_DELAYTIME = 5f;
    public static readonly float NOTICE_DURATION = 0.3f;
    public static readonly float NOTICE_SIZE_DURATION = 0.13f;
    public static readonly EWindowType BROWSER_KEY = EWindowType.Browser;
    public const int NOWYEAR = 2023;
    public const int NOWMONTH = 10;
    public const int NOWDAY = 23;
    public const float LOADING_DELAY = 0.75f;
    public const float POLICE_REPLY_DELAY = 2f;

    public static readonly System.DateTime DEFAULTDATE = new System.DateTime(2023, 10, 22, 7, 3, 0);

    public const string ZOOGLEPASSWORDGUIDE = "ZooglePasswordGuide";

    //public const int NEED_INFO_MONOLOG_ID = 131;
    #region File
    public static class FileID
    {
        public const int BACKGROUND = 1;
        public const int USB = 6;
        public const int DOWNLOAD = 10;
        public const int PROFILER = 49;
        public const int ZOOGLEPIN = 23;
        public const int ZOOGLEPASSWORD = 24;
        public const int CALLRECORDING = 89;
    }


    #endregion
    #region ProfilerInfoKey
    public static class ProfilerInfoKey
    {
        public const int PARKJUYOUNG_NAME = 7;
        public const int KIMYUJIN_NAME = 11;
        public const int CRIMINAL_ACTION = 23;
        public const int BAT_DETAIL = 40;
        public const int CCTV_TIME = 47;
        public const int CCTV_DETAIL = 48;
        public const int KANGYOHAN_PHONENUMBER = 66;
        public const int BRANCHID = 67;
        public const int ZOOGLEPASSWORD = 68;
        public const int HARMONY_PASSWORD = 69;
        public const int INCIDENTREPORT_TITLE = 76;
        public const int CCTV_UYOUNGWHEREABOUTS = 107;
    }
    #endregion
    #region SiteAddress
    public const string BranchNewPasswordSite = "https://branch/Branch-ResetPassword-a1cfrqvk5u8";
    public const string ZMailSite = "https://mail.zoogle.com/mail/";
    public const string BranchSite = "https://branch.com/";
    public const string LoginSite = "https://accounts.zoogle.com/v3/signin";
    #endregion
    #region MonologKey
    public static class MonologKey
    {
        public const int STARTCUTSCENE_1 = 108;
        public const int STARTCUTSCENE_2 = 109;
        public const int WINDOWS_LOGIN_COMPLETE = 110;
        public const int PROFILER_INSTALL_COMPLETE = 111;
        public const int END_PROFILER_TUTORIAL = 112;
        public const int WINDOWS_LOGIN_SCREEN_OPEN = 113;
        public const int LIBRARY_NOT_OPEN = 113;
        public const int NEEDINFO = 131;
        public const int TUTORIAL_NOT_FIND_INFO = 134;
        public const int PETCAM_CUTSCENE_1 = 174;
        public const int PETCAM_CUTSCENE_2 = 175;
        public const int PETCAM_CUTSCENE_3 = 176;
        public const int PETCAM_CUTSCENE_4 = 177;
        public const int CCTV_CUTSCENE_00 = 181;
        public const int CCTV_CUTSCENE_01 = 182;
        public const int CCTV_CUTSCENE_02 = 183;
        public const int CCTV_CUTSCENE_03 = 184;
        public const int CCTV_CUTSCENE_04 = 185;
    }
    #endregion
    #region MailKey

    public static class MailKey
    {
        public const int BACKGROUND = 1;
        public const int BRANCH_CERTIFICATION = 2;
        public const int ORDERLIST = 3;
    }

    #endregion

}