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

    public const string CALLLOGLOCATION = "내 PC\\CallLog\\";
    public static readonly System.DateTime DEFAULTDATE = new System.DateTime(2023, 10, 22, 7, 3, 0);
    #region File
    public static class FileID
    {
        public const int USB = 6;
        public const int ZOOGLEPIN = 23;
        public const int ZOOGLEPASSWORD = 24;
    }


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
        public const int YEONGOKIDNAPPINGTITLE = 1;
        public const int KIDNAPPINGREPORTTIME = 2;
        public const int VICTIMNAME = 3;
        public const int VICTIMUNIVERSITYSTATE = 4;
        public const int REPORTERNAME = 5;
        public const int CASETESTIMONY = 6;
        public const int HAVERECORDINGDATA = 7;
        public const int ASSAULTBRUISEDPHOTOOPEN = 8;
        public const int ASSAULTBRUISEDPHOTOCLICK = 9;
        public const int UNIVERSITYNAMEINFOACQ = 10;
        public const int SUSPECTDEPARTMENTINFO = 11;
        public const int VICTIMDEPARTMENT = 12;
        public const int OTHERDEPARTMENT = 13;
        public const int PETHOSPITALINFO = 14;
        public const int PETNAMEGET = 15;
        public const int DATEHITTESTIMONYDIARY = 17;
        public const int VICTIMINVESTIGATIONACQ = 18;
        public const int VICTIMANDOTHERGIRLSQCQ = 19;
        public const int ALONEBACKTOHOME = 20;
        public const int COUPLELIVINGTOGETHERACQ = 21;
        public const int CONSULTATIONAGREEMENTTITLE = 22;
        public const int GROUPASSIGNMENTANDGIRLFRIEND = 23;
        public const int GIRLFRIENDTOGETHERNOTDATE = 24;
        public const int CHATTINGWITHFRIENDS = 25;
        public const int PETPRESCRIPTIONTITLE = 26;
        public const int SUSPECTHOMENAME = 27;
        public const int PETGUARDIANSUSPECT = 29;
        public const int COUPLETOFIGHTVIDEO = 30;
        public const int VICTIMDIETIMECLICK = 31;
        public const int CORPSEAUTOPSYDATA_1_TRUE = 32;
        public const int CORPSEAUTOPSYDATA_1_NOT = 33;
        public const int CORPSEAUTOPSYDATA_2_TRUE = 34;
        public const int CORPSEAUTOPSYDATA_2_NOT = 35;
        public const int BODYCONDITIONSPECIALS = 36;
        public const int VICTIMBODYSIGN = 37;
        public const int SUSPECTNAMEGET = 38;
        public const int VICTIMNAMEGET = 39;
        public const int SUSPECTZOOGLEEMAILGET = 40;
        public const int CCTVENDVIEWVIDEO = 41;
        public const int VICTIMLASTCALLING = 42;
        public const int FANSIGNINGEVENTTOGETHER = 43;
        public const int LIKEGIRLGROUPNEWSSERAFIM = 44;
        public const int CURFEWTIMEACQ = 45;
        public const int ALUMINUMBATBUYMAIL = 46;
        public const int ETERNALLANDPHOTOOPEN = 47;
        public const int PETGRASSFIELDWALKDIARY = 46;
        public const int ZOOGLEPASSWORDGET = 49;
        public const int GIRLFRIENDCALLTOMARCH19 = 50;
        public const int NEWSSERAFIMALBUMBUYPHOTO = 51;
        public const int ALBUMPHOTOTODIARY = 52;
        public const int MENTALDRUGPRESCRIPTION = 53;
        public const int DRUGTYPEPHOTO = 54;
        public const int OBTAINBODYLOCATION = 55;
        public const int FIRSTCONVERSATIONACCOMPLICE = 56;
        public const int DATEENFORCEMENT = 57;
        public const int SUSPECTCONSENTACQ = 58;
        public const int FINDLOCATEBODY = 59;
        public const int EXPENSIVEBUYGIRLFRIENDGIFTS = 60;
        public const int EXPENSIVEBUYGIRLSTUFFACQ = 61;
        public const int MURDERWEAPONFIND = 62;
        public const int CONVERSATIONSFEMALECHAT = 63;
        public const int PETHOSPITALHARMONYCHAT = 64;
        public const int MURDERBODYLOCATIONGLOVES = 65;
        public const int JUYOUNGRESENTMENT = 66;
        public const int CONFESSIONTOCOUPLE = 67;
        public const int HITFORGIRLFRIEND = 68;
        public const int YUJINPROPOSAL = 69;
        public const int FEMALECHATDAYTODIARY1 = 70;
        public const int FEMALECHATDAYTODIARY2 = 71;
        public const int ASSISTANTDEFAULT = 72;
        public const int ASSISTANTEVIDENCE = 73;
        public const int ASSISTANTFINDUNIVERSITY = 74;
        public const int ASSISTANTFINDSUSPECTHOME = 75;
        public const int ASSISTANTFINDMOUNTAIN = 76;
        public const int ASSISTANTFINDALLEY = 77;
        public const int ASSISTANTRECEIVEUNIVERSITY = 78;
        public const int ASSISTANTRECEIVESUSPECTHOME = 79;
        public const int ASSISTANTRECEIVEMOUNTAIN = 80;
        public const int ASSISTANTRECEIVEALLEY = 81;
        public const int ASSISTANTNOTEXIST = 82;
        public const int YUJINDEFAULT = 83;
        public const int YUJINRELATIONWITHJUYOUNG = 84;
        public const int YUJINWHATDOING = 85;
        public const int YUJINWHYCALLING = 86;
        public const int YUJINNOTEXIST = 87;
        public const int POLICEDEFAULT = 88;
        public const int POLICEJUYOUNGMURDER = 89;
        public const int POLICEJUYOUNGWHEREABOUTS = 90;
        public const int POLICERELATIONWITHYUJIN = 91;
        public const int POLICEYOHANALIBI = 92;
        public const int POLICENOTEXIST = 93;
        public const int PARKJUYOUNGDEFAULT = 94;
        public const int PARKJUYOUNGPERSONALITY = 95;
        public const int PARKJUYOUNGPAST = 96;
        public const int PARKJUYOUNGNOTEXIST = 97;
        public const int TAEWOONGDEFAULT = 98;
        public const int TAEWOONGYOHANQUESTION = 99;
        public const int TAEWOONGJUYOUNGPARTY = 100;
        public const int TAEWOONGNOTEXIST = 101;
        public const int SECURITYDEFAULT = 102;
        public const int SECURITYYOHANWHEREABOUTS = 103;
        public const int SECURITYNOTEXIST = 104;
        public const int SECONDCOUNSELINGDETAIL = 105;
        public const int SECONDCOUNSELINGRESULT = 106;
        public const int JANUARY6TODIARYINTALK = 107;
        public const int STARTCUTSCENE1 = 108;
        public const int STARTCUTSCENE2 = 109;
        public const int WINDOWLOGINCOMPLETELOG = 110;
        public const int INSTALLCOMPLETE = 111;
        public const int ENDPROFILETUTORIALCHATLOG = 112;
        public const int WINDOWLOGINUIOPENLOG = 113;
        public const int LIBRARYNOTOPENGUIDELOG = 114;
        public const int HARMONYIDGET = 115;
        public const int HARMONYPASSWORDGET = 116;
        public const int BRANCHPASSWORDGET = 117;
        public const int JUYOUNGARGUESTART = 118;
        public const int YOHANWORRYBREAKUP = 119;
        public const int CONFESSED = 120;
        public const int HITBYGIRLFRIEND = 121;
        public const int INCIDENTREPORTREPORTTIME = 122;
        public const int LOVEFORDOG = 123;
        public const int NEWSSERAFIMFANMEETING = 124;
        public const int AZIDEAD = 125;
        public const int OFFERYUJIN = 127;
        public const int DEADJUYOUNG = 128;
        public const int WHOISYUJIN = 129;
        public const int PCPASSWORD = 130;
    }
    #endregion
}