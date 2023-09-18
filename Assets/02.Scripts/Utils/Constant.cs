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
    public const float INCOMMING_CHECK_DELAY = 8f;
    public const float PHONECALLSOUND_DELAY = 1.5f;

    public static readonly System.DateTime DEFAULTDATE = new System.DateTime(2023, 10, 22, 7, 3, 0);

    public const string ZOOGLEPASSWORDGUIDE = "주글비밀번호가이드";

    //public const int NEED_INFO_MONOLOG_ID = 131;

    #region File
    public static class FileID
    {
        public const string MYPC = "F_DR_1";
        public const string C = "F_DR_2";
        public const string D = "F_DR_3";
        public const string USB = "F_DR_4";
        public const string BACKGROUND = "F_DR_6";
        public const string DOWNLOAD = "F_DR_18";
        public const string PROFILER = "F_PF_0";
        public const string ZOOGLEPIN = "";
        public const string ZOOGLEPASSWORD = "";
        public const string CALLRECORDING = "F_DR_25";
        public const string INCIDENT_REPORT = "F_IV_1";
        public const string INSTALL = "F_IN_0";
        public const string Test = "F_DR_64";
        public const string INCIDENT_MAP = "F_IV_4";
    }
        public const string NOW_USB_CHECK = "T_M_96";


    #endregion
    #region ProfilerInfoKey
    public static class ProfilerInfoKey
    {
        public const string PARKJUYOUNG_NAME = "I_C_2_1";
        public const string PARKJUYOUNG_INCIDENT = "I_I_1_3";
        public const string KIMYUJIN_NAME = "I_C_3_1";
        public const string CRIMINAL_ACTION = "";
        public const string BAT_DETAIL = "";
        public const string CCTV_TIME = "";
        public const string CCTV_DETAIL = "";
        public const string KANGYOHAN_PHONENUMBER = "";
        public const string BRANCHID = "";
        public const string ZOOGLEPASSWORD = "";
        public const string HARMONY_PASSWORD = "";
        public const string INCIDENTREPORT_TITLE = "I_I_1_1";
        public const string CCTV_UYOUNGWHEREABOUTS = "";
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
        public const string STARTCUTSCENE_1 = "T_CS_ST_1";
        public const string STARTCUTSCENE_2 = "T_CS_ST_2";
        public const string STARTCUTSCENE_3 = "T_CS_ST_3";
        public const string STARTCUTSCENE_4 = "T_CS_ST_4";
        public const string STARTCUTSCENE_5 = "T_CS_ST_5";
        public const string WINDOWS_LOGIN_SCREEN_OPEN = "T_CS_ST_6";

        public const string LIBRARY_NOT_OPEN = "T_M_73";
        public const string NEEDINFO = "T_M_88";
        public const string TUTORIAL_NOT_FIND_INFO = "T_M_98";
        public const string PETCAM_CUTSCENE_1 = "T_CS_P_1";
        public const string PETCAM_CUTSCENE_2 = "T_CS_P_2";
        public const string PETCAM_CUTSCENE_3 = "T_CS_P_3";
        public const string PETCAM_CUTSCENE_4 = "T_CS_P_4";
        public const string CCTV_CUTSCENE_00 = "T_CS_C_1";
        public const string CCTV_CUTSCENE_01 = "T_CS_C_2";
        public const string CCTV_CUTSCENE_02 = "T_CS_C_3";
        public const string CCTV_CUTSCENE_03 = "T_CS_C_4";
        public const string CCTV_CUTSCENE_04 = "T_CS_C_5";

        //public const string FIRST_LOGIN_GUIDE = "T_M_155";
        //public const string COMPLETE_OVERLAY = "T_M_156";

        public const string TUTORIAL_CANNOT_GETINFO = "T_M_22";
        public const string TUTORIAL_CANNOT_GETINFO_CALL = "T_M_23";

        public const string Lock_HINT = "T_M_99";

        public const string TUTORIAL_CALL_ASSISTANT = "T_C_A_4";

        public const string NOW_USB_CHECK = "T_M_96";

    }
    #endregion

    public static class CallScreenMonologID
    {
        public const string POLICE_UNIVERSITY_CALLSCREEN_START = "T_C_P_7_1";
        public const string POLICE_UNIVERSITY_CALLSCREEN_PRESENT = "T_C_P_7_2";
        public const string POLICE_UNIVERSITY_CALLSCREEN_WRONG = "T_C_P_7_3";
        public const string POLICE_UNIVERSITY_CALLSCREEN_WRONG_HINT = "T_C_P_7_4";
        public const string POLICE_UNIVERSITY_CALLSCREEN_ANSWER = "T_C_P_7_5";

    }

    #region MailKey

    public static class MailKey
    {
        public const int BACKGROUND = 1;
        public const int BRANCH_CERTIFICATION = 2;
        public const int ORDERLIST = 3;
    }

    #endregion

    public static class OverlayID //파일이 아닌 것들의 정보들
    {
        public const int START_ATTRIBUTE = -1;
        public const int NOTIFICATION_SYSTEM = -2;
        public const int ORDER_LIST_MAIL = -3;
        public const int BACKGROUND_MAIL = -4;
        public const int HARMONY_ANIMALHOSPITAL = -5;
        public const int HARMONY_TAEWOONG = -6;
        public const int HARMONY_YUZIN = -7;
        public const int HARMONY_JUYOUNG = -8;
        public const int HARMONY_UNKNOWN = -9;
    }                

    public static class CallDataKey
    {
        public const string TUTORIAL_ASSISTANT_CALL = "C_A_I_1";
    }

    public static class CharacterKey
    {
        public const string ASSISTANT = "CD_AS";
        public const string MISSING = "CD_MS";
    }

    public static class ProfilerCategoryKey
    {
        public const string PETCAM = "IC_I_2";
        public const string INCIDENT = "IC_I_1";
        public static bool CheckInvisible(string id)
        {
            string[] arr = id.Split('_');
            if(arr[1] == "v")
            {
                return true;
            }
            return false;
        }
    }

}