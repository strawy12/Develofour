using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constant
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
    }
    #endregion
}
