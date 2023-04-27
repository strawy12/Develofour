using System.Collections;
using System.Collections.Generic;
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

    #region File
    public const string USB_FILENAME = "BestUSB";
    public const string ZOOGLEPASSWORDLOCATION = "User\\C\\내 문서\\Zoogle\\Zoogle비밀번호\\";
    public const string ZOOGLEPINLOCATION = "User\\C\\내 문서\\Zoogle\\ZooglePIN번호\\";
    #endregion
    #region ProfileInfoKey
    public static class ProfileInfoKey
    {
        public const string SUSPECTRESIDENCE = "SuspectResidence";
        public const string VICTIMUNIVERSITY = "VictimUniversity";
        public const string SUSPECTNAME = "SuspectName";
    }
    #endregion
    #region SiteAddress
    public const string BranchNewPasswordSite = "https://branch/Branch-ResetPassword-a1cfrqvk5u8";
    #endregion
    #region MonologKey
    public static class MonologKey
    {
        public const int SUSPECTNAME = 1;
        public const int SUSPECTAGE = 2;
        public const int SUSPECTJOB = 3;
        public const int SUSPECTPERSONALTY = 4;
        public const int SUSPECTRELATIONWITHVICTIM = 5;
        public const int SUSPECTLIVINGWITHVICTIM = 6;
        public const int SUSPECTVICTIMREPORTDATE = 7;
        public const int REPORTERNAME = 8;
        public const int VICTIMNAME1 = 9;
        public const int REPORTERSTATEMENT = 10;
        public const int REPORTEREVIDENCEFILE1 = 11;
        public const int REPORTEREVIDENCEFILE2 = 12;
        public const int MISSINGCASENAME = 13;
        public const int SUSPECTEMAIL = 14;
        public const int PETBIRTH = 15;
        public const int PETTEMPORARYNAME1 = 16;
        public const int PETTYPE = 17;
        public const int PETTEMPORARYNAME2 = 18;
        public const int SUSPECTBIRTH = 19;
        public const int SUSPECTRESIDENCE = 20;
        public const int VICTIMNAME2 = 21;
        public const int PETNAME = 22;
        public const int PETPRESCRIPTION = 23;
        public const int MURDERWEAPONBUY = 24;
        public const int SUSPECTPHONENUM = 25;
        public const int CORPSEDISPOSALMAIL = 26;
        public const int SUSPECTALIBI = 27;
        public const int VICTIMCORPSELOCATION = 28;
        public const int VICTIMMISSINGTIME = 29;
        public const int SUSPECTDEPARTMENT = 30;
        public const int SUSPECTUNIVERSITY = 31;
        public const int CRIMINALGENDER = 32;
        public const int CRIMINALRELATIONWITHVICTIM = 33;
        public const int VICTIMUNIVERSITY = 34;
        public const int MURDERSEARCHHISTORY = 35;
        public const int ENDPROFILETUTORIAL = 36;
        public const int PROFILEINSTALLCOMPLETE = 37;
        public const int NOTEBOOKLOGINGUIDE = 38;
        public const int ONUSBFILE = 39;
        public const int STARTCUTSCENEMONOLOG1 = 40;
        public const int STARTCUTSCENEMONOLOG2 = 41;
        public const int STARTMONOLOG = 42;
        public const int STARTSEARCHTUTORIAL = 43;
        public const int WINDOWLOGINCOMPLETE = 44;
        public const int ONGUIDEMONOLOG1 = 45;
        public const int TUTORIALNOTFINDNAME = 46;
        public const int SUSPECTRESIDENCECALLTRIGGER = 47;
        public const int SUSPECTPHONENUM2 = 48;
        public const int UNIVERSITYDEFAULT = 49;
        public const int SUSPECTRESIDENCEWITHOUTNEEDINFO = 50;

    }
    #endregion
}
